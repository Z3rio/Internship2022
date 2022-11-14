using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using Newtonsoft.Json;

namespace weather
{
    public class TimeObj
    {
        public int day { get; set; }
        public string time { get; set; }
    }
    public class PeriodObj
    {
        public TimeObj close { get; set; }
        public TimeObj open { get; set; }
    }
    public class OpeningHoursObj
    {
        public bool open_now { get; set; }
        public PeriodObj[] periods { get; set; }
    }

    public class PlaceObj
    {
        public OpeningHoursObj opening_hours { get; set; }
    }
    public class ApiObj
    {
        public PlaceObj[] results { get; set; }
    }

    internal class Program
    {
        static async Task<ApiObj> APICallAsync(string url, string urlParameters)
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

        static async Task Main(string[] args)
        {
            string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

            string apiKey = "AIzaSyBeRixqKpyrGhuJ5iwSarFI9abaPcaEkzw";
            string keyword = "mcdonalds";
            int radius = 2000;

            string lat = "57.78029486070066";
            string lon = "14.178692680912373";

            string urlParams = $"?key={apiKey}&keyword={keyword}&radius={radius}&location={lat}%2C{lon}";

            Console.WriteLine($"{baseUrl}{urlParams}");

            var apiData = await APICallAsync(baseUrl, urlParams);

            Console.WriteLine(apiData.results[1].opening_hours.open_now);
        }
    }
}