using Sny.Core.Goals;

namespace Sny.Core.Interfaces.Infrastructure
{
    public interface ITaskReadOnlyRepo
    {
        public Task<TasksAggregate.Task> GetTaskById(Guid id);

        public Task<IReadOnlyCollection<TasksAggregate.Task>> GetTasksByGoalId(Guid id);
    }
}
