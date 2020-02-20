using System;
using Blog.IRepository;
using Blog.IRepository.UnitOfWork;
using Blog.Model.Model;
using Blog.Repository.Base;

namespace Blog.Repository
{
    public class ModuleRepository : BaseRepository<Module>,IModuleRepository
    {
        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
