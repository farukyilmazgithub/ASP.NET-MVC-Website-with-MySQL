using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Models;
using mvc.Controllers;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
namespace mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly MvcTextContext _context;
        public LoginController(MvcTextContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel Loguser)
        {
            var users = _context.SystemUsers.ToList();

            if (ModelState.IsValid)
            {
                var isUser = _context.SystemUsers.FirstOrDefault(x => x.Email == Loguser.Email && x.Password == Loguser.Password);
                
                if (isUser != null)
                {
                    List<Claim> userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.Name));
                    userClaims.Add(new Claim(ClaimTypes.Surname, isUser.SurName.ToString()));
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = Loguser.IsRememberMe
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Index", "AdminPanel");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect please try again");
                }
            }    
            return View();
        }
 
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Site");
        }
    }
}