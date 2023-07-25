using FitSync.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FitSync.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["jwtCookie"] is HttpCookie jwtCookie)
            {
                var token = jwtCookie.Value;

                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;

                    // Check if the token has expired
                    var expirationClaim = tokenS.Claims.FirstOrDefault(c => c.Type == "exp");
                    if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long expirationTime))
                    {
                        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        if (expirationTime <= now)
                        {
                            // Token is expired, invalidate the user by redirecting to the login page
                            httpContext.Response.Redirect("~/Authentication/Login");
                            return false; 
                        }
                        else if (HttpContext.Current.Session["UserProfile"] as User == null)
                        {
                            httpContext.Response.Redirect("~/Authentication/Login");
                            return false;
                        }
                    }

                    // Token is valid, create a custom identity for the user
                    var identity = new ClaimsIdentity(tokenS.Claims, "CustomAuthType");
                    var principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                    return true;
                }
            }

            // Token not present or invalid, redirect to the login page
            httpContext.Response.Redirect("~/Authentication/Login");
            return false; 
        }
    }
}
