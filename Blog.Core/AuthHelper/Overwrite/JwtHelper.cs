using Blog.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper.Overwrite
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtHelper
    {
        private static string secretKey= "sdfsdfsrty45634kkhllghtdgdfss345t678fs";
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJWT model)
        {
            var dateTime = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim (JwtRegisteredClaimNames.Jti,model.Uid.ToString()),
                new Claim ("Role",model.Role),
                new Claim (JwtRegisteredClaimNames.Iat,dateTime.ToString(),ClaimValueTypes.Integer64)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(issuer: "Blog.Core", claims: claims, expires: dateTime.AddHours(2), signingCredentials: creds);
            var jwtHander = new JwtSecurityTokenHandler();
            var token = jwtHander.WriteToken(jwt);
            return token;
        }
        /// <summary>
        /// 解析JWT
        /// </summary>
        /// <param name="JwtStr"></param>
        /// <returns></returns>
        public static TokenModelJWT SerializeJWT(string JwtStr)
        {
            var jwtHander = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHander.ReadJwtToken(JwtStr);
            object role = new object(); ;
            try
            {
                jwtToken.Payload.TryGetValue("Role", out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModelJWT
            {
                Uid = int.Parse(jwtToken.Id),
                Role = role != null ? role.ToString() : "",
            };
            return tm;
        }
    }
}
