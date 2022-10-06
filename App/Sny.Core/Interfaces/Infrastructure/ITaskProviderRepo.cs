namespace Sny.Core.Interfaces.Infrastructure
{
    public interface ITaskProviderRepo
    {
        public Task<TasksAggregate.Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId);
        
        public Task<TasksAggregate.Task> EditTask(TasksAggregate.Task task);
        
        public void DeleteTask(Guid id);
    }
}
