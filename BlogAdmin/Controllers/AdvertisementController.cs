using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Permissions.Name)]
    public class AdvertisementController : Controller
    {
        
        readonly IAdvertisementServices _advertisementServices;
        public AdvertisementController(IAdvertisementServices advertisementServices)
        {
            _advertisementServices = advertisementServices;
        }
        // GET: api/values
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            
            var id = await _advertisementServices.Add(new Blog.Model.Model.Advertisement
            {
                Createdate = DateTime.Now,
                ImgUrl = "12312/23232",
                Title="ceshi",
                Url="dddd",
                Remark="备注点点滴滴"
            });
           var model=   await _advertisementServices.QueryById(id);
            return Json(model);
        }

    }
}
