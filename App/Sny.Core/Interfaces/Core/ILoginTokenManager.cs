using Sny.Core.AccountsAggregate;

namespace Sny.Core.Interfaces.Core
{
    public interface ILoginTokenManager
    {
        public Task SetToken(Account acc, string token);

        public Task InvalidateToken(Account acc, string token);
        
        public Task InvalidateAllTokens(Account acc);

        public Task<bool> TokenIsValid(Account acc, string token);
    }
}
