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

// {"coord":{"lon":18.0632,"lat":59.3346},"weather":[{"id":804,"main":"Clouds","description":"overcast clouds","icon":"04d"}],"base":"stations","main":{"temp":280.83,"feels_like":278.51,"temp_min":279.18,"temp_max":281.98,"pressure":1022,"humidity":91},"visibility":10000,"wind":{"speed":3.6,"deg":240},"clouds":{"all":100},"dt":1668428437,"sys":{"type":1,"id":1788,"country":"SE","sunrise":1668407926,"sunset":1668435943},"timezone":3600,"id":2673730,"name":"Stockholm","cod":200}

namespace weather
{
    public class MainObj
    {
        public float temp { get; set; }
    }
    public class WeatherObj
    {
        public MainObj main { get; set; }
        public string name { get; set; }
    }

    internal class Program
    {
        static async Task<WeatherObj> APICallAsync(string url, string urlParameters)
        {
            HttpClient apiClient = new HttpClient();

            apiClient.BaseAddress = new Uri(url);

            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage apiResp = apiClient.GetAsync(urlParameters).Result;

            if (apiResp.IsSuccessStatusCode)
            {
                string apiJSON = await apiResp.Content.ReadAsStringAsync();
                WeatherObj apiData = JsonConvert.DeserializeObject<WeatherObj>(apiJSON);
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
            string baseUrl = "http://api.openweathermap.org/data/2.5/forecast";

            Console.WriteLine("Input latitude: ");
            string lat = Console.ReadLine();

            Console.WriteLine("Input longitude: ");
            string lon = Console.ReadLine();

            string urlParams = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=fcb474d654a08751798995f3ae4a2508";

            var apiData = await APICallAsync(baseUrl, urlParams);
            Console.WriteLine($"Det är just nu {apiData.main.temp - 273.15} celcius i \"{apiData.name}\"");
        }
    }
}
