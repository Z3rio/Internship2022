using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace api
{
    internal class Handler
    {
        public static async Task<string?> APICallAsync(string url, string urlParameters, string type)
        {
            HttpClient apiClient = new HttpClient();

            apiClient.BaseAddress = new Uri(url);

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));

            HttpResponseMessage apiResp = apiClient.GetAsync(urlParameters).Result;

            if (apiResp.IsSuccessStatusCode)
            {
                string apiJSON = await apiResp.Content.ReadAsStringAsync();
                string formattedJSON = Regex.Replace(apiJSON, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

                return formattedJSON;
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
