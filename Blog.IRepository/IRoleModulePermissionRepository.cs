using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IRepository.Base;
using Blog.Model.Model;

namespace Blog.IRepository
{
    public interface IRoleModulePermissionRepository:IBaseRepository<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
