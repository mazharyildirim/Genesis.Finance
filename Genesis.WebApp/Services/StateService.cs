namespace Genesis.WebApp.Services
{
    public class StateService
    {
        private readonly IApiService api;

        public StateService(IApiService api)
        {
            this.api = api;
            UserResponse = new Genesis.Shared.Users.UserLogin();
        }

        public event Action OnUserChange;

        private void NotifyUserChanged() => OnUserChange?.Invoke();

        
        public Genesis.Shared.Users.UserLogin UserResponse { get; private set; }

        public bool IsSignedIn => UserResponse?.Access_Token != null;

        public void UpdateUser(Genesis.Shared.Users.UserLogin userResponse)
        {

            var oldToken = UserResponse?.Access_Token;
            var newToken = userResponse?.Access_Token;

            if (oldToken != newToken)
            {
                UserResponse = userResponse;

                if (newToken != null)
                {
                    api.SetToken(newToken);
                }
                else
                {
                    api.ClearToken();
                }

                NotifyUserChanged();
            }
        }
    }
}
