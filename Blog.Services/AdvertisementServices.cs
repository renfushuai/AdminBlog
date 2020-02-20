using System;
using Blog.IRepository;
using Blog.IServices;
using Blog.Model.Model;
using Blog.Services.Base;

namespace Blog.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        IAdvertisementRepository _dal;
        public AdvertisementServices(IAdvertisementRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}
