namespace Sny.Api.Dtos.Models.Tasks
{
    public class Tasks
    {
        public record TaskDto(Guid Id, string Name, string Description, DateTime? DueDate, bool IsCompleted, Guid GoalId);

        public record AddRequestTaskDto(string Name, string Description, DateTime? DueDate, bool IsCompleted, Guid GoalId);
        
        public record AddResponseTaskDto(Guid Id, string Name, string Description, DateTime? DueDate, bool IsCompleted, Guid GoalId);
        
        public record EditRequestTaskDto(Guid Id, string Name, string Description, DateTime? DueDate, bool IsCompleted, Guid GoalId);
        
        public record EditResponseTaskDto(Guid Id, string Name, string Description, DateTime? DueDate, bool IsCompleted, Guid GoalId);
    }
}
