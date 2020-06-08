using System;
using System.Threading.Tasks;
using Blog.IServices.Base;
using Blog.Model.Model;

namespace Blog.IServices
{
    public interface IAdvertisementServices : IBaseServices<Advertisement>
    {
        Task<string> TestUnitOfWork();
    }
}
