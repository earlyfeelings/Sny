namespace Sny.Core.TasksAggregate.Exceptions
{
    public class TaskNotFoundException : ApplicationException
    {
        public TaskNotFoundException() : base("Task not found.")
        {
        }
    }
}
