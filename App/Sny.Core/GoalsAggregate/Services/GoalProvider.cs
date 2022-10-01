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

            return _gror.GetGoalById(id, d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }

        public Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _gror.GetGoals(d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }
        
        public Task<Goal> AddGoal(string name, bool active, string description)
        {
            return _gpr.AddGoal(name, active, description, _cac.CurrentAccountId.GetValueOrDefault());
        }

        public Task<Goal> EditGoal(Guid id, string name, bool active, string description)
        {
            return _gpr.EditGoal(id, name, active, description, _cac.CurrentAccountId.GetValueOrDefault(), d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }

        public void DeleteGoal(Guid id)
        {
            _gpr.DeleteGoal(id, d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }

        public void ChangeActiveGoal(Guid id, bool activate)
        {
            _gpr.ChangeActiveGoal(id, activate, d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }
    }
}
