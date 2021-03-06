﻿using System;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class UserRoleServices : BaseServices<UserRole>, IUserRoleServices
    {
        IUserRoleRepository _dal;
        public UserRoleServices(IUserRoleRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
