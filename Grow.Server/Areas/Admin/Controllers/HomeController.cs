using Grow.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseAdminController
    {
        public HomeController(GrowDbContext context) : base(context)
        {
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }
    }
}
