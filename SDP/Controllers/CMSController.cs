using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class CMSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
