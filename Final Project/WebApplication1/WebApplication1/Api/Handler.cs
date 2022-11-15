using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace api
{
    internal class Handler
    {
        public static async Task<string?> APICallAsync(string url, string urlParameters)
        {
            HttpClient apiClient = new HttpClient();

            apiClient.BaseAddress = new Uri(url);

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage apiResp = apiClient.GetAsync(urlParameters).Result;

            if (apiResp.IsSuccessStatusCode)
            {
                string apiJSON = await apiResp.Content.ReadAsStringAsync();
                return apiJSON;
            }
            else
            {
                Console.WriteLine($"{(int)apiResp.StatusCode} ({apiResp.ReasonPhrase})");
            }

            apiClient.Dispose();

            return null;
        }
    }
}
