using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace TodoWpfClient.API
{
    public abstract class ApiClientBase
    {
        private readonly string _apiUrl;
        protected readonly HttpClient HttpClient;

        public ApiClientBase(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        protected async Task<TReturn> GetAsync<TReturn>(string relativeUri)
        {
            HttpResponseMessage res = await HttpClient.GetAsync($"{_apiUrl}/{relativeUri}");

            if (res.IsSuccessStatusCode)
            {
                if (res.StatusCode == HttpStatusCode.NoContent)
                    return default;
                else
                    return await res.Content.ReadFromJsonAsync<TReturn>();
            }
            else
            {
                string msg = await res.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }

        protected async Task<TReturn> PostAsync<TReturn, TRequest>(string relativeUri, TRequest request)
        {
            HttpResponseMessage res = await HttpClient.PostAsJsonAsync<TRequest>($"{_apiUrl}/{relativeUri}", request);
            if (res.IsSuccessStatusCode)
            {
                if (res.StatusCode == HttpStatusCode.NoContent)
                    return default;
                else
                    return await res.Content.ReadFromJsonAsync<TReturn>();
            }
            else
            {
                string msg = await res.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
        
        protected async Task PutAsync<TRequest>(string relativeUri, TRequest request)
        {
            HttpResponseMessage res = await HttpClient.PutAsJsonAsync<TRequest>($"{_apiUrl}/{relativeUri}", request);
            if (res.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                string msg = await res.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
    }
}