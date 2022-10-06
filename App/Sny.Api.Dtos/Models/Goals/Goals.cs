namespace Sny.Api.Dtos.Models.Goals
{
    public record GoalDto(Guid Id, string Name, bool Active, bool IsCompleted, string Description);

    public record AddRequestGoalDto(string Name, bool Active, string Description);

    public record AddResponseGoalDto(Guid Id, string Name, bool Active, string Description);
    
    public record EditRequestGoalDto(Guid Id, string Name, bool Active, bool IsCompleted, string Description);

    public record EditResponseGoalDto(Guid Id, string Name, bool Active, bool IsCompleted, string Description);
}
