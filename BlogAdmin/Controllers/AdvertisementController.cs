using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.Common.Helper;
using Blog.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize(Permissions.Name)]
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

            var s = await _advertisementServices.TestUnitOfWork();
            return Json(s);
        }
        [HttpGet]
        public async Task SendEmailTest()
        {
            var fromEmail = "renfushuai@xdf.cn";
            var pwd = "******";
            var toEmail = "951400721@qq.com";
            var subject = "测试主题";
            var body = "test";
            await SendEmail.SendMailAvailableAsync(fromEmail, pwd, toEmail, subject, body, "staff.neworiental.org");
        }
    }
}
