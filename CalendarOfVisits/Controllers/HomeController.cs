using Microsoft.AspNetCore.Mvc;

namespace CalendarOfVisits.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
