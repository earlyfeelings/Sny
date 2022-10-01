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

        private List<Account> _accounts = new List<Account>()
        {
            new Account(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "dev@dev.cz"),
            new Account(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "test@test.cz"),
        };

        public Guid AddAccount(string email)
        {
            Guid guid = Guid.NewGuid();
            _accounts.Add(new Account(guid, email));
            return guid;
        }

        /// <summary>
        /// returns null if account not found
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Account?> FindAcountByEmail(string email)
        {
            return (_accounts.SingleOrDefault(d => d.Email == email)) ?? null;
        }
    }
}
