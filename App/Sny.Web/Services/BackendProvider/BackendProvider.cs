using Microsoft.AspNetCore.Components;
using Sny.Api.Dtos.Models.Accounts;
using Sny.Api.Dtos.Models.Goals;
using Sny.Web.Exceptions;
using Sny.Web.Model;
using Sny.Web.Pages;
using Sny.Web.Services.LocalStorageService;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Reflection;
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
        private readonly NavigationManager _navManager;
        private SemaphoreSlim _lockRefreshToken = new SemaphoreSlim(1, 1);


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

        public BackendProvider(Uri baseUri, HttpClient client, ILocalStorageService localStorageService, NavigationManager navManager)
        {
            _baseUri = baseUri;
            _client = client;
            _localStorageService = localStorageService;
            this._navManager = navManager;
        }

        public Uri GetUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        private async Task RotateCredentialsWhenRequired()
        {
            var creds = _credentials;
            if (creds == null) return;
            if (creds.ExpireAtUtc > DateTime.UtcNow.AddSeconds(10)) return;

            //Token expired
            await _lockRefreshToken.WaitAsync();
            try
            {
                creds = _credentials;
                if (creds == null) return;
                if (creds.ExpireAtUtc > DateTime.UtcNow.AddSeconds(10)) return;

                //rotate credentials
                var res = await RefreshToken(creds.Jwt, creds.RefreshToken);
                if (!res.Response.IsSuccessStatusCode)
                {
                    //credentials rotation not successed
                    await ClearCredentials();
                    _navManager.NavigateTo("/login?logout=true");
                    return;
                }

                await SetCredentials(new BackendApiCredentials(res.Data.Jwt, res.Data.RefreshToken, res.Data.ExpiryAtUtc));
            }
            finally
            {
                _lockRefreshToken.Release();
            }
        }

        private async Task<ApiResponse<T>> MakeStandardRequest<T>(Func<Task<HttpResponseMessage>> handler) where T : class
        {
            await RotateCredentialsWhenRequired();
            return await ValidateResponse<T>(await handler());
        }

        private async Task<ApiResponse> MakeStandardRequest(Func<Task<HttpResponseMessage>> handler)
        {
            await RotateCredentialsWhenRequired();
            return await ValidateResponse(await handler());
        }

        /// <summary>
        /// Method is not calling MakeStandardRequest, because we do not want check credentials validity.
        /// (it can cause infinity check loop)
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<ApiResponse<LoginResponseDto>> RefreshToken(string jwt, string refreshToken)
        {
            var raw = await _client.GetAsync(GetUri($"account/refresh-token?jwt={jwt}&refreshToken={refreshToken}"));
            return await ValidateResponse<LoginResponseDto>(raw);
        }

        public async Task<ApiResponse> Logout(LogoutRequestDto model)
        {
            return await MakeStandardRequest(() => _client.PostAsJsonAsync(GetUri("account/logout"), model));
        }

        public async Task<ApiResponse<MyInfoResponseDto>> GetMyInfo()
        {
            return await MakeStandardRequest<MyInfoResponseDto>(() => _client.GetAsync(GetUri("account/myinfo")));
        }

        public async Task<ApiResponse> DeleteTask(Guid id)
        {
            return await MakeStandardRequest(() => _client.DeleteAsync(GetUri($"tasks/{id}")));
        }

        public async Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto model)
        {
            return await MakeStandardRequest<LoginResponseDto>(() => _client.PostAsJsonAsync(GetUri("account/login"), model));
        }

        public async Task<ApiResponse<AddResponseTaskDto>> AddTask(AddRequestTaskDto model)
        {
            return await MakeStandardRequest<AddResponseTaskDto>(() => _client.PostAsJsonAsync(GetUri("tasks"), model));
        }

        public async Task<ApiResponse<EditResponseGoalDto>> UpdateGoal(EditRequestGoalDto model)
        {
            return await MakeStandardRequest<EditResponseGoalDto>(() => _client.PutAsJsonAsync(GetUri($"goals/{model.Id}"), model));
        }

        public async Task<ApiResponse<EditResponseTaskDto>> UpdateTask(EditRequestTaskDto model)
        {
            return await MakeStandardRequest<EditResponseTaskDto>(() => _client.PutAsJsonAsync(GetUri($"tasks/{model.Id}"), model));
        }

        public async Task<ApiResponse> SetTaskComplete(Guid id, bool complete)
        {
            return await MakeStandardRequest(() => _client.PostAsync(GetUri($"tasks/{id}/complete/{complete}"), null));
        }

        public async Task<ApiResponse> SetGoalActive(Guid id, bool active)
        {
            return await MakeStandardRequest(() => _client.PostAsync(GetUri($"goals/{id}/activate/{active}"), null));
        }

        public async Task<ApiResponse<GoalDto[]>> GetGoals()
        {
            return await MakeStandardRequest<GoalDto[]>(() => _client.GetAsync(GetUri("goals")));
        }

        public async Task<ApiResponse<GoalDto>> GetGoal(Guid id)
        {
            return await MakeStandardRequest<GoalDto>(() => _client.GetAsync(GetUri($"goals/{id}")));
        }

        public async Task<ApiResponse<TaskDto[]>> GetTasksByGoalId(Guid id)
        {
            return await MakeStandardRequest<TaskDto[]>(() => _client.GetAsync(GetUri($"tasks/byGoal/{id}")));
        }

        public async Task<ApiResponse<AddResponseGoalDto>> AddGoal(AddRequestGoalDto model)
        {
            return await MakeStandardRequest<AddResponseGoalDto>(() => _client.PostAsJsonAsync(GetUri("goals"), model));
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
