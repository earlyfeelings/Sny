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

        public Task<TasksAggregate.Task> GetTaskById(Guid id)
        {
            var task = _tror.GetTaskById(id);
            CheckPermissions(task.Result.GoalId);
            return task;
        }

        public Task<IReadOnlyCollection<TasksAggregate.Task>> GetTasksByGoalId(Guid goalId)
        {
            CheckPermissions(goalId);
            return _tror.GetTasksByGoalId(goalId);
        }

        public Task<Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId)
        {
            return _tpr.AddTask(name, description, dueDate, isCompleted, goalId);
        }

        public Task<Task> EditTask(Task task)
        {
            CheckPermissions(task.GoalId);
            var taskToReplace = _tror.GetTaskById(task.Id);
            if(task.GoalId != taskToReplace.Result.GoalId) CheckPermissions(taskToReplace.Result.GoalId);
            return _tpr.EditTask(task);
        }

        public void DeleteTask(Guid id)
        {
            var task = _tror.GetTaskById(id);
            CheckPermissions(task.Result.GoalId);
            _tpr.DeleteTask(id);
        }
        
        public void ChangeCompleteTask(Guid id, bool complete)
        {
            var task = _tror.GetTaskById(id);
            CheckPermissions(task.Result.GoalId);
            _tpr.ChangeCompleteTask(id, complete);
        }

        private void CheckPermissions(Guid id)
        {
            var goal = _gror.GetGoalById(id, d => d.Where(d => d.AccountId == _cac.CurrentAccountId)).Result;
        }
    }
}
