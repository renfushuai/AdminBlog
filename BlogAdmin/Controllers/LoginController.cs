using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Common.Helper;
using Blog.Common.LogsMethod;
using Blog.IServices;
using Blog.Model;
using Blog.Model.Dto;
using BlogAdmin.AuthHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        readonly PermissionRequirement _requirement;
        readonly ISysUserInfoServices _sysUserInfoServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleServices _roleServices;
        readonly IRoleModulePermissionServices _roleModulePermissionServices;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        /// <param name="requirement"></param>
        /// <param name="roleModulePermissionServices"></param>
        public LoginController(ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices, PermissionRequirement requirement, IRoleModulePermissionServices roleModulePermissionServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
            _requirement = requirement;
            _roleModulePermissionServices = roleModulePermissionServices;
        }
        [HttpGet]
        [Route("GetJwtStr")]
        [AllowAnonymous]
        public async Task<ApiResponseModel<JwtTokenDto>> GetJwtStr(string name, string pass)
        {
            string jwtStr = string.Empty;
            LogServer.WriteLog("用户登录："+name);
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return ApiResponse.Error<JwtTokenDto>("用户名或密码不能为空");
            }
            pass = MD5Helper.MD5Encrypt32(pass);
            var userInfo =await _sysUserInfoServices.GetUserByLogin(name, pass);
            if (userInfo == null)
            {
                return ApiResponse.Error<JwtTokenDto>("用户名不存在");
            }
            var roleList = await _sysUserInfoServices.GetUserRoleByUserId(userInfo.uID);
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, userInfo.uID.ObjToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };

            claims.AddRange(roleList.Select(s => new Claim(ClaimTypes.Role, s.Name)));



            var data = await _roleModulePermissionServices.RoleModuleMaps();
           var list= data.OrderBy(m=>m.Id).Select(m => new PermissionItem
            {
                Url = m.Module?.LinkUrl,
                Role = m.Role?.Name,
            }).ToList();
            _requirement.Permissions = list;
            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            var token=JwtHelper.BuildJwtToken(claims, _requirement);
                return ApiResponse.Success(token);
        }
    }
}
