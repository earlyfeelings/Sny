using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;

namespace Sny.Infrastructure.Services.Repos
{
    public class GoalInmemoryRepo : IGoalReadOnlyRepo, IGoalProviderRepo
    {

        private List<Goal> _goals = new List<Goal>()
        {
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "Cíl test1", true, "Tak ale toto je cool popisek.", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2"), "Cíl test2", false, "Tak ale toto je cool popisek!2", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b3"), "Cíl test3", false, "Tak ale toto je cool popisek!3", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4"), "Cíl test4", false, "Tak ale toto je cool popisek!4", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b5"), "Cíl test5", true, "Tak ale toto je cool popisek!5", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b6"), "Cíl test6", false, "Tak ale toto je cool popisek!6", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
        };

        public Task<Goal> AddGoal(string name, bool active, string description, Guid accountId)
        {
            var goal = new Goal(Guid.NewGuid(), name, active, description, accountId);
            _goals.Add(goal);
            return Task.FromResult(goal);
        }

        public Task<Goal> EditGoal(Guid id, string name, bool active, string description, Guid accountId, Func<IQueryable<Goal>, IQueryable<Goal>> filter)
        {
            DeleteGoal(id, filter);
            var goal = new Goal(id, name, active, description, accountId);
            _goals.Add(goal);
            return Task.FromResult(goal);
        }

        public async Task<Goal> GetGoalById(Guid id, Func<IQueryable<Goal>, IQueryable<Goal>> filter)
        {
            var goal = filter(_goals.AsQueryable()).SingleOrDefault(d => d.Id == id);
            return goal ?? throw new GoalNotFoundException();
        }

        public async Task<IReadOnlyCollection<Goal>> GetGoals(Func<IQueryable<Goal>, IQueryable<Goal>> filter)
        {
            return filter(_goals.AsQueryable()).ToArray();
        }

        public void DeleteGoal(Guid id, Func<IQueryable<Goal>, IQueryable<Goal>> filter)
        {
            var goal = GetGoalById(id, filter);
            _goals.Remove(goal.Result);
        }

        public void ChangeActiveGoal(Guid id, bool activate, Func<IQueryable<Goal>, IQueryable<Goal>> filter)
        {
            var goal = GetGoalById(id, filter).Result;
            DeleteGoal(id, filter);
            goal.Active = activate;
            _goals.Add(goal);
        }
    }
}
