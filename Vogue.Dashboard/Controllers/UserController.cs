using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vogue.Dashboard.Models;
using VogueCore.Entities.Identity;

namespace Vogue.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //public async Task< IActionResult> Index()
        //{
        //    var users = await _userManager.Users.Select(u => new UserViewModel()
        //    {
        //        Id = u.Id,
        //        UserName = u.UserName,
        //        DisplayName = u.DisplayName,
        //        Email = u.Email,
        //        Roles = _userManager.GetRolesAsync(u).Result


        //    }).ToListAsync(); 
        //    return View(users);
        //}
        

        public async Task<IActionResult> Index()
        {
            // Fetch the users first without fetching roles inside the query
            var users = await _userManager.Users
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Email = u.Email
                    // Do not fetch Roles inside the Select query
                })
                .ToListAsync();

            // Now fetch the roles asynchronously for each user
            foreach (var user in users)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }

            return View(users);
        }





        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var AllRoles = await _roleManager.Roles.ToListAsync();
            var userRoleViewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.DisplayName,
                Roles = AllRoles.Select(R => new RoleViewModel
                {
                    Id = R.Id,
                    Name = R.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, R.Name).Result
                }).ToList()
            };
            return View(userRoleViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var User = await _userManager.FindByIdAsync(model.UserId);
            var UserRoles = await _userManager.GetRolesAsync(User);
            foreach (var Role in model.Roles)
            {
                if (UserRoles.Any(R => R == Role.Name) && !Role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(User, Role.Name);
                if (!UserRoles.Any(R => R == Role.Name) && Role.IsSelected)
                    await _userManager.AddToRoleAsync(User, Role.Name);
            }
            return RedirectToAction(nameof(Index));
        }








    }
}
