using System;
using Microsoft.AspNetCore.Builder;

namespace BlogAdmin.Middlewares
{
    public static class MiddlewareHelper
    {
        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
     public static IApplicationBuilder UseExecptionHandlerMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
