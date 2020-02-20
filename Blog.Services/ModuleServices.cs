using System;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class ModuleServices : BaseServices<Module>, IModuleServices
    {
        IModuleRepository _dal;
        public ModuleServices(IModuleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
