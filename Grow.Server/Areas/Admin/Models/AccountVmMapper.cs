using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Models
{
    public class AccountVmMapper
    {
        private readonly UserManager<Account> _userManager;
        private readonly GrowDbContext _dbContext;

        public AccountVmMapper(UserManager<Account> userManager, GrowDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task UpdateAccountAsync(Account oldAccount, AccountCreateViewModel newViewModel)
        {
            // Update password
            if (!string.IsNullOrEmpty(newViewModel.Password))
            {
                var acc = await _userManager.FindByIdAsync(oldAccount.Id).ConfigureAwait(false);
                var code = await _userManager.GeneratePasswordResetTokenAsync(acc).ConfigureAwait(false);
                await _userManager.ResetPasswordAsync(acc, code, newViewModel.Password).ConfigureAwait(false);
            }

            // Update roles
            var isAdmin = await _userManager.IsInRoleAsync(oldAccount, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            var isSuperAdmin = await _userManager.IsInRoleAsync(oldAccount, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);
            if (!isAdmin && newViewModel.IsAdmin)
                await _userManager.AddToRoleAsync(oldAccount, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            if (isAdmin && !newViewModel.IsAdmin)
                await _userManager.RemoveFromRoleAsync(oldAccount, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            if (!isSuperAdmin && newViewModel.IsSuperAdmin)
                await _userManager.AddToRoleAsync(oldAccount, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);
            if (isSuperAdmin && !newViewModel.IsSuperAdmin)
                await _userManager.RemoveFromRoleAsync(oldAccount, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);

            // Update normal fields
            if (oldAccount.Email != newViewModel.Email)
                oldAccount.EmailConfirmed = false;
            oldAccount.Email = newViewModel.Email;
            oldAccount.IsEnabled = newViewModel.IsActive;
            oldAccount.UserName = newViewModel.Name;
            await _userManager.UpdateAsync(oldAccount).ConfigureAwait(false);

            // TODO: update team
        }

        public Account ViewModelToAccount(AccountViewModel vm)
        {
            return new Account
            {
                Id = vm.CompleteId,
                Email = vm.Email,
                UserName = vm.Name,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                IsEnabled = vm.IsActive
            };
        }

        public async Task<AccountViewModel> AccountToViewModelAsync(Account account, AccountViewModel entityToFill = null)
        {
            var vm = entityToFill ?? new AccountViewModel();

            // Account uses 128-bit GUID string id => take last 32 bit (part of node id)
            vm.Id = int.Parse(account.Id.Substring(account.Id.Length - 7), System.Globalization.NumberStyles.HexNumber);
            vm.CompleteId = account.Id;
            vm.Email = account.Email;
            // TODO: Team + team ID from claims
            vm.Name = account.UserName;
            vm.IsActive = account.IsEnabled;
            vm.IsAdmin = await _userManager.IsInRoleAsync(account, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            vm.IsSuperAdmin = await _userManager.IsInRoleAsync(account, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);

            return vm;
        }

        public bool DoesAccountMatchShortId(Account account, int shortenedId)
        {
            var shortId = shortenedId.ToString("X7");
            return account.Id.EndsWith(shortId, StringComparison.OrdinalIgnoreCase);
        }

        public Account FindAccountByShortId(int id)
        {
            var userQuery = _dbContext.Users.Where(u => DoesAccountMatchShortId(u, id));
            if (userQuery.Count() > 1)
            {
                // TODO: what to do in case of collision?
            }
            return userQuery.FirstOrDefault();
        }
    }
}
