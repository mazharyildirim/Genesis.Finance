using System;

using Genesis.Finance.Models;

namespace Genesis.Finance.Services
{
    public class StateService
    {
        private readonly IApiService api;

        public StateService(IApiService api)
        {
            this.api = api;
            User = new Genesis.Shared.Users.Tokens();
        }

        public event Action OnUserChange;

        private void NotifyUserChanged() => OnUserChange?.Invoke();

        public ErrorsModel Errors { get; private set; }
        public Genesis.Shared.Users.Tokens User { get; private set; }

        public bool IsSignedIn => User?.Access_Token != null;

        public void UpdateUser(Genesis.Shared.Users.Tokens user)
        {
            var oldToken = user?.Access_Token;
            var newToken = user?.Access_Token;

            if (oldToken != newToken)
            {
                User = user;

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
