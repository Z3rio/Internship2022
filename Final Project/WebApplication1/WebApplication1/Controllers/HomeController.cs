using Microsoft.AspNetCore.Mvc;
using static api.Struct;
using static api.Handler;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // (main website)
        // GET: /
        public async Task<IActionResult> Index()
        {
            string apiResp = await APICallAsync("https://localhost:7115/resturants", "?search=mcdonalds&radius=2000", "text/html");

            ApiObj apiData = JsonConvert.DeserializeObject<ApiObj>(apiResp);

            ViewData["Resturants"] = apiData.results;

            return View();
        }
    }
}
