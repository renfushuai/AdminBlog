using System;
using Blog.IRepository;
using Blog.IRepository.UnitOfWork;
using Blog.Model.Model;
using Blog.Repository.Base;

namespace Blog.Repository
{
    public class SysUserInfoRepository : BaseRepository<SysUserInfo>, ISysUserInfoRepository
    {
        public SysUserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
