namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IAccountProviderRepo
    {
        public Guid AddAccount(string email);
    }
}
