using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;
using System.Reflection;

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

        public async Task<Goal> GetGoalById(Guid id)
        {
            var goal = await _gror.GetGoalById(id);
            if (goal.AccountId != _cac.CurrentAccountId) 
                throw new GoalNotFoundException();
            return goal;
        }

        public Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _gror.GetGoals(d => d.Where(d => d.AccountId == _cac.CurrentAccountId));
        }
        
        public Task<Goal> AddGoal(string name, bool active, string description)
        {
            return _gpr.AddGoal(name, active, description, _cac.CurrentAccountId.GetValueOrDefault());
        }

        public async Task<Goal> EditGoal(Guid id, string name, bool active, bool isCompleted, string description)
        {
            var goal = await GetGoalById(id); //throw exception if unauthorized
            goal.Name = name;
            goal.Active = active;
            goal.IsCompleted = isCompleted;
            goal.Description = description;
            return await _gpr.EditGoal(goal);
        }

        public async Task DeleteGoal(Guid id)
        {
            var _ = await GetGoalById(id); //throw exception if unauthorized
            _gpr.DeleteGoal(id);
        }

        public async Task ChangeActiveGoal(Guid id, bool activate)
        {
            var goal = await GetGoalById(id); //throw exception if unauthorized
            goal.Active = activate;
            await _gpr.EditGoal(goal);
        }

        public async Task ChangeCompleteGoal(Guid id, bool complete)
        {
            var goal = await GetGoalById(id); //throw exception if unauthorized
            goal.IsCompleted = complete;
            await _gpr.EditGoal(goal);
        }
    }
}
