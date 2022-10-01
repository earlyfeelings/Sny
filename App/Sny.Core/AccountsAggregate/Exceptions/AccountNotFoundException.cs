using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.AccountsAggregate.Exceptions
{
    public class AccountNotFoundException : ApplicationException
    {
        public AccountNotFoundException() : base("Account not found")
        {
        }
    }
}
