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
            // Display the registration view
            return View();
        }

        [HttpPost]
        [Route("~/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return to the registration view
                return View(model);
            }

            if (!Validation.IsValidEmail(model.Email))
            {
                // Check for valid email format
                ModelState.AddModelError("INVALID_MAIL", "Enter a valid Email!");
                return View(model);
            }

           
            try
            {
                // Attempt to register the user
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
            // Display the login view
            return View();
        }

        [HttpPost]
        [Route("~/login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return to the login view
                return View(model);
            }
            if (!Validation.IsValidEmail(model.Email))
            {
                // Check for valid email format
                ModelState.AddModelError("INVALID_MAIL", "Enter a valid Email!");
                return View(model);
            }


            try
            {
                // Attempt to log in the user
                var result = await _authenticationService.Login(model.Email, model.Password);
                if (result)
                {
                    TempData["Message"] = "Login successful.";

                    var user = await _authenticationService.GetCurrentUserByUsername(model.Email);

                    // Create claims for authentication
                    var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name,model.Email),
                        };

                    var claimIdentity = new ClaimsIdentity(claims, "Login");
                    // Sign in the user with claims
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
            // Sign out the user and redirect to the home page
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }
    }
}