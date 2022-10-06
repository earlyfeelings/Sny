using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Core
{
    public interface IGoalProvider
    {
        public Task<Goal> GetGoalById(Guid id);

        public Task<IReadOnlyCollection<Goal>> GetGoals();

        public Task<Goal> AddGoal(string name, bool active, string description);

        public Task<Goal> EditGoal(Guid id, string name, bool active, bool isCompleted, string description);

        public Task DeleteGoal(Guid id);

        public Task ChangeActiveGoal(Guid id, bool activate);
        Task ChangeCompleteGoal(Guid id, bool complete);
    }
}
