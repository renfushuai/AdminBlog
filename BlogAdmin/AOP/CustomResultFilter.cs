using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAdmin.AOP
{
    public class CustomResultFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                var errorMsg = "";

                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errorMsg += error.ErrorMessage + ",";
                    }
                }
                context.Result = new JsonResult(ApiResponse.Error<string>(errorMsg.Trim(',')));
            }
        }
    }
}
