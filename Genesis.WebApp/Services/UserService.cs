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
               
            }
          
        }

        private async void SetAuth(Genesis.Shared.Users.UserLogin user)
        {
            if (user != null)
            {
                var userData = user;

                state.UpdateUser(user);
                await jwtService.SaveTokenAsync(userData.Access_Token, userData.Username);
            }
            else
            {
                await PurgeAuth();
            }

        }

        private async Task PurgeAuth()
        {
            Genesis.Shared.Users.UserLogin newUser = new Genesis.Shared.Users.UserLogin();
            await jwtService.DestroyTokenAsync();

            if (state?.UserResponse != newUser)
                state.UpdateUser(newUser);
        }

        public async Task SignOutAsync()
        {
            await PurgeAuth();
        }

        public async Task<ApiResponse<Genesis.Shared.Users.UserLogin>> AttemptAuth(Genesis.Shared.Users.Login credentials)
        {
            var response = await api.PostAsync<Genesis.Shared.Users.UserLogin>($"/Auth/login", credentials);
           
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
      
        public UserModel User { get; set; }
    }

    public static class AuthenticationType
    {
        public const string Login = "/login";
        public const string Register = "";
    }
}
