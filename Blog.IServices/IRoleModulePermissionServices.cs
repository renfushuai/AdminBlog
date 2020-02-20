using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IServices.Base;
using Blog.Model.Model;

namespace Blog.IServices
{
    public interface IRoleModulePermissionServices : IBaseServices<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
