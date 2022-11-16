using Microsoft.AspNetCore.Mvc;
using static api.Handler;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ApiController : Controller
    {
        // 
        // GET: /resturants/search
        [AllowCrossSiteJson]
        [Route("/resturants/search")]
        public async Task<IActionResult> GetResturants(
            string sort, 
            string search = "resturant", string radius = "2000", string lat = "57.78029486070066", string lon = "14.178692680912373"
        )
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            if (
                string.IsNullOrEmpty(search) == false && string.IsNullOrEmpty(radius) == false && 
                (sort == null || (sort == "rating" || sort == "alphabetical" || sort == "expensive" || sort == "cheap"))
            )
            {
                string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

                string urlParams = $"?key={config["GOOGLE_API_KEY"]}&keyword={search}&radius={radius}&location={lat}%2C{lon}&type=resturant";

                string apiResp = await APICallAsync(baseUrl, urlParams, "application/json");
                ResturantsModel apiObj = JsonConvert.DeserializeObject<ResturantsModel>(apiResp);

                if (apiObj != null && sort != null)
                {
                    IOrderedEnumerable<PlaceObj> sortedResturants = null;

                    if (sort == "alphabetical")
                    {
                        sortedResturants = from s in apiObj.results orderby s.name select s;
                    } 
                    else if (sort == "rating")
                    {
                        sortedResturants = from s in apiObj.results orderby s.rating descending select s;
                    } 
                    else if (sort == "expensive")
                    {
                        sortedResturants = from s in apiObj.results orderby s.price_level descending select s;
                    }
                    else if (sort == "cheap")
                    {
                        sortedResturants = from s in apiObj.results orderby s.price_level ascending select s;
                    }

                    if (sortedResturants != null)
                    {
                        return Ok(sortedResturants);
                    } 
                    else
                    {
                        return NoContent();
                    }
                }

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

            return BadRequest();
        }
    }
}
