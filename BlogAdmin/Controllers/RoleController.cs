using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.HttpContextUser;
using Blog.IServices;
using Blog.Model;
using Blog.Model.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class RoleController : Controller
    {
        readonly IRoleServices _roleServices;
        readonly ICurrentUser _user;


        public RoleController(IRoleServices roleServices, ICurrentUser user)
        {
            _roleServices = roleServices;
            _user = user;
        }
        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<ApiResponseModel<PageModel<Role>>> Get(int page = 1, string key = "")
        {

            int intPageSize = 50;

            var data = await _roleServices.QueryPage(a => a.IsDeleted == false, page, intPageSize, " Id desc ");

            return ApiResponse.Success<PageModel<Role>>(data);

        }
    }
}
