using Grow.Server.Model;
using Grow.Server.Model.Entities;
using Grow.Server.Model.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Grow.Server.Controllers
{
    public class AccountController : BasePublicController
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, SignInManager<Account> signInManager, UserManager<Account> userManager) 
            : base(dbContext, appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    throw new Exception("Invalid login attempt");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}