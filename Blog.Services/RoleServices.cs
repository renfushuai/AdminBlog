using System;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class RoleServices : BaseServices<Role>, IRoleServices
    {
        IRoleRepository _dal;
        public RoleServices(IRoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
