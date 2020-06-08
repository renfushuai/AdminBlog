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
