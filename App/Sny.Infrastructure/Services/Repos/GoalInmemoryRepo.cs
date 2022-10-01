using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Infrastructure.Services.Repos
{
    public class GoalInmemoryRepo : IGoalReadOnlyRepo
    {

        private List<Goal> _goals = new List<Goal>()
        {
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b1"), "Cíl test1", true, "Tak ale toto je cool popisek."),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b2"), "Cíl test2", false, "Tak ale toto je cool popisek!2"),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b3"), "Cíl test3", false, "Tak ale toto je cool popisek!3"),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b4"), "Cíl test4", false, "Tak ale toto je cool popisek!4"),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b5"), "Cíl test5", true, "Tak ale toto je cool popisek!5"),
            new Goal(new Guid("8fcfc3fa-590d-462d-b063-dbac4e4b42b6"), "Cíl test6", false, "Tak ale toto je cool popisek!6"),
        };

        public async Task<Goal> GetGoalById(Guid id)
        {
            var goal = _goals.SingleOrDefault(d => d.Id == id);
            return goal ?? throw new GoalNotFoundException();
        }

        public async Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _goals;
        }
    }
}
