using System;
using Blog.IRepository;
using Blog.IRepository.UnitOfWork;
using Blog.Model.Model;
using Blog.Repository.Base;

namespace Blog.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
