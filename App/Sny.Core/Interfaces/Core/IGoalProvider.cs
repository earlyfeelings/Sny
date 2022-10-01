using Sny.Core.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.Interfaces.Core
{
    public interface IGoalProvider
    {
        public Task<Goal> GetGoalById(Guid id);

        public Task<IReadOnlyCollection<Goal>> GetGoals();
    }
}
