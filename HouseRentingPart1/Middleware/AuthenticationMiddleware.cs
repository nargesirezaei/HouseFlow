using HouseFlowPart1.Interfaces;
using System.Security.Principal;

namespace HouseFlowPart1.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthenticationService authenticationService;

        public AuthenticationMiddleware(RequestDelegate next, IAuthenticationService authenticationService)//
        {
            _next = next;
            this.authenticationService = authenticationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the user is authenticated
            context.Items["IsLoggedIn"] = false;
            IIdentity? identity = context.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {

                context.Items["IsLoggedIn"] = true;
                var user = await authenticationService.GetCurrentUserByUsername(identity.Name ?? "");
                context.Items["User"] = $"{user.FirstName} {user.LastName}";
                
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
