using CalendarOfVisits.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarOfVisits.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        
        var usersWithRoles = new List<UserRoleViewModel>();
        
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            usersWithRoles.Add(new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Role = roles.ToList().FirstOrDefault()
            });
        }

        return View(usersWithRoles);
    }

    public async Task<IActionResult> AssignRole(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        

        var roles = await _userManager.GetRolesAsync(user);

        var model = new UserRoleViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            Role = roles.ToList().FirstOrDefault()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(UserRoleViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound();
        }
        
        var currentRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "An error occurred while deleting existing roles");
            return View(model);
        }

        result = await _userManager.AddToRoleAsync(user, model.Role);
        if (result.Succeeded)
        {
            TempData["Success"] = "The role has been successfully assigned";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "An error occurred while assigning the role");
        return View(model);
    }
}
