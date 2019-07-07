﻿using Grow.Server.Model;
using Grow.Data.Entities;
using Grow.Server.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Grow.Data;
using Grow.Server.Model.Helpers;

namespace Grow.Server.Controllers
{
    [Authorize]
    public class AccountController : BaseBackendController
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(GrowDbContext dbContext, IOptions<AppSettings> appSettings, SignInManager<Account> signInManager, UserManager<Account> userManager, ILogger logger)
            : base(dbContext, appSettings, logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = null)
        {
            var model = new LoginViewModel()
            {
                ReturnUrl = WebUtility.UrlDecode(ReturnUrl)
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("LoginFailed", "The email or password was incorrect");
                }
                else if (!user.IsEnabled)
                {
                    ModelState.AddModelError("LoginFailed", "The account has been disabled");
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        var returnUrl = model.ReturnUrl ?? "/Account/Index";
                        return Redirect(returnUrl);
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("LoginFailed", "The account has been locked");
                    }
                    else
                    {
                        ModelState.AddModelError("LoginFailed", "The user name or password was incorrect");
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user).ConfigureAwait(false);

            return View("ChangePasswordConfirmation");
        }

        public async Task<IActionResult> Logout(string ReturnUrl = null)
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);

            if (ReturnUrl != null)
            {
                return Redirect(WebUtility.UrlDecode(ReturnUrl));
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}