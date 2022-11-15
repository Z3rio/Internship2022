using static resturant.APIHandler;
using static resturant.ApiStruct;

namespace resturant
{
    internal class Program
    {
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

            ApiObj apiData = await APICallAsync(baseUrl, urlParams);

            if (apiData != null)
            {
                Console.WriteLine(apiData.results[1].opening_hours.open_now);
            }
        }
    }
}