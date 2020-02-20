using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class SysUserInfoServices : BaseServices<SysUserInfo>, ISysUserInfoServices
    {
        ISysUserInfoRepository _dal;
        IUserRoleServices _userRoleServices;
        IRoleRepository _roleRepository;
        public SysUserInfoServices(ISysUserInfoRepository dal, IUserRoleServices userRoleServices, IRoleRepository roleRepository)
        {
            this._dal = dal;
            this._userRoleServices = userRoleServices;
            this._roleRepository = roleRepository;
            base.BaseDal = dal;
        }

        public async Task<SysUserInfo> GetUserByLogin(string loginName, string pwd)
        {
            var userModel = (await Query(a => a.uLoginName == loginName && a.uLoginPWD == pwd)).FirstOrDefault() ;
            return userModel;
        }

        public async Task<List<Role>> GetUserRoleByUserId(long userId)
        {
            var userRoleList =await _userRoleServices.Query(c => c.UserId == userId);
            var roleIds = userRoleList.Select(s => s.RoleId);
            var roleList=await _roleRepository.Query(m => roleIds.Contains(m.Id));
            return roleList;
        }
    }
}
