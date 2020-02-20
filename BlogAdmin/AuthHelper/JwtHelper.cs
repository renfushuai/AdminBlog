using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Blog.Common.Helper;
using Blog.Model.Dto;
using Microsoft.IdentityModel.Tokens;

namespace BlogAdmin.AuthHelper
{
    public class JwtHelper
    {
        ///// <summary>
        ///// 颁发token
        ///// </summary>
        ///// <param name="tokenModel"></param>
        ///// <returns></returns>
        //public static string IssueJwt(TokenModelJwt tokenModel)
        //{
        //    string iss = Appsettings.app(new string[] { "Audience", "Issuer" });
        //    string aud = Appsettings.app(new string[] { "Audience", "Audience" });
        //    string secret= Appsettings.app(new string[] { "Audience", "Secret" });
        //    var claims = new List<Claim>()
        //    {
        //           new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
        //        new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
        //        //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
        //        new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
        //        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1000).ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iss,iss),
        //        new Claim(JwtRegisteredClaimNames.Aud,aud),
        //    };
        //    claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));
        //    //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var jwt = new JwtSecurityToken(
        //        issuer: iss,
        //        claims: claims,
        //        signingCredentials: creds);

        //    var jwtHandler = new JwtSecurityTokenHandler();
        //    var encodedJwt = jwtHandler.WriteToken(jwt);

        //    return encodedJwt;
        //}
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            var tm = new TokenModelJwt
            {
                Uid = jwtToken.Id.ObjToInt(),
                Role = role != null ? role.ObjToString() : ""
            };
            return tm;
        }
        public static dynamic BuildJwtToken(List<Claim> claims,PermissionRequirement permissionRequirement)
        {
            var now = DateTime.Now;
            // 实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience:permissionRequirement.Audience,
                claims:claims,
                notBefore:now,
                expires:now.Add(permissionRequirement.Expiration),
                signingCredentials:permissionRequirement.SigningCredentials
                );
            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new JwtTokenDto
            {
                token = encodedJwt,
                expires_in = permissionRequirement.Expiration.TotalSeconds,
                token_type = "Bearer"
            };
            return responseJson;
        }
    }
    public class TokenModelJwt
    {
        public long Uid { get; set; }
        public string Role { get; set; }
        public string Work { get; set; }
    }
}
