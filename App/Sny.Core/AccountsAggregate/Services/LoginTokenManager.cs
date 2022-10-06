using Sny.Core.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.AccountsAggregate.Services
{
    public class LoginTokenManager : ILoginTokenManager
    {
        //TODO: For now inmemory; make persistent - e.c. store in REDIS database
        private Dictionary<Guid, HashSet<string>> _validRefreshTokens { get; set; } = new Dictionary<Guid, HashSet<string>>();

        public async Task InvalidateAllTokens(Account acc)
        {
            if (!_validRefreshTokens.TryGetValue(acc.Id, out var validTokens))
                return;
            validTokens.Clear();
        }

        public async Task InvalidateToken(Account acc, string token)
        {
            if (!_validRefreshTokens.TryGetValue(acc.Id, out var validTokens))
                return;
            validTokens.Remove(token);
        }

        public async Task SetToken(Account acc, string token)
        {
            _validRefreshTokens.TryAdd(acc.Id, new HashSet<string>());
            _validRefreshTokens[acc.Id].Add(token);
        }

        public async Task<bool> TokenIsValid(Account acc, string token)
        {
            if (!_validRefreshTokens.TryGetValue(acc.Id, out var validTokens))
                return false;
            return validTokens.Contains(token);
        }
    }
}
