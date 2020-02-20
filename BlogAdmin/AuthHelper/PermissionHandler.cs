using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Common.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlogAdmin.AuthHelper
{
    public class PermissionHandler:AuthorizationHandler<PermissionRequirement>
    {
        public IAuthenticationSchemeProvider Schemes { get; set; }
        private readonly IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="accessor"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            Schemes = schemes;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;
            var permissionsList = requirement.Permissions;
            if (!permissionsList.Any())
            {
               // var data = await _roleModulePermissionServices.RoleModuleMaps();
               // var list = (from item in data
                //            where item.IsDeleted == false
                //            orderby item.Id
                //            select new PermissionItem
                //            {
                 //               Url = item.Module?.LinkUrl,
                 //               Role = item.Role?.Id.ObjToString(),
                  //          }).ToList();
                requirement.Permissions = new List<PermissionItem>() {
                    new PermissionItem{Role="Admin",Url="/WeatherForecast/UpdateWeather"}
                };
            }
            if (httpContext!=null)
            {
                var requestUrl = httpContext.Request.Path.Value.ToLower();
                //判断请求是否停止 TODO没懂
                var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
                foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
                {
                    if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                    {
                        context.Fail();
                        return;
                    }
                }
                //“登录用户名”，“email”，“用户Id”就是ClaimType。
                //“身份证号码：xxx”是一个claim，“姓名：xxx”是另一个claim。
                //一组claims构成了一个identity，具有这些claims的identity就是 ClaimsIdentity ，驾照就是一种ClaimsIdentity，可以把ClaimsIdentity理解为“证件”，驾照是一种证件，护照也是一种证件。
                //ClaimsIdentity的持有者就是 ClaimsPrincipal ，一个ClaimsPrincipal可以持有多个ClaimsIdentity，就比如一个人既持有驾照，又持有护照。
                var defaultAuthticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthticate==null)
                {       
                    context.Fail();
                    return;
                }
                var result = await httpContext.AuthenticateAsync();
                if (result?.Principal == null)
                {
                    context.Fail();
                    return;
                }
                // 获取当前用户的角色信息
                var currentUserRoles =httpContext.User.Claims.Where(m => m.Type == requirement.ClaimType).Select(m => m.Value).ToList();
                if (currentUserRoles.Count <= 0)
                {
                    context.Fail();
                    return;
                }
                var isMatchRole = false;
                var currentUrls = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role)).Select(m=>m.Url).ToList();
                foreach (var item in currentUrls)
                {
                    
                        if (Regex.Match(requestUrl, item.ToLower())?.Value == requestUrl)
                        {
                            isMatchRole = true;
                            break;
                        }
                }

                //验证权限
                //if (currentUserRoles.Count <= 0 || requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl).Count() <= 0)
                if (!isMatchRole)
                {
                    context.Fail();
                    return;
                }
                //判断过期时间（这里仅仅是最坏验证原则，你可以不要这个if else的判断，因为我们使用的官方验证，Token过期后上边的result?.Principal 就为 null 了，进不到这里了，因此这里其实可以不用验证过期时间，只是做最后严谨判断）
                if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                    return;
                }
                //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
                //if (!requestUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
                //{
                //    context.Fail();
                //    return;
                //}
            }
            context.Succeed(requirement);
        }
    }
}
