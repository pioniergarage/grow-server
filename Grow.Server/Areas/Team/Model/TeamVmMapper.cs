using Grow.Data;
using Grow.Data.Entities;
using Grow.Server.Areas.Team.Model.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Team.Model
{
    public class TeamVmMapper
    {
        private readonly GrowDbContext _dbContext;

        public TeamVmMapper(GrowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateTeam(Data.Entities.Team oldTeam, TeamViewModel newViewModel)
        {
            if (oldTeam.Id != newViewModel.Id)
            {
                throw new ArgumentException("Can't update a different team");
            }
            
            oldTeam.ActiveSince = newViewModel.ActiveSince;
            oldTeam.ContestId = newViewModel.ContestId;
            oldTeam.Description = newViewModel.Description;
            oldTeam.Email = newViewModel.Email;
            oldTeam.FacebookUrl = newViewModel.FacebookUrl;
            oldTeam.InstagramUrl = newViewModel.InstagramUrl;
            oldTeam.LogoImageId = newViewModel.LogoImageId;
            oldTeam.Members = newViewModel.Members.Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
            oldTeam.Name = newViewModel.Name;
            oldTeam.TagLine = newViewModel.TagLine;
            oldTeam.TeamPhotoId = newViewModel.TeamPhotoId;
            oldTeam.WebsiteUrl = newViewModel.WebsiteUrl;

            _dbContext.SaveChanges();
        }

        public TeamViewModel TeamToViewModel(Data.Entities.Team team, TeamViewModel entityToFill = null)
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
            vm.LogoImage = team.LogoImage;
            vm.LogoImageId = team.LogoImageId;
            vm.Members = team.Members;
            vm.Name = team.Name;
            vm.TagLine = team.TagLine;
            vm.TeamPhoto = team.TeamPhoto;
            vm.TeamPhotoId = team.TeamPhotoId;
            vm.WebsiteUrl = team.WebsiteUrl;

            return vm;
        }
    }
}
