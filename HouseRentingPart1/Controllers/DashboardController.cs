using HouseFlowPart1.Interfaces;
using HouseFlowPart1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace HouseFlowPart1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public DashboardController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Check if the user is authenticated
            IIdentity? identity = User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                // Get the username of the authenticated user
                string? username = identity.Name;
                // Retrieve the current user's information using the username
                Users currentUser = await _authenticationService.GetCurrentUserByUsername(username ?? "");
                // Use the currentUser object as needed
                return View(currentUser);
            }
            else
            {
                // Handle the case when the user is not authenticated
                return View();
            }
        }
    }
}
