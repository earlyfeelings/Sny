﻿
using Microsoft.Extensions.Options;
using Sny.Api.Options;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Core.Options;
using Sny.Kernel.Extensions;
using System.Net.Mail;


namespace Sny.Core.AccountsAggregate.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountReadOnlyRepo _arop;
        private readonly IAccountProviderRepo _apr;

        private static SemaphoreSlim _addAccountLock = new SemaphoreSlim(1, 1);
        private PasswordOptions _passwordOptions = default!;
        public AccountManager(IAccountReadOnlyRepo arop, 
            IAccountProviderRepo apr,
            IOptions<PasswordOptions> passwordOptions)
        {
            this._arop = arop;
            this._apr = apr;
            this._passwordOptions = passwordOptions.Value;
        }

        public async Task<(LoginResult Result, Account Account)> Login(LoginModel model)
        {
            var acc = await _arop.FindAcountByEmail(model.Email);
            if (acc == null) throw new LoginFailedException();

            var hash = await _arop.FindAcountPasswordHash(acc.Id);

            if (Hashing.HashPassword(model.Password, _passwordOptions.HashSalt) == hash)
                return (new LoginResult(true), acc);

            throw new LoginFailedException();
        }

        public async Task<RegisterResult> Register(RegisterModel model)
        {
            if (model.Password != model.PasswordAgain)
                return new RegisterResult(RegisterStatus.PasswordsNotSame);

            if (model.Password.Length < 3)
                return new RegisterResult(RegisterStatus.WeakPassword);

            try
            {
                MailAddress address = new MailAddress(model.Email);
            }
            catch 
            {
                return new RegisterResult(RegisterStatus.BadEmailFormat);
            }

            var acc = await _arop.FindAcountByEmail(model.Email);
            if (acc != null) return new RegisterResult(RegisterStatus.AlreadyExists);

            await _addAccountLock.WaitAsync();
            try
            {
                var accInLock = await _arop.FindAcountByEmail(model.Email);
                if (accInLock != null) return new RegisterResult(RegisterStatus.AlreadyExists);

                _apr.AddAccount(model.Email, Hashing.HashPassword(model.Password, _passwordOptions.HashSalt));

                return new RegisterResult(RegisterStatus.Success);
            }
            finally
            {
               _addAccountLock.Release();
            }
        }
    }
}
