using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.AccountsAggregate.Exceptions
{
    public class LoginFailedException : ApplicationException
    {
        public LoginFailedException() : base("Login failed. Invalid credentials or user name.")
        {
        }
    }
}
