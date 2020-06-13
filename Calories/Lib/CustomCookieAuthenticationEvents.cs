using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Calories.Models;
using Calories.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    private readonly AuthService _auth;

    public CustomCookieAuthenticationEvents(AuthService auth)
    {
        _auth = auth;
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        ClaimsPrincipal userPrincipal = context.Principal;

        Person who = await _auth.GetCurrentPersonAsync(userPrincipal);

        if (who == null)
        {
            context.RejectPrincipal(); 
            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}