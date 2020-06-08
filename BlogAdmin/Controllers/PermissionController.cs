using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.Dto;
using Blog.IServices;
using Blog.Model;
using Blog.Model.Model;
using BlogAdmin.AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BlogAdmin.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Permissions.Name)]
    public class PermissionController : Controller
    {
        readonly IPermissionServices _permissionServices;
        readonly IUserRoleServices _userRoleServices;
        readonly IRoleModulePermissionServices _roleModulePermissionServices;
        readonly IHttpContextAccessor _httpContext;
        readonly IModuleServices _moduleServices;
        public PermissionController(IHttpContextAccessor httpContext,IModuleServices moduleServices, IPermissionServices permissionServices,IUserRoleServices userRoleServices,IRoleModulePermissionServices roleModulePermissionServices)
        {
            _permissionServices = permissionServices;
            _userRoleServices = userRoleServices;
            _roleModulePermissionServices = roleModulePermissionServices;
            _httpContext = httpContext;
            _moduleServices = moduleServices;
        }
        #region 查询树形 列表
        /// <summary>
        /// 查询树形 列表
        /// </summary>
        /// <param name="f">父节点</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponseModel<List<Permission>>> GetTreeTable(int f = 0, string key = "")
        {
            var apiList = await _moduleServices.Query(d => d.IsDeleted == false);
            //所有菜单
            var permissionsList = await _permissionServices.Query(d => d.IsDeleted == false);
            //查找所有父级菜单
            var permissions = permissionsList.Where(a => a.Pid == f).OrderBy(a => a.OrderSort).ToList();
            foreach (var item in permissions)
            {
                item.MName = apiList.FirstOrDefault(d => d.Id == item.Mid)?.LinkUrl;
                item.hasChildren = permissionsList.Where(d => d.Pid == item.Id).Any();
            }
            return ApiResponse.Success(permissions);
        }
        #endregion

        #region 获取左侧导航和按钮
        /// <summary>
        /// 获取左侧导航和按钮
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponseModel<NavigationBarDto>> GetNavigationBar(int uid)
        {
            var token = _httpContext.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
            var id = JwtHelper.SerializeJwt(token)?.Uid;
            if (id != uid)
            {
                return ApiResponse.Error<NavigationBarDto>("参数和token不一样");
            }
            var roleIds = (await _userRoleServices.Query(m => m.IsDeleted == false && m.UserId == id)).Select(m => m.RoleId).Distinct().ToList();
            if (!roleIds.Any())
            {
                return ApiResponse.Error<NavigationBarDto>("用户未分配角色");
            }
            var permissionIds = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(m => m.PermissionId).Distinct().ToList();
            var permissionList = (await _permissionServices.Query(m => permissionIds.Contains(m.Id) && m.IsDeleted == false)).OrderBy(c => c.OrderSort).ToList();
            var permissionTree = permissionList.Select(child => new NavigationBarDto
            {

                id = child.Id,
                name = child.Name,
                pid = child.Pid,
                order = child.OrderSort,
                path = child.Code,
                iconCls = child.Icon,
                Func = child.Func,
                IsHide = child.IsHide.ObjToBool(),
                IsButton = child.IsButton.ObjToBool(),
                meta = new NavigationBarMetaDto
                {
                    requireAuth = true,
                    title = child.Name,
                    NoTabPage = child.IsHide.ObjToBool()
                }
            }).ToList();
            NavigationBarDto rootRoot = new NavigationBarDto
            {
                id = 0,
                pid = 0,
                order = 0,
                name = "根节点",
                path = "",
                iconCls = "",
                meta = new NavigationBarMetaDto(),

            };
            LoopNaviBarAppendChildren(permissionTree, rootRoot);
            return ApiResponse.Success(rootRoot);
        }
        private void LoopNaviBarAppendChildren(List<NavigationBarDto> list, NavigationBarDto curentItem)
        {
            var childrenList = list.Where(m => m.pid == curentItem.id).ToList();
            curentItem.children.AddRange(childrenList);
            foreach (var item in childrenList)
            {
                LoopNaviBarAppendChildren(list, item);
            }
        }
        #endregion



        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public async Task<ApiResponseModel<string>> Delete(int id)
        {
            if (id > 0)
            {
                var userDetail = await _permissionServices.QueryById(id);
                userDetail.IsDeleted = true;
                var b = await _permissionServices.Update(userDetail);
                if (b)
                {
                    return ApiResponse.Success(userDetail.Id.ObjToString());
                }
            }

            return ApiResponse.Error("Id不能为0");
        }
    }
}
