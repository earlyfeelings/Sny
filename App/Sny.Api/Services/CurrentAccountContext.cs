using Sny.Core.Interfaces.Infrastructure;

namespace Sny.Api.Services
{
    public class CurrentAccountContext : ICurrentAccountContext
    {
        public Guid? CurrentAccountId { get; set; }

        public Guid GetCurrentAccountId()
        {
            if (CurrentAccountId == null)
            {
                throw new ApplicationException();
            }
            return CurrentAccountId.Value;
        }
    }
}
