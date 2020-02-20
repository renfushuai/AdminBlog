using System;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class PermissionServices:BaseServices<Permission>,IPermissionServices
    {
        IPermissionRepository _dal;
        public PermissionServices(IPermissionRepository dal)
        {
            this._dal = dal;
            BaseDal = dal;
        }
    }
}
