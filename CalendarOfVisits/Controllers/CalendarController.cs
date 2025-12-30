using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarOfVisits.Controllers
{
    [Authorize(Roles = "Manager,Sales Representative")]
    public class CalendarController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public CalendarController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Role = roles.ToList().FirstOrDefault();
            
            return View();
        }
    }
}
