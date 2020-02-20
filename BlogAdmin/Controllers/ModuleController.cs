using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.GlobalVar;
using Blog.Common.HttpContextUser;
using Blog.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class ModuleController : Controller
    {
        readonly IModuleServices _moduleServices;
        readonly ICurrentUser _user;


        public ModuleController(IModuleServices moduleServices, ICurrentUser user)
        {
            _moduleServices = moduleServices;
            _user = user;
        }
    }
}
