using Sny.Core.Goals;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;

namespace Sny.Core.GoalsAggregate.Services
{
    public class GoalProvider : IGoalProvider
    {
        private readonly IGoalReadOnlyRepo _gror;
        private readonly IGoalProviderRepo _gpr;
        private readonly ICurrentAccountContext _cac;

        public GoalProvider(IGoalReadOnlyRepo gror, IGoalProviderRepo gpr, ICurrentAccountContext cac)
        {
            this._gror = gror;
            this._gpr = gpr;
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
        
        public Task<Goal> AddGoal(string name, bool active, string description)
        {
            return _gpr.AddGoal(name, active, description);
        }

        public Task<Goal> EditGoal(Guid id, string name, bool active, string description)
        {
            return _gpr.EditGoal(id, name, active, description);
        }

        public void DeleteGoal(Guid id)
        {
            _gpr.DeleteGoal(id);
        }

        public void ChangeActiveGoal(Guid id, bool activate)
        {
            _gpr.ChangeActiveGoal(id, activate);
        }
    }
}
