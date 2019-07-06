using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IdentityResult> CreateAccountAsync(AccountEditViewModel vm)
        {
            // Create entity
            Account account = ViewModelToAccount(vm);
            account.Id = Guid.NewGuid().ToString();
            account.IsEnabled = true;

            var result = await _userManager.CreateAsync(account, vm.Password).ConfigureAwait(false);
            if (!result.Succeeded)
                return result;

            // Set admin roles
            if (vm.IsAdmin)
                await _userManager.AddToRoleAsync(account, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            if (vm.IsSuperAdmin)
                await _userManager.AddToRoleAsync(account, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);

            // Add team claim
            if (vm.TeamId != null)
            {
                var team = _dbContext.Teams.Find(vm.TeamId);
                if (team != null)
                {
                    var claim = new Claim(Constants.TEAM_CLAIM_TYPE, team.Id.ToString());
                    await _userManager.AddClaimAsync(account, claim).ConfigureAwait(false);
                }
            }

            return result;
        }

        public async Task UpdateAccountAsync(Account oldAccount, AccountEditViewModel newViewModel)
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

            // Update team
            var oldClaim = await GetTeamClaimOrDefault(oldAccount).ConfigureAwait(false);
            if (oldClaim?.Value != newViewModel.TeamId?.ToString())
            {
                if (oldClaim != null)
                {
                    // remove old
                    await _userManager.RemoveClaimAsync(oldAccount, oldClaim).ConfigureAwait(false);
                }
                if (newViewModel.TeamId != null)
                {
                    // add new
                    var newClaim = new Claim(Constants.TEAM_CLAIM_TYPE, newViewModel.TeamId.ToString());
                    await _userManager.AddClaimAsync(oldAccount, newClaim).ConfigureAwait(false);
                }
            }
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

            // Normal properties
            vm.CompleteId = account.Id;
            vm.Email = account.Email;
            vm.Name = account.UserName;
            vm.IsActive = account.IsEnabled;

            // Roles
            vm.IsAdmin = await _userManager.IsInRoleAsync(account, Constants.ADMIN_ROLE_NAME).ConfigureAwait(false);
            vm.IsSuperAdmin = await _userManager.IsInRoleAsync(account, Constants.SUPERADMIN_ROLE_NAME).ConfigureAwait(false);
            
            // Team info from claims
            var teamId = await GetTeamIdOrDefault(account).ConfigureAwait(false);
            if (teamId != null)
            {
                var team = _dbContext.Teams.Find(teamId);
                if (team != null)
                {
                    vm.Team = team;
                    vm.TeamId = team.Id;
                }
            }

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

        private async Task<Claim> GetTeamClaimOrDefault(Account account)
        {
            var teamClaim = (await _userManager.GetClaimsAsync(account).ConfigureAwait(false)).FirstOrDefault(c => c.Type == Constants.TEAM_CLAIM_TYPE);
            return teamClaim;
        }

        private async Task<int?> GetTeamIdOrDefault(Account account)
        {
            var teamClaim = await GetTeamClaimOrDefault(account).ConfigureAwait(false);
            if (teamClaim != null && int.TryParse(teamClaim.Value, out int teamId))
            {
                return teamId;
            }
            return null;
        }
    }
}
