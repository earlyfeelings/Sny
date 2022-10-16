using Sny.Core.Goals;
using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Infrastructure;
using Sny.DB.Data;

namespace Sny.Infrastructure.Services.Repos
{
    public class GoalSQLiteRepo : IGoalReadOnlyRepo, IGoalProviderRepo
    {
        private readonly SnySQLiteContext _context;
        public GoalSQLiteRepo(SnySQLiteContext context)
        {
            _context = context;
        }
        public async Task<Goal> AddGoal(string name, bool active, string description, Guid accountId)
        {
            var goal = new DB.Entities.Goal
            {
                Name = name,
                Active = active,
                IsCompleted = false,
                Description = description,
                AccountId = accountId
            };
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return new Goal(goal.Id!.Value, goal.Name, goal.Active, goal.IsCompleted, goal.Description, goal.AccountId);
        }

        public async void DeleteGoal(Guid id)
        {
            var goal = GetGoalByIdFromDB(id);
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }

        public async Task<Goal> EditGoal(Goal model)
        {
            var goal = GetGoalByIdFromDB(model.Id);
            goal.Id = model.Id;
            goal.Name = model.Name;
            goal.Active = model.Active;
            goal.IsCompleted = model.IsCompleted;
            goal.Description = model.Description;
            goal.AccountId = model.AccountId;
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
            return new Goal(goal.Id!.Value, goal.Name, goal.Active, goal.IsCompleted, goal.Description, goal.AccountId);
        }

        public async Task<Goal> GetGoalById(Guid id)
        {
            var goal = GetGoalByIdFromDB(id);
            return new Goal(goal.Id!.Value, goal.Name, goal.Active, goal.IsCompleted, goal.Description, goal.AccountId);
        }

        public async Task<IReadOnlyCollection<Goal>> GetGoals()
        {
            return _context.Goals.Select(d => new Goal(d.Id!.Value, d.Name, d.Active, d.IsCompleted, d.Description, d.AccountId)).ToList();
        }

        private DB.Entities.Goal GetGoalByIdFromDB(Guid id)
        {
            var goal = _context.Goals.SingleOrDefault(d => d.Id == id);
            return goal ?? throw new GoalNotFoundException();
        }
    }
}
