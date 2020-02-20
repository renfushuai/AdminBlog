using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.Common.Helper;
using BlogAdmin.AuthHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BlogAdmin.Extemsions
{
    public static class AuthorizationSetup
    {
       public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            if (services==null)
            {
                throw new ArgumentException(nameof(services));
            }
            #region 参数
            //读取配置文件
            var symmetricKeyAsBase64 = Appsettings.app(new string[] { "Audience", "Secret" });
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = Appsettings.app(new string[] { "Audience", "Issuer" });
            var Audience = Appsettings.app(new string[] { "Audience", "Audience" });

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                Issuer,//发行人
                Audience,//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
                );
            #endregion
            //策略授权
            services.AddAuthorization(option =>
            {
                option.AddPolicy(Permissions.Name, policy => policy.Requirements.Add(permissionRequirement));
            });


            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = Issuer,//发行人

                ValidateAudience = true,
                ValidAudience = Audience,//订阅人

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };
            //认证  开启Bearer认证
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultForbidScheme = nameof(ApiResponseHandler);
                x.DefaultChallengeScheme = nameof(ApiResponseHandler);
            })
            .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = tokenValidationParameters;
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                }).AddScheme<AuthenticationSchemeOptions,ApiResponseHandler>(nameof(ApiResponseHandler),o=> { });
            // 注入权限处理器
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
        }
    }
}
