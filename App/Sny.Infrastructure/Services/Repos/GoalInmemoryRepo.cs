using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;

namespace Sny.Infrastructure.Services.Repos
{
    public class GoalInmemoryRepo : IGoalReadOnlyRepo, IGoalProviderRepo
    {

        private List<Goal> _goals = new List<Goal>()
        {
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "Cíl test1", true, false, "Tak ale toto je cool popisek.", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2"), "Cíl test2", false, false, "Tak ale toto je cool popisek!2", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b3"), "Cíl test3", false, false, "Tak ale toto je cool popisek!3", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4"), "Cíl test4", false, false, "Tak ale toto je cool popisek!4", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b5"), "Cíl test5", true, false, "Tak ale toto je cool popisek!5", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b6"), "Cíl test6", false, false, "Tak ale toto je cool popisek!6", new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4")),
        };

        public async Task<Goal> AddGoal(string name, bool active, string description, Guid accountId)
        {
            var goal = new Goal(Guid.NewGuid(), name, active, false, description, accountId);
            _goals.Add(goal);
            return goal;
        }

        public async Task<Goal> EditGoal(Goal model)
        {
            DeleteGoal(model.Id);
            _goals.Add(model);
            return model;
        }

        public async Task<Goal> GetGoalById(Guid id)
        {
            var goal = _goals.AsQueryable().SingleOrDefault(d => d.Id == id);
            return goal ?? throw new GoalNotFoundException();
        }

        public async Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _goals.AsReadOnly();
        }
        
        public async void DeleteGoal(Guid id)
        {
            var goal =  await GetGoalById(id);
            _goals.Remove(goal);
        }
    }
}
