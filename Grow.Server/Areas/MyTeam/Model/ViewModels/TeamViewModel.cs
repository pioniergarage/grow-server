using Grow.Data.Entities;
using Grow.Data.Helpers.Attributes;
using Grow.Server.Model.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.MyTeam.Model.ViewModels
{
    public class TeamViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "A team name is required")]
        [DisplayName("Name of your team")]
        public string Name { get; set; }
        
        [MaxLength(120, ErrorMessage = "Your tag line must be shorter than 120 chars")]
        [DisplayName("Describe your team in two sentences")]
        public string TagLine { get; set; }

        [MaxLength(1000, ErrorMessage = "Your description must be shorter than 1000 chars")]
        [DisplayName("Full description of your team")]
        public string Description { get; set; }
        
        [DisplayName("New logo file")]
        [FormFileExtensions(Extensions = "png,jpg,jpeg,gif,svg", ErrorMessage = "Only files of types png, jpg, jpeg, gif, svg are allowed")]
        public IFormFile NewLogoImage { get; set; }
        
        [DisplayName("New photo of your team")]
        [FormFileExtensions(Extensions = "png,jpg,jpeg,gif,svg", ErrorMessage = "Only files of types png, jpg, jpeg, gif, svg are allowed")]
        public IFormFile NewTeamPhoto { get; set; }

        public string LogoImageUrl
        {
            get { return _logoImageUrl ?? "/img/icon/unknown.jpg"; }
            set { _logoImageUrl = value; }
        }
        private string _logoImageUrl;

        public string TeamPhotoUrl
        {
            get { return _teamPhotoUrl ?? "/img/icon/unknown.jpg"; }
            set { _teamPhotoUrl = value; }
        }
        private string _teamPhotoUrl;

        public virtual Contest Contest { get; set; }
        public int ContestId { get; set; }

        [MaxLength(100, ErrorMessage = "This field is too long. A month and year are sufficient")]
        [DisplayName("Since when are you active")]
        public string ActiveSince { get; set; }

        [Url(ErrorMessage = "This is not a valid URL")]
        [DisplayName("URL to your website")]
        public string WebsiteUrl { get; set; }

        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        [DisplayName("Public contact email")]
        public string Email { get; set; }

        [DisplayName("Facebook identifier")]
        public string FacebookUrl { get; set; }

        [DisplayName("Instagram identifier")]
        public string InstagramUrl { get; set; }
        
        public virtual ICollection<string> Members { get; set; }
    }
}
