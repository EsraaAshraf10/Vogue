using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VogueApis.DTOs;
using VogueCore.Entities.Identity;

namespace Vogue.Dashboard.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDto login)
        //{
        //    var user = await _userManager.FindByEmailAsync(login.Email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("Email", "Email Is Invalid");
        //        return RedirectToAction(nameof(Login));
        //    }
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        //    if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "Admin"))
        //    {
        //        ModelState.AddModelError(string.Empty, "you Are Not Authorized");
        //        return RedirectToAction(nameof(Login));
        //    }
        //    else
        //        return RedirectToAction("Index", "Home");
        //}


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
           
            if (string.IsNullOrWhiteSpace(login.Email))
            {
                ModelState.AddModelError("Email", "");
                return View(login); 
            }

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is invalid.");
                return View(login);
            }
            if (string.IsNullOrWhiteSpace(login.Password))
            {
                ModelState.AddModelError("Password", "");
                return View(login);
            }
            #region Old Login
            //var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            //if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "Admin"))
            //{
            //    ModelState.AddModelError(string.Empty, "You are not authorized.");
            //    return View(login);
            //}
            #endregion
            #region New Login

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded ||
                !(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Employee")))
            {
                ModelState.AddModelError(string.Empty, "You are not authorized.");
                return View(login);
            }

            #endregion
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
