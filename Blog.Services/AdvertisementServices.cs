using System;
using System.Threading.Tasks;
using Blog.Common;
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
        [UnitOfWork]
        public async Task<string> TestUnitOfWork()
        {
            var model = await _dal.QueryById(1);
            model.Title = "任富帅";
            var modelInfo = await _dal.QueryById(7);
            modelInfo.ImgUrl = "bbbb";
            var insertId = await _dal.Add(modelInfo);
            //  var del = await _dal.Delete(modelInfo);
            var s = 10 / model.Id;
            var up = await _dal.Update(model);
            return "ok";
        }
    }
}
