using Sny.Core.AccountsAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IAccountReadOnlyRepo
    {
        public Task<Account?> FindAcountByEmail(string email);

        public Task<string> FindAcountPasswordHash(Guid id);
    }
}
