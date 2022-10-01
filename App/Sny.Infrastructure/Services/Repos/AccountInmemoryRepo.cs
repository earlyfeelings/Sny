using Sny.Core.AccountsAggregate;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Infrastructure.Services.Repos
{
    public class AccountInmemoryRepo : IAccountProviderRepo, IAccountReadOnlyRepo
    {
        private static string password_1234_hash = "rq6QAAvjqmE2Xqfxl8eNwQ==";

        private List<AccountInternal> _accounts = new List<AccountInternal>()
        {
            new AccountInternal(new Account(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "dev@dev.cz"), password_1234_hash),
            new AccountInternal(new Account(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4"), "test@dev.cz"), password_1234_hash),
        };

        public Guid AddAccount(string email, string hashedPassword)
        {
            Guid guid = Guid.NewGuid();
            _accounts.Add(new AccountInternal(new Account(guid, email), hashedPassword));
            return guid;
        }

        /// <summary>
        /// returns null if account not found
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Account?> FindAcountByEmail(string email)
        {
            return (_accounts.SingleOrDefault(d => d.Account.Email == email))?.Account ?? null;
        }

        /// <summary>
        /// returns null if account not found
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Account?> FindAcount(Guid id)
        {
            return (_accounts.SingleOrDefault(d => d.Account.Id == id))?.Account ?? null;
        }

        public async Task<string> FindAcountPasswordHash(Guid id)
        {
            string? passwordHash = (_accounts.SingleOrDefault(d => d.Account.Id == id))?.PasswordHash;
            return passwordHash ?? throw new AccountNotFoundException();
        }

        private record AccountInternal(Account Account, string PasswordHash);
    }
}
