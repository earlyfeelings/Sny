using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IGoalProviderRepo
    {
        public Task<Goal> AddGoal(string name, bool active, string description, Guid accountId);
        
        public Task<Goal> EditGoal(Goal model);
        
        public void DeleteGoal(Guid id);
    }
}
