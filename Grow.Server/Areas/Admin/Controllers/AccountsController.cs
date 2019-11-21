using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model;
using Grow.Data.Entities;
using Microsoft.Extensions.Options;
using Grow.Data;
using Microsoft.AspNetCore.Identity;
using Grow.Server.Areas.Admin.Model.ViewModels;
using System;
using System.Collections.Generic;
using Grow.Server.Model.Helpers;
using Grow.Server.Model.ViewModels;

namespace Grow.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : BaseAdminController
    {
        private readonly UserManager<Account> _userManager;
        private readonly AccountVmMapper _mapper;

        public AccountsController(UserManager<Account> userManager, GrowDbContext dbContext, IOptions<AppSettings> appSettings, ILogger logger) : base(dbContext, appSettings, logger)
        {
            _userManager = userManager;
            _mapper = new AccountVmMapper(userManager, DbContext);
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _userManager.Users.ToListAsync().ConfigureAwait(false);
            var accountVms = new List<AccountIndexViewModel>();
            foreach (var account in accounts)
            {
                var vm = (AccountIndexViewModel) await _mapper.AccountToViewModelAsync(account, new AccountIndexViewModel()).ConfigureAwait(false);
                vm.CanEdit = await IsCurrentUserAllowedToEditAsync(account).ConfigureAwait(false);
                accountVms.Add(vm);
            }

            return View(accountVms);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = _mapper.FindAccountByShortId(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            var vm = await _mapper.AccountToViewModelAsync(account).ConfigureAwait(false);
            return View(vm);
        }

        public IActionResult BulkCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkCreateConfirm(AccountBulkCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(BulkCreate), vm);
            }

            try
            {
                vm.ParseInput();
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(AccountBulkCreateViewModel.Input), "Parsing error: \r\n" + e.Message);
                return View(nameof(BulkCreate), vm);
            }
            
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreateResult(AccountBulkCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(BulkCreate), vm);
            }

            // Parse input
            try
            {
                vm.ParseInput();
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(AccountBulkCreateViewModel.Input), "Parsing error: \r\n" + e.Message);
                return View(nameof(BulkCreate), vm);
            }

            try
            {
                // Create teams
                vm.CreateTeams(DbContext, SelectedContestId);

                // Create accounts
                await vm.CreateAccounts(DbContext, _mapper).ConfigureAwait(false);
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(nameof(AccountBulkCreateViewModel.Input), "Error creating entities: \r\n" + e.Message);
                return View(vm);
            }

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountEditViewModel vm)
        {
            RemoveNavigationPropertiesFromModelState<AccountEditViewModel>();
            if (ModelState.IsValid)
            {
                ViewHelpers.RemoveAllNavigationProperties(vm);

                if (string.IsNullOrEmpty(vm.Password))
                {
                    ModelState.AddModelError(nameof(AccountEditViewModel.Password), "A Password must be set");
                    return View(vm);
                }

                if (!await IsCurrentUserSuperAdminAsync().ConfigureAwait(false))
                {
                    vm.IsAdmin = false;
                    vm.IsSuperAdmin = false;
                }

                var result = await _mapper.CreateAccountAsync(vm).ConfigureAwait(false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, result.Errors.First().Description);
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = _mapper.FindAccountByShortId(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            var vm = await _mapper.AccountToViewModelAsync(account, new AccountEditViewModel()).ConfigureAwait(false);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AccountEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            RemoveNavigationPropertiesFromModelState<AccountEditViewModel>();
            if (ModelState.IsValid)
            {
                ViewHelpers.RemoveAllNavigationProperties(vm);

                // account exists?
                var oldAccount = _mapper.FindAccountByShortId(vm.Id);
                if (oldAccount == null)
                {
                    return NotFound();
                }

                // access control
                if (!await IsCurrentUserAllowedToEditAsync(oldAccount).ConfigureAwait(false))
                {
                    return Unauthorized();
                }
                if (!await IsCurrentUserSuperAdminAsync().ConfigureAwait(false))
                {
                    vm.IsAdmin = false;
                    vm.IsSuperAdmin = false;
                }

                // update
                try
                {
                    await _mapper.UpdateAccountAsync(oldAccount, vm).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Could not update account: " + ex.Message);
                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = _mapper.FindAccountByShortId(id.Value);
            if (account == null)
            {
                return NotFound();
            }

            var vm = await _mapper.AccountToViewModelAsync(account).ConfigureAwait(false);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // not deleting self?
            var currentUser = await GetCurrentUserAsync().ConfigureAwait(false);
            if (_mapper.DoesAccountMatchShortId(currentUser, id))
                return BadRequest();

            // can delete other?
            var account = _mapper.FindAccountByShortId(id);
            if (account == null)
            {
                return NotFound();
            }
            if (!await IsCurrentUserAllowedToEditAsync(account).ConfigureAwait(false))
                return Unauthorized();

            // delete
            await _userManager.DeleteAsync(account).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Toggle(int id, bool value)
        {
            var account = _mapper.FindAccountByShortId(id);
            if (account == null)
            {
                return NotFound();
            }

            if (!await IsCurrentUserAllowedToEditAsync(account).ConfigureAwait(false))
                return Unauthorized();

            account.IsEnabled = value;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private Task<Account> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        private async Task<bool> IsCurrentUserSuperAdminAsync()
        {
            var self = await GetCurrentUserAsync().ConfigureAwait(false);
            return await _userManager.IsInRoleAsync(self, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);
        }

        private async Task<bool> IsCurrentUserAllowedToEditAsync(Account other)
        {
            bool selfIsSuperAdmin = await IsCurrentUserSuperAdminAsync().ConfigureAwait(false);

            bool otherIsAdmin = await _userManager.IsInRoleAsync(other, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false)
                || await _userManager.IsInRoleAsync(other, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);

            // cant edit Admins or Superadmins if you're not a Superadmin
            return selfIsSuperAdmin || !otherIsAdmin;
        }
    }
}
