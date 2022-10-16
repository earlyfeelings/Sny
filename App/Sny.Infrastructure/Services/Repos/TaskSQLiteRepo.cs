using Microsoft.EntityFrameworkCore;
using Sny.Core.Interfaces.Infrastructure;
using Sny.Core.TasksAggregate.Exceptions;
using Sny.DB.Data;

namespace Sny.Infrastructure.Services.Repos
{
    public class TaskSQLiteRepo : ITaskProviderRepo, ITaskReadOnlyRepo
    {
        private readonly SnySQLiteContext _context;
        public TaskSQLiteRepo(SnySQLiteContext context)
        {
            _context = context;
        }
        public async Task<Core.TasksAggregate.Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId)
        {            
            var task = new DB.Entities.Task
            {
                Name = name,
                Description = description,
                DueDate = dueDate,
                IsCompleted = isCompleted,
                GoalId = goalId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return new Core.TasksAggregate.Task(task.Id!.Value, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
        }

        public async void DeleteTask(Guid id)
        {
            var task = GetTaskByIdFromDB(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<Core.TasksAggregate.Task> EditTask(Core.TasksAggregate.Task updatedTask)
        {
            var task = GetTaskByIdFromDB(updatedTask.Id);
            task.Id = updatedTask.Id;
            task.Name = updatedTask.Name;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.IsCompleted = updatedTask.IsCompleted;
            task.GoalId = updatedTask.GoalId;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return new Core.TasksAggregate.Task(task.Id!.Value, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
        }

        public async Task<Core.TasksAggregate.Task> GetTaskById(Guid id)
        {
            var task = GetTaskByIdFromDB(id);
            return new Core.TasksAggregate.Task(task.Id!.Value, task.Name, task.Description, task.DueDate, task.IsCompleted, task.GoalId);
        }

        public async Task<IReadOnlyCollection<Core.TasksAggregate.Task>> GetTasksByGoalId(Guid id)
        {
            return await _context.Tasks.Where(d => d.GoalId == id)
               .Select(d => new Core.TasksAggregate.Task(d.Id!.Value, d.Name, d.Description, d.DueDate, d.IsCompleted, d.GoalId))
               .ToListAsync();
        }
        
        private DB.Entities.Task GetTaskByIdFromDB(Guid id)
        {
            var goal = _context.Tasks.SingleOrDefault(d => d.Id == id);
            return goal ?? throw new TaskNotFoundException();
        }
    }
}
