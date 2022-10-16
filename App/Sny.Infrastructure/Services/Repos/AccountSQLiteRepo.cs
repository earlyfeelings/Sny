using Microsoft.EntityFrameworkCore;
using Sny.Core.AccountsAggregate;
using Sny.Core.AccountsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;
using Sny.DB.Data;

namespace Sny.Infrastructure.Services.Repos
{
    public class AccountSQLiteRepo : IAccountProviderRepo, IAccountReadOnlyRepo
    {
        private readonly SnySQLiteContext _context;
        public AccountSQLiteRepo(SnySQLiteContext context)
        {
            _context = context;
        }
        public Guid AddAccount(string email, string hashedPassword)
        {
            var account = new DB.Entities.Account
            {
                Email = email,
                Password = hashedPassword
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account.Id!.Value;
        }

        public async Task<Account?> FindAcount(Guid id)
        {
            return await _context.Accounts
                .Where(d => d.Id == id)
                .Select(d => new Account(d.Id!.Value, d.Email))
                .SingleOrDefaultAsync();
        }
        
        public async Task<Account?> FindAcountByEmail(string email)
        {
            return await _context.Accounts
                .Where(d => d.Email == email)
                .Select(d => new Account(d.Id!.Value, d.Email))
                .SingleOrDefaultAsync();
        }

        public async Task<string> FindAcountPasswordHash(Guid id)
        {
            return await _context.Accounts
                .Where(d => d.Id == id)
                .Select(d => d.Password)
                .SingleOrDefaultAsync() ?? throw new AccountNotFoundException();
        }
    }
}
