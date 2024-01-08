using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace PresentationLayer.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;


        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<UserContextMiddleware> logger, UserManager<ApplicationUser> userManager)
        {
            string? userContext =  userManager.GetUserId(context.User);
            ApplicationUser user =await userManager.GetUserAsync(context.User);
            if (userContext != null && user !=null)
            {
                UserContext.UserId = user.Id;
                UserContext.UserName =user.UserName;
                UserContext.User = user;
                logger.LogInformation($"User autentification, id = {userContext}");
            }
            else
            {
                throw new Exception("Autorize error");
            }
           

            await _next(context);
        }
    }
}
