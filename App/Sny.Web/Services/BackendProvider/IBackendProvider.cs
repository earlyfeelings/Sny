using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Api.Dtos.Models.Tasks;

namespace Sny.Web.Services.BackendProvider
{
    public interface IBackendProvider
    {
        Task<ApiResponse<AddResponseGoalDto>> AddGoal(AddRequestGoalDto model);
        Task<ApiResponse<Tasks.AddResponseTaskDto>> AddTask(Tasks.AddRequestTaskDto model);
        Task<ApiResponse> DeleteTask(Guid id);
        Task<ApiResponse<GoalDto>> GetGoal(Guid id);
        Task<ApiResponse<GoalDto[]>> GetGoals();
        Task<ApiResponse<MyInfoResponseDto>> GetMyInfo();
        Task<ApiResponse<Tasks.TaskDto[]>> GetTasksByGoalId(Guid id);
        Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto model);
        Task<ApiResponse> Logout();
        Task<ApiResponse> SetGoalActive(Guid id, bool active);
        Task<ApiResponse> SetTaskComplete(Guid id, bool complete);
        Task<ApiResponse<EditResponseGoalDto>> UpdateGoal(EditRequestGoalDto model);
        Task<ApiResponse<Tasks.EditResponseTaskDto>> UpdateTask(Tasks.EditRequestTaskDto model);
    }
}
