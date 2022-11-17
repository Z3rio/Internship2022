using Microsoft.AspNetCore.Mvc;
using static api.Handler;
using Newtonsoft.Json;
using WebApplication1.Models;
using System.Linq;
using static WebApplication1.Models.ResturantsModel;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class ApiController : Controller
    { 
        List<string> ROTD = new List<string> { "ChIJzwAKy8WxEmsRh-SqQrC5mnk" };

        // 
        // GET: /resturants/search
        [AllowCrossSiteJson]
        [Route("/resturants/search")]
        public async Task<IActionResult> GetResturants(
            string sort, bool onlyOpenNow,
            int maxPrice = 5, int minPrice = 0,
            string search = "resturant", string radius = "2000", 
            string lat = "57.78029486070066", string lon = "14.178692680912373"
        )
        {
            if (
                string.IsNullOrEmpty(search) == false && string.IsNullOrEmpty(radius) == false && 
                (sort == null || (sort == "rating" || sort == "alphabetical" || sort == "expensive" || sort == "cheap" || sort == "distance" || sort == "opennow" || sort == "closednow")) && 
                minPrice >= 0 && minPrice <= 5 && maxPrice >= 0 && maxPrice <= 5 && minPrice <= maxPrice
            )
            {
                string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
                string urlParams = $"?key={Program.apiKey}&keyword={search}&location={lat}%2C{lon}&type=resturant&maxprice={maxPrice}&minprice={minPrice}";

                if (sort == "distance")
                {
                    urlParams += "&rankby=distance";
                } else
                {
                    urlParams += $"&radius={radius}";
                }

                if (onlyOpenNow == true)
                {
                    urlParams += "&opennow";
                }

                string? apiResp = await APICallAsync(baseUrl, urlParams, "application/json");
                    
                if (apiResp != null)
                {
                    UnsortedResults? apiObj = JsonConvert.DeserializeObject<UnsortedResults>(apiResp);

                    if (apiObj != null && (sort != null && sort != "distance") && apiObj.status != null && apiObj.results != null)
                    {
                        IOrderedEnumerable<PlaceObj>? sortedResturants = null;

                        switch (sort)
                        {
                            case "alphabetical":
                                sortedResturants = apiObj.results.OrderBy(el => el.name);
                                break;
                            case "rating":
                                sortedResturants = apiObj.results.OrderByDescending(el => el.rating);
                                break;
                            case "expensive":
                                sortedResturants = apiObj.results.OrderByDescending(el => el.price_level);
                                break;
                            case "cheap":
                                sortedResturants = apiObj.results.OrderBy(el => el.price_level);
                                break;
                            case "opennow":
                                sortedResturants = apiObj.results.OrderByDescending((el) =>
                                {
                                    if (el.opening_hours != null)
                                    {
                                        return el.opening_hours.open_now;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                });
                                break;
                            case "closednow":
                                sortedResturants = apiObj.results.OrderBy((el) => {
                                    if (el.opening_hours != null)
                                    {
                                        return el.opening_hours.open_now;
                                    } else
                                    {
                                        return false;
                                    }
                                });
                                break;
                        }

                        if (sortedResturants != null)
                        {
                            return Ok(new SortedResults
                            {
                                results = sortedResturants,
                                status = apiObj.status
                            });
                        }
                        else
                        {
                            return NoContent();
                        }
                    }

                    return Ok(apiObj);
                }
                else
                {
                    return NoContent();
                }
            }

            return BadRequest();
        }

        [Route("/resturants/getresturant")]
        public async Task<IActionResult> GetResturantFromPlaceId(string PlaceId)
        {
            if (PlaceId != null)
            {
                string? apiResp = await APICallAsync("https://maps.googleapis.com/maps/api/place/details/json", $"?key={Program.apiKey}&place_id={PlaceId}", "application/json");
                
                if (apiResp != null)
                {
                    PlaceResult? apiObj = JsonConvert.DeserializeObject<PlaceResult>(apiResp);
                    
                    if (apiObj != null && apiObj.status == "OK")
                    {
                        return Ok(apiObj.result);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return NoContent();
                }
            } 

            return BadRequest();
        }

        [Route("/resturants/resturantsoftheday")]
        public async Task<IActionResult> GetResturantsOfTheDay()
        {
            if (ROTD.Count != 0)
            {
                List<PlaceObj> RetVal = new List<PlaceObj> { };

                for (int i = 0; i < ROTD.Count; i++)
                {
                    string PlaceId = ROTD[i];

                    string? apiResp = await APICallAsync("https://localhost:7115/resturants/getresturant", $"?placeId={PlaceId}", "application/json");
                        
                    if (apiResp != null)
                    {
                        PlaceObj? apiObj = JsonConvert.DeserializeObject<PlaceObj>(apiResp);

                        if (apiResp != null && apiObj != null)
                        {
                            RetVal.Add(apiObj);
                        }
                    }
                    else
                    {
                        return NoContent();
                    }
                }

                return Ok(RetVal);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
