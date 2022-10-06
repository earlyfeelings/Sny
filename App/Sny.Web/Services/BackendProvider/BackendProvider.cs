using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Web.Exceptions;
using Sny.Web.Model;
using Sny.Web.Pages;
using Sny.Web.Services.LocalStorageService;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Sny.Api.Dtos.Models.Tasks.Tasks;

namespace Sny.Web.Services.BackendProvider
{
    public class BackendProvider : IBackendProvider
    {
        private Uri _baseUri;
        private readonly HttpClient _client;
        private BackendApiCredentials? _credentials;
        private ILocalStorageService _localStorageService;


        public async Task SetCredentials(BackendApiCredentials credentials)
        {
            _credentials = credentials;
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {credentials.Jwt}");
            await _localStorageService.SetItem(LocalStorageKeys.Jwt, credentials.Jwt);
            await _localStorageService.SetItem(LocalStorageKeys.RefreshToken, credentials.RefreshToken);
        }

        public async Task ClearCredentials()
        {
            _credentials = null;
            _client.DefaultRequestHeaders.Remove("Authorization");
            await _localStorageService.RemoveItem(LocalStorageKeys.Jwt);
            await _localStorageService.RemoveItem(LocalStorageKeys.RefreshToken);
        }

        public BackendApiCredentials? CurrentCredentials => _credentials;

        public BackendProvider(Uri baseUri, HttpClient client, ILocalStorageService localStorageService)
        {
            _baseUri = baseUri;
            _client = client;
            _localStorageService = localStorageService;
        }

        public Uri GetUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        public async Task<ApiResponse> Logout(LogoutRequestDto model)
        {
            var raw = await _client.PostAsJsonAsync(GetUri("account/logout"), model);
            return await ValidateResponse(raw);
        }

        public async Task<ApiResponse<MyInfoResponseDto>> GetMyInfo()
        {
            var raw = await _client.GetAsync(GetUri("account/myinfo"));
            return await ValidateResponse<MyInfoResponseDto>(raw);
        }

        public async Task<ApiResponse> DeleteTask(Guid id)
        {
            var raw = await _client.DeleteAsync(GetUri($"tasks/{id}"));
            return await ValidateResponse(raw);
        }

        public async Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto model)
        {
            var raw = await _client.PostAsJsonAsync(GetUri("account/login"), model);
            return await ValidateResponse<LoginResponseDto>(raw);
        }

        public async Task<ApiResponse<AddResponseTaskDto>> AddTask(AddRequestTaskDto model)
        {
            var raw = await _client.PostAsJsonAsync(GetUri("tasks"), model);
            return await ValidateResponse<AddResponseTaskDto>(raw);
        }

        public async Task<ApiResponse<EditResponseGoalDto>> UpdateGoal(EditRequestGoalDto model)
        {
            var raw = await _client.PutAsJsonAsync(GetUri($"goals/{model.Id}"), model);
            return await ValidateResponse<EditResponseGoalDto>(raw);
        }

        public async Task<ApiResponse<EditResponseTaskDto>> UpdateTask(EditRequestTaskDto model)
        {
            var raw = await _client.PutAsJsonAsync(GetUri($"tasks/{model.Id}"), model);
            return await ValidateResponse<EditResponseTaskDto>(raw);
        }

        public async Task<ApiResponse> SetTaskComplete(Guid id, bool complete)
        {
            var raw = await _client.PostAsync(GetUri($"tasks/{id}/complete/{complete}"), null);
            return await ValidateResponse(raw);
        }

        public async Task<ApiResponse> SetGoalActive(Guid id, bool active)
        {
            var raw = await _client.PostAsync(GetUri($"goals/{id}/activate/{active}"), null);
            return await ValidateResponse(raw);
        }

        public async Task<ApiResponse<GoalDto[]>> GetGoals()
        {
            var raw = await _client.GetAsync(GetUri("goals"));
            return await ValidateResponse<GoalDto[]>(raw);
        }

        public async Task<ApiResponse<GoalDto>> GetGoal(Guid id)
        {
            var raw = await _client.GetAsync(GetUri($"goals/{id}"));
            return await ValidateResponse<GoalDto>(raw);
        }

        public async Task<ApiResponse<TaskDto[]>> GetTasksByGoalId(Guid id)
        {
            var raw = await _client.GetAsync(GetUri($"tasks/byGoal/{id}"));
            return await ValidateResponse<TaskDto[]>(raw);
        }

        public async Task<ApiResponse<AddResponseGoalDto>> AddGoal(AddRequestGoalDto model)
        {
            var raw = await _client.PostAsJsonAsync(GetUri("goals"), model);
            return await ValidateResponse<AddResponseGoalDto>(raw);
        }

        private async Task<ApiResponse> ValidateResponse([NotNull] HttpResponseMessage? response) 
        {
            if (response == null)
                throw new ApiClientException("Response is null");
            return new ApiResponse(response);
        }

        private async Task<ApiResponse<T>> ValidateResponse<T>([NotNull] HttpResponseMessage? response) where T : class
        {
            if (response == null)
                throw new ApiClientException("Response is null");
            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>(response, null);
            }
            var r = await response.Content.ReadFromJsonAsync<T>();
            return new ApiResponse<T>(response, r);
        }
    }

    public record ApiResponse<T>(HttpResponseMessage Response, T? RawResponseData)
    {
        /// <summary>
        /// Returns non-nullable response data.
        /// Throws exception, if response data does not exists.
        /// </summary>
        public T Data 
        { 
            get {
                return RawResponseData ?? throw new ApiClientException("Response data is null.");
            }
        }

        public ApiResponse<T> ThrowWhenUnsuccessful()
        {
            if (!Response.IsSuccessStatusCode) throw new ApiClientException("Response is not success.");
            return this;
        }
    }
    public record ApiResponse(HttpResponseMessage Response)
    {
        public ApiResponse ThrowWhenUnsuccessful()
        {
            if (!Response.IsSuccessStatusCode) throw new ApiClientException("Response is not success.");
            return this;
        }
    }
}
