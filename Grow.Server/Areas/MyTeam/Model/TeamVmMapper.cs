using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.MyTeam.Model.ViewModels;
using Grow.Server.Model;
using Grow.Server.Model.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Grow.Server.Areas.MyTeam.Model
{
    public class TeamVmMapper
    {
        private readonly GrowDbContext _dbContext;
        private readonly Lazy<StorageConnector> _storage;

        public TeamVmMapper(GrowDbContext dbContext, Lazy<StorageConnector> storage)
        {
            _dbContext = dbContext;
            _storage = storage;
        }

        public void UpdateTeam(Team oldTeam, TeamViewModel newViewModel)
        {
            if (oldTeam.Id != newViewModel.Id)
            {
                throw new ArgumentException("Can't update a different team");
            }
            
            oldTeam.ActiveSince = newViewModel.ActiveSince;
            oldTeam.Description = newViewModel.Description;
            oldTeam.Email = newViewModel.Email;
            oldTeam.FacebookUrl = newViewModel.FacebookUrl;
            oldTeam.InstagramUrl = newViewModel.InstagramUrl;
            oldTeam.Members = newViewModel.Members.Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
            oldTeam.Name = newViewModel.Name;
            oldTeam.TagLine = newViewModel.TagLine;
            oldTeam.WebsiteUrl = newViewModel.WebsiteUrl;

            UpdateTeamPhoto(oldTeam, newViewModel);
            UpdateLogoImage(oldTeam, newViewModel);

            _dbContext.SaveChanges();
        }

        private void UpdateTeamPhoto(Team oldTeam, TeamViewModel newViewModel)
        {
            if (newViewModel.NewTeamPhoto == null)
                return;

            using (var stream = newViewModel.NewTeamPhoto.OpenReadStream())
            {
                var extension = newViewModel.NewTeamPhoto.FileName.Split('.').Last();
                var fileName = string.Format("team-{0}.{1}", newViewModel.Id, extension);
                var oldFile = _dbContext.Teams.Include(t => t.TeamPhoto).Single(t => t.Id == newViewModel.Id).TeamPhoto;

                if (oldFile == null)
                {
                    var category = ViewHelpers.GetFileCategoryForProperty<Team>(t => t.TeamPhoto);
                    oldTeam.TeamPhoto = _storage.Value.Create(
                        category,
                        fileName,
                        stream
                    );
                }
                else
                {
                    oldTeam.TeamPhoto = _storage.Value.Replace(
                        oldFile,
                        fileName,
                        stream
                    );
                }
                oldTeam.TeamPhoto.Name = string.Format("{0}.{1}", newViewModel.Name, extension);
                newViewModel.TeamPhotoUrl = oldTeam.TeamPhoto.Url;
            }
        }

        private void UpdateLogoImage(Team oldTeam, TeamViewModel newViewModel)
        {
            if (newViewModel.NewLogoImage == null)
                return;

            using (var stream = newViewModel.NewLogoImage.OpenReadStream())
            {
                var extension = newViewModel.NewLogoImage.FileName.Split('.').Last();
                var fileName = string.Format("logo-{0}.{1}", newViewModel.Id, extension);
                var oldFile = _dbContext.Teams.Include(t => t.LogoImage).Single(t => t.Id == newViewModel.Id).LogoImage;

                if (oldFile == null)
                {
                    var category = ViewHelpers.GetFileCategoryForProperty<Team>(t => t.LogoImage);
                    oldTeam.LogoImage = _storage.Value.Create(
                        category,
                        fileName,
                        stream
                    );
                }
                else
                {
                    oldTeam.LogoImage = _storage.Value.Replace(
                        oldFile,
                        fileName,
                        stream
                    );
                }
                oldTeam.LogoImage.Name = string.Format("{0}.{1}", newViewModel.Name, extension);
                newViewModel.LogoImageUrl = oldTeam.LogoImage.Url;
            }
        }

        public TeamViewModel TeamToViewModel(Team team, TeamViewModel entityToFill = null)
        {
            var vm = entityToFill ?? new TeamViewModel();
            
            vm.Id = team.Id;
            vm.ActiveSince = team.ActiveSince;
            vm.Contest = team.Contest;
            vm.ContestId = team.ContestId;
            vm.Description = team.Description;
            vm.Email = team.Email;
            vm.FacebookUrl = team.FacebookUrl;
            vm.InstagramUrl = team.InstagramUrl;
            vm.Members = team.Members;
            vm.Name = team.Name;
            vm.TagLine = team.TagLine;
            vm.WebsiteUrl = team.WebsiteUrl;
            vm.TeamPhotoUrl = team.TeamPhoto?.Url;
            vm.LogoImageUrl = team.LogoImage?.Url;

            return vm;
        }
    }
}
