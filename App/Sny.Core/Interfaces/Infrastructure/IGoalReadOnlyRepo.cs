using Sny.Core.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface IGoalReadOnlyRepo
    {
        public Task<Goal> GetGoalById(Guid id);

        public Task<IReadOnlyCollection<Goal>> GetGoals();

    }
}
