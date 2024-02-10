namespace BlogManagementSystem.Authorization;

using System.Text;
using Microsoft.AspNetCore.Http;


public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Un Authorized");
            return;
        }


        var header = context.Request.Headers["Authorization"].ToString();
        var encodedCreds = header.Substring(6);
        var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));
        string[] uidpwd = creds.Split(':');
        var uid = uidpwd[0];
        var password = uidpwd[1];

        if (uid != "admin" || password != "admin")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("unauthorized Incorrect User Name Or Password ");
            return;
        }


        
        await _next(context);
    }

}