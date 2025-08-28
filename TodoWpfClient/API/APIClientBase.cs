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
            if (string.IsNullOrWhiteSpace(apiUrl)) 
            {  
                throw new ArgumentNullException(nameof(apiUrl));
            }

            _apiUrl = apiUrl;

            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri(_apiUrl)
            };

            HttpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        protected async Task<TReturn> GetAsync<TReturn>(string relativeUri)
        {
            HttpResponseMessage res = HttpClient.GetAsync(relativeUri, HttpCompletionOption.ResponseHeadersRead).Result;

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
            HttpResponseMessage res = HttpClient.PostAsJsonAsync<TRequest>(relativeUri, request).Result;
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
            HttpResponseMessage res = HttpClient.PutAsJsonAsync<TRequest>($"{_apiUrl}/{relativeUri}", request).Result;
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