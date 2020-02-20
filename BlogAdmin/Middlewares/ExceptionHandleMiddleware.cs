using System;
using System.Net;
using System.Threading.Tasks;
using Blog.Common.LogsMethod;
using Blog.Model;
using Microsoft.AspNetCore.Http;

namespace BlogAdmin.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        public readonly RequestDelegate _next;
        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            if (e == null) return;
            if (e is UnauthorizedAccessException)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (e is Exception)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            LogServer.WriteErrorLog($"{context.Request.Path.Value}接口请求错误", e);
            context.Response.ContentType = "application/json";
            var res = ApiResponse.Error<string>(e.Message);
            await context.Response.WriteAsync(res.ToJson());
        }

    }
}
