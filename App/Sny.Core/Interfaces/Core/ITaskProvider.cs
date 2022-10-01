﻿namespace Sny.Core.Interfaces.Core
{
    public interface ITaskProvider
    {
        public Task<TasksAggregate.Task> GetTaskById(Guid id);
        
        public Task<IReadOnlyCollection<TasksAggregate.Task>> GetTasksByGoalId(Guid id);

        public Task<TasksAggregate.Task> AddTask(string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId);

        public Task<TasksAggregate.Task> EditTask(TasksAggregate.Task task);

        public void DeleteTask(Guid id);

        public void ChangeCompleteTask(Guid id, bool complete);

    }
}
