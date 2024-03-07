using System.Globalization;
using System.Net.Http.Headers;
using AlpataBLL.BaseResult.Concretes;
using System.Text;
using Newtonsoft.Json;
using AlpataUI.Helpers.AppSettings;

namespace AlpataUI.Helpers.ClientHelper
{
    public class AlpataClient : IAlpataClient
    {
        readonly HttpClient _client;

        public AlpataClient(HttpClient client, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _client = client;
            _client.BaseAddress = new Uri(AppSettingsHelper.GetApiUrl(configuration));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (contextAccessor.HttpContext?.User.FindFirst("AccessToken")?.Value != null)
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + contextAccessor.HttpContext.User.FindFirst("AccessToken")?.Value);
                _client.DefaultRequestHeaders.Add("RefreshToken", contextAccessor.HttpContext.Request.Cookies["RefreshToken"]);
            }
        }
        public async Task<DataResult<TResult>> PostAsync<T, TResult>(T root, string uri)
        {
            if (root is null)
                throw new NullReferenceException(nameof(root) + " can not be null");
            string json = JsonConvert.SerializeObject(root, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            DataResult<TResult>? result = new();

            using (StringContent content = new(json, Encoding.UTF8, "application/json"))
            {
                try
                {
                    HttpResponseMessage response = await _client.PostAsync(uri, content);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result.Success = false;
                        result.Message = "Url Not Found";
                    }
                    else
                    {
                        string stringResult = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<DataResult<TResult>>(stringResult);
                    }
                }
                catch (Exception ex)
                {
                    if (result is not null)
                    {
                        result.Success = false;
                        result.Message = $"Add: {uri}. {ex}";
                    }
                }
            }
            return result ?? new ErrorDataResult<TResult>("Internal Server Error");
        }

        public async Task<DataResult<T>> Add<T>(T root, string uri)
        {
            if (root is null)
                throw new NullReferenceException(nameof(root) + " can not be null");

            string json = JsonConvert.SerializeObject(root, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            DataResult<T>? result = new();
            using (StringContent content = new(json, Encoding.UTF8, "application/json"))
            {
                try
                {
                    HttpResponseMessage response = await _client.PostAsync(uri, content);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result.Success = false;
                        result.Message = "Url Not Found";
                    }
                    else
                    {
                        string stringResult = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<DataResult<T>>(stringResult);
                    }
                }
                catch (Exception ex)
                {
                    if (result is not null)
                    {
                        result.Success = false;
                        result.Message = $"Add: {uri}. {ex}";
                    }
                }
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }

        public async Task<DataResult<T>> Update<T>(T root, string uri)
        {
            if (root is null)
                throw new NullReferenceException(nameof(root) + " can not be null");

            string json = JsonConvert.SerializeObject(root, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            DataResult<T>? result = new();
            using (StringContent content = new(json, Encoding.UTF8, "application/json"))
            {
                try
                {
                    HttpResponseMessage response = await _client.PostAsync(uri, content);
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        result.Success = false;
                        result.Message = "Url Not Found";
                    }
                    else
                    {
                        string stringResult = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<DataResult<T>>(stringResult);
                    }
                }
                catch (Exception ex)
                {
                    if (result is not null)
                    {
                        result.Success = false;
                        result.Message = $"Update: {uri}. {ex}";
                    }
                }
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }

        public async Task<DataResult<T>> Action<T, TData>(string uri, TData root)
        {
            if (root is null)
                throw new NullReferenceException(nameof(root) + " can not be null");

            DataResult<T>? result = new();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(root), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(uri, content);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Success = false;
                    result.Message = "Url Not Found";
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DataResult<T>>(data);
                }
            }
            catch (Exception ex)
            {
                if (result is not null) result.Message = $"Action: {uri}. {ex}";
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }

        public async Task<DataResult<T>> Get<T>(string uri, T root)
        {
            DataResult<T>? result = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Success = false;
                    result.Message = "Url Not Found";
                }
                else
                {
                    string stringResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DataResult<T>>(stringResult);
                }
            }
            catch (Exception ex)
            {
                if (result is not null) result.Message = $"Get: {uri}. {ex}";
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }

        public async Task<DataResult<T>> GetNoRoot<T>(string uri)
        {
            DataResult<T>? result = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Success = false;
                    result.Message = "Url Not Found";
                }
                else
                {
                    string stringResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DataResult<T>>(stringResult);
                }
            }
            catch (Exception ex)
            {
                if (result is not null) result.Message = $"Get: {uri}. {ex}";
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }

        public async Task<DataResult<List<T>>> GetList<T>(string uri)
        {
            DataResult<List<T>>? result = new();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Success = false;
                    result.Message = "Url Not Found";
                }
                else
                {
                    var stringResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DataResult<List<T>>>(stringResult);
                }
            }
            catch (Exception ex)
            {
                if (result is not null) result.Message = $"Get: {uri}. {ex}";
            }
            return result ?? new ErrorDataResult<List<T>>("Internal Server Error");
        }

        public async Task<DataResult<T>> Delete<T>(string uri)
        {
            DataResult<T>? result = new()
            {
                Success = true
            };

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.Success = false;
                    result.Message = "Url Not Found";
                }
                else
                {
                    string stringResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<DataResult<T>>(stringResult);
                }
            }
            catch (Exception ex)
            {
                if (result is not null)
                {
                    result.Success = false;
                    result.Message = $"Delete: {uri}. {ex}";
                }
            }
            return result ?? new ErrorDataResult<T>("Internal Server Error");
        }
    }
}
