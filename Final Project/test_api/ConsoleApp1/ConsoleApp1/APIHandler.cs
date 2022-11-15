using Newtonsoft.Json;
using System.Net.Http.Headers;
using static resturant.ApiStruct;

namespace resturant
{
    internal class APIHandler
    {
        public static async Task<ApiObj?> APICallAsync(string url, string urlParameters)
        {
            HttpClient apiClient = new HttpClient();

            apiClient.BaseAddress = new Uri(url);

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage apiResp = apiClient.GetAsync(urlParameters).Result;

            if (apiResp.IsSuccessStatusCode)
            {
                string apiJSON = await apiResp.Content.ReadAsStringAsync();
                Console.WriteLine(apiJSON);
                ApiObj apiData = JsonConvert.DeserializeObject<ApiObj>(apiJSON);
                return apiData;
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
