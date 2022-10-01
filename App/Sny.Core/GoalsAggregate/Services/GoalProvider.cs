using Sny.Core.Goals;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Core.GoalsAggregate.Services
{
    public class GoalProvider : IGoalProvider
    {
        private readonly IGoalReadOnlyRepo _gror;
        private readonly ICurrentAccountContext _cac;

        public GoalProvider(IGoalReadOnlyRepo gror, ICurrentAccountContext cac)
        {
            this._gror = gror;
            this._cac = cac;
        }

        public Task<Goal> GetGoalById(Guid id)
        {
            return _gror.GetGoalById(id);
        }

        public Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _gror.GetGoals();
        }
    }
}
