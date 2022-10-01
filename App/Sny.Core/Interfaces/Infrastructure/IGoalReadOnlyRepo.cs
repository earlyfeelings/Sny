using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IGoalReadOnlyRepo
    {
        public Task<Goal> GetGoalById(Guid id, Func<IQueryable<Goal>, IQueryable<Goal>> filter);

        public Task<IReadOnlyCollection<Goal>> GetGoals(Func<IQueryable<Goal>, IQueryable<Goal>> filter);

    }
}
