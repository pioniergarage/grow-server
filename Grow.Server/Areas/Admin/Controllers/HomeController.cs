using Grow.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly GrowDbContext _context;

        public HomeController(GrowDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Home
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
