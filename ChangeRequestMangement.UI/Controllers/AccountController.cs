using ChangeRequestManagement.BusinessLayer.Services.Abstraction;
using ChangeRequestManagment.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChangeRequestMangement.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContext;

        public AccountController
            (
            IAccountService accountService,
            IHttpContextAccessor httpContext
            )
        {
            _accountService = accountService;
            _httpContext = httpContext;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                LoginViewModel model = new LoginViewModel() { ReturnUrl = ReturnUrl };
                return View(model);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = await _accountService.LoginPersonAsync(model.EmailAddress, model.Password);
                if (person.IsSuccess)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(nameof(person.PersonId), person.PersonId.ToString()),
                        new Claim(nameof(person.FirstName), person.FirstName),
                        new Claim(nameof(person.LastName), person.LastName),
                        new Claim(ClaimTypes.Email, person.EmailAddress),
                        new Claim(ClaimTypes.Role, person.Role)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();

                    await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                    return Redirect(model.ReturnUrl ?? "/Dashboard/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}