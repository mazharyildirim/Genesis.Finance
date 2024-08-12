using System.Threading.Tasks;
using Genesis.WebApp.Models;

namespace Genesis.WebApp.Services
{
    public class UserService
    {
        private readonly IJwtService jwtService;
        private readonly IApiService api;
        private readonly StateService state;

        public UserService(IJwtService _jwtService, IApiService _api, StateService _state)
        {
            jwtService = _jwtService;
            api = _api;
            state = _state;
        }

        public async Task PopulateAsync()
        {
            string token = await jwtService.GetTokenAsync();
            string username = await jwtService.GetUserNameAsync();
            if (!string.IsNullOrEmpty(token))
            {
                api.SetToken(token);
                var response = await api.GetAsync<Genesis.Shared.Users.Tokens>($"/User/GetUsername/{username}");
                state.UpdateUser(response.Value);
            }
            else
            {
                await PurgeAuth();
            }
        }

        private async void SetAuth(Genesis.Shared.Users.Tokens user)
        {
            if (user != null)
            {
                state.UpdateUser(user);
                await jwtService.SaveTokenAsync(user.Access_Token,user.Username);
            }
            else
            {
                await PurgeAuth();
            }

        }

        private async Task PurgeAuth()
        {
            Genesis.Shared.Users.Tokens newUser = new Genesis.Shared.Users.Tokens();
            await jwtService.DestroyTokenAsync();

            if (state?.User != newUser)
                state.UpdateUser(newUser);
        }

        public async Task SignOutAsync()
        {
            await PurgeAuth();
        }

        public async Task<ApiResponse<Genesis.Shared.Users.Tokens>> AttemptAuth(Genesis.Shared.Users.Login credentials)
        {
            var response = await api.PostAsync<Genesis.Shared.Users.Tokens>($"/Auth/login", credentials);

            SetAuth(response?.Value);
            return response;
        }

        //public async Task<ApiResponse<UserResponse>> UpdateAsync(UserModel user)
        //{
        //    var response = await api.PutAsync<UserResponse>("/user", new
        //    {
        //        user = user
        //    });

        //    if (response?.Value != null)
        //        state.UpdateUser(response.Value.User);

        //    return response;
        //}
    }

    public class UserResponse
    {
        public ErrorsModel Errors { get; set; }
        public UserModel User { get; set; }
    }

    public static class AuthenticationType
    {
        public const string Login = "/login";
        public const string Register = "";
    }
}
