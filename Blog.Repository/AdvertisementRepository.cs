using System;
using Blog.IRepository;
using Blog.IRepository.UnitOfWork;
using Blog.Model.Model;
using Blog.Repository.Base;

namespace Blog.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
