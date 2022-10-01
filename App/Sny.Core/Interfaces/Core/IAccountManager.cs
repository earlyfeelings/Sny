﻿using Sny.Core.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Interfaces.Core
{
    public interface IAccountManager
    {
        public Task<LoginResult> Login(LoginModel model);

        public Task<RegisterResult> Register(RegisterModel model);
    }

    public record LoginModel(string Email, string Password);

    public record RegisterModel(string Email, string Password, string PasswordAgain);

    public record LoginResult(string Jwt);

    public record RegisterResult(RegisterStatus RegisterStatus);

    public enum RegisterStatus
    {
        Success,
        AlreadyExists,
        WeakPassword,
        PasswordsNotSame,
        BadEmailFormat,
        Error
    }
}
