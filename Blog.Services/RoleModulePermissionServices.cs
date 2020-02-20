using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class RoleModulePermissionServices : BaseServices<RoleModulePermission>, IRoleModulePermissionServices
    {
        IRoleModulePermissionRepository _dal;
        public RoleModulePermissionServices(IRoleModulePermissionRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        public async Task<List<RoleModulePermission>> RoleModuleMaps()
        {
            return await _dal.RoleModuleMaps();
        }
    }
}
