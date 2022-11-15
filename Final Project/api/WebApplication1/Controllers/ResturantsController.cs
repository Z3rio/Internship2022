using Microsoft.AspNetCore.Mvc;
using static api.Struct;
using static api.Handler;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    public class ResturantsController : Controller
    {
        // 
        // GET: /Resturants/
        [AllowCrossSiteJson]
        public async Task<string> Index(string search = "resturant", string radius = "2000")
        {
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(radius))
            {
                string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

                string apiKey = "AIzaSyBeRixqKpyrGhuJ5iwSarFI9abaPcaEkzw";

                string lat = "57.78029486070066";
                string lon = "14.178692680912373";

                string urlParams = $"?key={apiKey}&keyword={search}&radius={radius}&location={lat}%2C{lon}&type=resturant";

                string apiResp = await APICallAsync(baseUrl, urlParams, "application/json");

                if (apiResp != null && apiResp != "")
                {
                    return apiResp;
                }
            }

            return "{\"results\":[]}";
        }
    }
}
