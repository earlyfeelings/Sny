using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IGoalProviderRepo
    {
        public Task<Goal> AddGoal(string name, bool active, string description);
        public Task<Goal> EditGoal(Guid id, string name, bool active, string description);
        public Task<bool> DeleteGoal(Guid id);
    }
}
