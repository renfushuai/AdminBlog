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
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // GET: api/User
        [HttpGet]
        public async Task<ApiResponseModel<PageModel<Permission>>> Get(int page = 1, string key = "")
        {
            PageModel<Permission> permissions = new PageModel<Permission>();
            int intPageSize = 50;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            permissions = await _permissionServices.QueryPage(a => a.IsDeleted != true && (a.Name != null && a.Name.Contains(key)), page, intPageSize, " Id desc ");


            #region 单独处理

            var apis = await _moduleServices.Query(d => d.IsDeleted == false);
            var permissionsView = permissions.data;

            var permissionAll = await _permissionServices.Query(d => d.IsDeleted != true);
            foreach (var item in permissionsView)
            {
                List<int> pidarr = new List<int>
                {
                    item.Pid
                };
                if (item.Pid > 0)
                {
                    pidarr.Add(0);
                }
                var parent = permissionAll.FirstOrDefault(d => d.Id == item.Pid);

                while (parent != null)
                {
                    pidarr.Add(parent.Id);
                    parent = permissionAll.FirstOrDefault(d => d.Id == parent.Pid);
                }


                item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();
                foreach (var pid in item.PidArr)
                {
                    var per = permissionAll.FirstOrDefault(d => d.Id == pid);
                    item.PnameArr.Add((per != null ? per.Name : "根节点") + "/");
                }

                item.MName = apis.FirstOrDefault(d => d.Id == item.Mid)?.LinkUrl;
            }

            permissions.data = permissionsView;

            #endregion


            return ApiResponse.Success(permissions);

        }
        /// <summary>
        /// 查询树形 Table
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
            var  permissions = permissionsList.Where(a => a.Pid == f).OrderBy(a => a.OrderSort).ToList();
            foreach (var item in permissions)
            {
                //List<int> pidarr = new List<int>
                //{
                //    item.Pid
                //};
                //if (item.Pid > 0)
                //{
                //    pidarr.Add(0);
                //}
                //var parent = permissionsList.FirstOrDefault(d => d.Id == item.Pid);

                //while (parent != null)
                //{
                //    pidarr.Add(parent.Id);
                //    parent = permissionsList.FirstOrDefault(d => d.Id == parent.Pid);
                //}


                ///item.PidArr = pidarr.OrderBy(d => d).Distinct().ToList();
                item.MName = apiList.FirstOrDefault(d => d.Id == item.Mid)?.LinkUrl;
                item.hasChildren = permissionsList.Where(d => d.Pid == item.Id).Any();
            }
            return ApiResponse.Success(permissions);
        }
        /// <summary>
        /// 获取路由树
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponseModel<NavigationBarDto>> GetNavigationBar(int uid)
        {
            var token = _httpContext.HttpContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
            var id=JwtHelper.SerializeJwt(token)?.Uid;
            if (id!=uid)
            {
                return ApiResponse.Error<NavigationBarDto>("参数和token不一样");
            }
            var roleIds = (await _userRoleServices.Query(m => m.IsDeleted == false && m.UserId == id)).Select(m=>m.RoleId).Distinct().ToList();
            if (!roleIds.Any())
            {
                return ApiResponse.Error<NavigationBarDto>("用户未分配角色");
            }
            var permissionIds = (await _roleModulePermissionServices.Query(d => d.IsDeleted == false && roleIds.Contains(d.RoleId))).Select(m => m.PermissionId).Distinct().ToList();
            var permissionList = (await _permissionServices.Query(m => permissionIds.Contains(m.Id) && m.IsButton == false && m.IsDeleted == false)).OrderBy(c => c.OrderSort).ToList();
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


        private void LoopNaviBarAppendChildren(List<NavigationBarDto> list,NavigationBarDto curentItem)
        {
            var childrenList=list.Where(m => m.pid == curentItem.id).ToList();
            curentItem.children.AddRange(childrenList);
            foreach (var item in childrenList)
            {
                LoopNaviBarAppendChildren(list, item);
            }
        }
    }
}
