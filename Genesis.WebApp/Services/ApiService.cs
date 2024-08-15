using Genesis.Shared;
using Genesis.WebApp.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Genesis.WebApp.Services
{
    public class ApiService : IApiService
    {
        private string BaseUrl = "";

        private HttpClient httpClient;
        private IConsoleLogService console;

        public ApiService(HttpClient _httpClient, IConsoleLogService _console, IConfiguration configuration)
        {
            httpClient = _httpClient;
            console = _console;
            BaseUrl = configuration.GetSection("settings").Get<Settings>().ApiUrl;
        }

        public void SetToken(string token)
        {

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public void ClearToken()
        {
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string Path, IDictionary<string, string> Params = null)
        {
            HttpResponseMessage response = await httpClient.GetAsync(BuildUri(Path, Params));

            return await ApiResponse<T>.CreateAsync(response);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string Path, IDictionary<string, string> Params, object Value = null)
        {
            if (Value == null) Value = string.Empty;
            string valueString = JsonSerializer.Serialize(Value);
            StringContent content = new StringContent(valueString, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(BuildUri(Path, Params), content);

            return await ApiResponse<T>.CreateAsync(response);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string Path, object Value = null)
        {
            return await PutAsync<T>(Path, null, Value);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string Path, IDictionary<string, string> Params, object Value = null)
        {
            if (Value == null) Value = string.Empty;
            string valueString = JsonSerializer.Serialize(Value);
            StringContent content = new StringContent(valueString, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(BuildUri(Path, Params), content);

            return await ApiResponse<T>.CreateAsync(response);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string Path, object Value = null)
        {
            return await PostAsync<T>(Path, null, Value);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string Path, IDictionary<string, string> Params = null)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync(BuildUri(Path, Params));

            return await ApiResponse<T>.CreateAsync(response);
        }

        protected string BuildUri(string Path, IDictionary<string, string> Params = null)
        {
            UriBuilder result = new UriBuilder($"{BaseUrl}{Path}");

            if (Params != null && Params.Count > 0)
            {
                foreach (string key in Params.Keys)
                {
                    string queryPart = key + "=" + Params[key];
                    if (result.Query != null && result.Query.Length > 1)
                        result.Query = result.Query.Substring(1) + "&" + queryPart;
                    else
                        result.Query = queryPart;
                }
            }

            return result.Uri.AbsoluteUri;
        }
    }

    public class ApiResponse<T>
    {
        public T Value { get; set; }
        public ErrorMessages Errors { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public bool HasSuccessStatusCode { get; set; }

        private ApiResponse()
        {

        }

        private async Task<ApiResponse<T>> InitalizeAsync(HttpResponseMessage response)
        {
            StatusCode = response.StatusCode;
            HasSuccessStatusCode = response.IsSuccessStatusCode;
            Errors = new ErrorMessages();
            Value = default(T);
            var data = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    Value = JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    ErrorMessages errors = JsonSerializer.Deserialize<ErrorMessages>(data, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    Errors = errors;
                }
                catch (Exception ex)
                {
                    Errors = new ErrorMessages
                    {
                         title = ex.Message,
                          detail = ex.StackTrace
                    };
                }
            }

            return this;
        }

        public static Task<ApiResponse<T>> CreateAsync(HttpResponseMessage response)
        {
            var result = new ApiResponse<T>();
            return result.InitalizeAsync(response);
        }
    }



}
