using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BlogAdmin.Extemsions
{
    public static class SwaggerSetup
    {
     public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services==null)
            {
                throw new ArgumentException(nameof(services));
            }
            var basePath = AppContext.BaseDirectory;
            var apiName = "Blog";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version="v1",
                    Title=$"{apiName} 接口文档",
                    Description=$"{apiName} Http API v1",
                    //Contact = new OpenApiContact { Name = apiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    //License = new OpenApiLicense { Name = apiName + " 官方文档", Url = new Uri("http://apk.neters.club/.doc/") }
                });
                var xmlPath = Path.Combine(basePath, "BlogAdmin.xml");
                c.IncludeXmlComments(xmlPath,true);
                #region Token绑定到ConfigureServices
                //开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
        }
    }
}
