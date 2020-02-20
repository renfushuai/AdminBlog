using System;
using Blog.IRepository;
using Blog.IRepository.UnitOfWork;
using Blog.Model.Model;
using Blog.Repository.Base;

namespace Blog.Repository
{
    public class UserRoleRepository: BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
