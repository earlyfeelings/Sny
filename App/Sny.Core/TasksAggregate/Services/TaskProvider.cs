using Sny.Core.GoalsAggregate.Exceptions;
using Sny.Core.Interfaces.Core;
using Sny.Core.Interfaces.Infrastructure;

namespace Sny.Core.TasksAggregate.Services
{
    public class TaskProvider : ITaskProvider
    {
        private readonly ITaskReadOnlyRepo _tror;
        private readonly ITaskProviderRepo _tpr;
        private readonly ICurrentAccountContext _cac;
        private readonly IGoalReadOnlyRepo _gror;

        public TaskProvider(ITaskReadOnlyRepo tror, ITaskProviderRepo tpr, ICurrentAccountContext cac, IGoalReadOnlyRepo gror)
        {
            _tror = tror;
            _tpr = tpr;
            _cac = cac;
            _gror = gror;
        }

        public async System.Threading.Tasks.Task<TasksAggregate.Task> GetTaskById(Guid id)
        {
            var task = await _tror.GetTaskById(id);
            await CheckPermissions(task.GoalId);
            return task;
        }

        public async System.Threading.Tasks.Task<IReadOnlyCollection<TasksAggregate.Task>> GetTasksByGoalId(Guid goalId)
        {
            await CheckPermissions(goalId);
            return await _tror.GetTasksByGoalId(goalId);
        }

        public async System.Threading.Tasks.Task<Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId)
        { 
             await CheckPermissions(goalId);
             return await _tpr.AddTask(name, description, dueDate, isCompleted, goalId);
        }

        public async System.Threading.Tasks.Task<Task> EditTask(Guid id, string name, string description, DateTime? dueDate, bool isCompleted)
        {
            var task = await GetTaskById(id); //throw exception if unauthorized
            task.Name = name;
            task.Description = description;
            task.DueDate = dueDate;
            task.IsCompleted = isCompleted;
            return await _tpr.EditTask(task);
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid id)
        {
            var task = await GetTaskById(id); //throw exception if unauthorized
            _tpr.DeleteTask(id);
        }
        
        public async System.Threading.Tasks.Task ChangeCompleteTask(Guid id, bool complete)
        {
            var task = await GetTaskById(id); //throw exception if unauthorized
            task.IsCompleted = complete;
            await _tpr.EditTask(task);
        }

        private async System.Threading.Tasks.Task CheckPermissions(Guid id)
        {
            var goal = await _gror.GetGoalById(id);
            if (goal.AccountId != _cac.CurrentAccountId)
                throw new GoalNotFoundException();
        }
    }
}
