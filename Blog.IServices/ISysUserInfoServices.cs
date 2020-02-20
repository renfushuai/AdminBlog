using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IServices.Base;
using Blog.Model.Model;

namespace Blog.IServices
{
    public interface ISysUserInfoServices : IBaseServices<SysUserInfo>
    {
        Task<SysUserInfo> GetUserByLogin(string loginName, string pwd);
        Task<List<Role>> GetUserRoleByUserId(long userId);
    }
}
