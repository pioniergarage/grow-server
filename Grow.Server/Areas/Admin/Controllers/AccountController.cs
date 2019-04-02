using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly GrowDbContext _context;

        public AccountController(GrowDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}