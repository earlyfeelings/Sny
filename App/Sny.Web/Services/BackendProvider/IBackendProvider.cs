using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Api.Dtos.Models.Tasks;
using Sny.Web.Model;

namespace Sny.Web.Services.BackendProvider
{
    public interface IBackendProvider
    {
        BackendApiCredentials? CurrentCredentials { get; }

        Task<ApiResponse<AddResponseGoalDto>> AddGoal(AddRequestGoalDto model);
        Task<ApiResponse<Tasks.AddResponseTaskDto>> AddTask(Tasks.AddRequestTaskDto model);
        Task ClearCredentials();
        Task<ApiResponse> DeleteTask(Guid id);
        Task<ApiResponse<GoalDto>> GetGoal(Guid id);
        Task<ApiResponse<GoalDto[]>> GetGoals();
        Task<ApiResponse<MyInfoResponseDto>> GetMyInfo();
        Task<ApiResponse<Tasks.TaskDto[]>> GetTasksByGoalId(Guid id);
        Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto model);
        Task<ApiResponse> Logout(LogoutRequestDto model);
        Task SetCredentials(BackendApiCredentials credentials);
        Task<ApiResponse> SetGoalActive(Guid id, bool active);
        Task<ApiResponse> SetTaskComplete(Guid id, bool complete);
        Task<ApiResponse<EditResponseGoalDto>> UpdateGoal(EditRequestGoalDto model);
        Task<ApiResponse<Tasks.EditResponseTaskDto>> UpdateTask(Tasks.EditRequestTaskDto model);
    }
}
