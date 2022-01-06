using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Api.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task Invoke(HttpContext context, EfCoreUserRepository userService, IJwtUtils jwtUtils)
        //{
        //    var token = context.Request.Headers["x-auth-token"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = jwtUtils.ValidateToken(token);
        //    if (userId != null)
        //    {
        //        // attach user to context on successful jwt validation
        //        context.Items["User"] = await userService.Get(userId.Value);
        //    }

        //    await _next(context);
        //}
    }
}
