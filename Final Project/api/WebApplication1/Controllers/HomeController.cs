using Microsoft.AspNetCore.Mvc;
using static api.Handler;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // (main website)
        // GET: /
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
