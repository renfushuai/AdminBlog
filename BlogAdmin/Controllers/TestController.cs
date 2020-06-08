using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAdmin.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [Route("Index")]
        [AllowAnonymous]
        public string Index(string str)
        {
            int stmp = 10 / str.ObjToInt();
            var bys = Convert.FromBase64String(str);
            var s = BitConverter.ToString(bys);
            return s;
        }
    }
}