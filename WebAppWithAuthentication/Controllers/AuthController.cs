using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppWithAuthentication.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAppWithAuthentication.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly string username = "admin";

        private readonly string password = "admin";
        

        // GET: /<controller>/
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                return View(model);
            }

            if (!username.Equals(model.Username,
                StringComparison.InvariantCultureIgnoreCase) ||
                !password.Equals(model.Password))
            {
                TempData["failed"] = "failed";
                ModelState.AddModelError("Error", "Username or password is invalid");
                return View(model);
            }

            var claims = new List<Claim>()
            {
                new Claim("fullname", model.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            //claimsIdentity.AddClaim(new Claim("fullname", model.Username));

            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(principal);

            var q = this.HttpContext.Request.Query["keyword"];

           // return Redirect("~/Protected/Index");
            return RedirectToAction("Index", "Protected");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }
    }
}
