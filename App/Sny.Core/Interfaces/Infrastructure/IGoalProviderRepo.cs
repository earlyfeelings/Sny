using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IGoalProviderRepo
    {
        public Task<Goal> AddGoal(string name, bool active, string description, Guid accountId);
        public Task<Goal> EditGoal(Guid id, string name, bool active, string description, Guid accountId, Func<IQueryable<Goal>, IQueryable<Goal>> filter);
        public void DeleteGoal(Guid id, Func<IQueryable<Goal>, IQueryable<Goal>> filter);
        public void ChangeActiveGoal(Guid id, bool activate, Func<IQueryable<Goal>, IQueryable<Goal>> filter);
    }
}
