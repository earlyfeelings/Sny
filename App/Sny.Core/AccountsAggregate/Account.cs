using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.AccountsAggregate
{
    public class Account
    {
        public Account(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; }
        public string Email { get; }
    }
}
