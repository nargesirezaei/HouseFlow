using HouseFlowPart1.Helper;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using IAuthenticationService = HouseFlowPart1.Interfaces.IAuthenticationService;

namespace HouseFlowPart1.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route("~/register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("~/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!Validation.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("INVALID_MAIL", "Enter a valid Email!");
                return View(model);
            }

           
            try
            {
                await _authenticationService.Register(model.Email, model.Password, model.FirstName, model.LastName);
                TempData["Message"] = "Registration successful.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("REGISTER_FAILED", "Registration failed: " + ex.Message);
            }

            return View(model);
        }

        
        [Route("~/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("~/login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!Validation.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("INVALID_MAIL", "Enter a valid Email!");
                return View(model);
            }


            try
            {
                var result = await _authenticationService.Login(model.Email, model.Password);
                if (result)
                {
                    TempData["Message"] = "Login successful.";

                    var user = await _authenticationService.GetCurrentUserByUsername(model.Email);

                    var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name,model.Email),
                        };

                    var claimIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));


                    return Redirect(returnUrl ?? "/");
                }
                else
                {
                    ModelState.AddModelError("INVALID_AUTH_DATA", "Invalid email or password");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LOGIN_FAILED", "Login failed: " + ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }
    }
}