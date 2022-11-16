using Microsoft.AspNetCore.Mvc;
using static api.Handler;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ResturantsController : Controller
    {
        // 
        // GET: /Resturants/
        [AllowCrossSiteJson]
        [Route("/resturants/search")]
        public async Task<IActionResult> GetResturants(string search = "resturant", string radius = "2000")
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            if (string.IsNullOrEmpty(search) == false && string.IsNullOrEmpty(radius) == false)
            {
                string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

                string lat = "57.78029486070066";
                string lon = "14.178692680912373";

                string urlParams = $"?key={config["GOOGLE_API_KEY"]}&keyword={search}&radius={radius}&location={lat}%2C{lon}&type=resturant";

                string apiResp = await APICallAsync(baseUrl, urlParams, "application/json");
                ResturantsModel apiObj = JsonConvert.DeserializeObject<ResturantsModel>(apiResp);

                if (apiResp != null && apiResp != "")
                {
                    //return apiResp;
                    return Ok(apiObj);
                }
                else
                {
                    return NoContent();
                }
            }

            //return "{\"results\":[]}";
            return BadRequest();
        }
    }
}
