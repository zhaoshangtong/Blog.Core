using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper.Overwrite
{
    /// <summary>
    /// JWT
    /// </summary>
    public class JwtTokenAuth
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            //检测是否包含'Authorization'请求头
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(context);
            }

            var token= context.Request.Headers["Authorization"].ToString();
            TokenModelJWT tm = JwtHelper.SerializeJWT(token);//序列化token，获取授权

            var claims = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tm.Role);
            claims.Add(claim);
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            return _next(context);
        }
    }
}
