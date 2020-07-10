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
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
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

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
