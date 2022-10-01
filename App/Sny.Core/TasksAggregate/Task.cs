namespace Sny.Core.TasksAggregate
{
    public class Task
    {
        public Task(Guid id, string name, string description, DateTime? dueDate, bool isCompleted, Guid goalId)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            IsCompleted = isCompleted;
            GoalId = goalId;
        }

        public Guid Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public Guid GoalId { get; }
    }
}
