using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.IServices;
using Blog.Model;
using Blog.Model.Model;
using BlogAdmin.AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    /// <summary>
    /// 用户Api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Authorize(Permissions.Name)]
    public class UserController : Controller
    {
        readonly ISysUserInfoServices _sysUserInfoServices;
        public UserController(ISysUserInfoServices sysUserInfoServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
        }
        [HttpGet]
        public async Task<ApiResponseModel<PageModel<SysUserInfo>>> Get(int page = 1, string key = "")
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }
            int intPageSize = 50;


            var data = await _sysUserInfoServices.QueryPage(a => a.tdIsDelete != true && a.uStatus >= 0 && ((a.uLoginName != null && a.uLoginName.Contains(key)) || (a.uRealName != null && a.uRealName.Contains(key))), page, intPageSize, " uID desc ");

            return ApiResponse.Success(data);
           

        }
        /// <summary>
        /// 获取用户详情根据token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponseModel<SysUserInfo>> GetInfoByToken(string token)
        {
            if (token.IsNullOrEmpty())
            {
                return ApiResponse.Error<SysUserInfo>("token 不能为空");
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel == null || tokenModel.Uid == 0)
            {
                return ApiResponse.Error<SysUserInfo>("token 无效");
            }
            var userinfo = await _sysUserInfoServices.QueryById(tokenModel.Uid);
            if (userinfo==null)
            {
                return ApiResponse.Error<SysUserInfo>("找不到用户");
            }
            return ApiResponse.Success(userinfo);
        }
    }
}
