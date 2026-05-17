using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{
    public static class GenericApiClient
    {
        private const string BaseUrl = "http://localhost:50954/";
        private static HttpClient _httpClient;

        private static void Init()
        {
            if (_httpClient == null)
                _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        }

        public static async Task<TResponse> GetAsync<TResponse>(string path)
        {
            Init();
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data)
        {
            Init();
            string json = data != null ? JsonConvert.SerializeObject(data) : "null";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(path, content);
            response.EnsureSuccessStatusCode();
            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public static async Task<TResponse> PutAsync<TRequest, TResponse>(string path, TRequest data)
        {
            Init();
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(path, content);
            response.EnsureSuccessStatusCode();
            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public static async Task DeleteAsync(string path)
        {
            Init();
            var response = await _httpClient.DeleteAsync(path);
            response.EnsureSuccessStatusCode();
        }
    }
}
