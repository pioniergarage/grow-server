using Grow.Data.Helpers.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grow.Data.Entities
{
    public class Event : BaseContestSubEntity
    {
        /* Value properties */

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Url]
        public string ExternalEventUrl { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public EventVisibility Visibility { get; set; }

        public EventType Type { get; set; }

        public bool HasTimesSet { get; set; }

        public bool IsMandatory { get; set; }

        public bool CanVisitorsRegister { get; set; }

        public bool CanTeamsRegister { get; set; }

        /* Navigation Properties */

        public TeamRegistrationOptions TeamRegistrationOptions { get; set; }

        [FileCategory(FileCategory.Events)]
        public virtual File Image { get; set; }
        public int? ImageId { get; set; }

        [FileCategory(FileCategory.Slides)]
        public virtual File Slides { get; set; }
        public int? SlidesId { get; set; }

        public virtual Partner HeldBy { get; set; }
        public int? HeldById { get; set; }

        public virtual ICollection<EventResponse> Responses { get; set; }


        public enum EventVisibility
        {
            Public,
            ForAllTeams,
            ForActiveTeams
        }

        public enum EventType
        {
            Other,
            MainEvent,
            Workshop,
            Mentoring
        }
    }

    [Owned]
    public class TeamRegistrationOptions
    {
        public DateTime? From { get; set; }

        public DateTime? Until { get; set; }

        public bool AcceptFileUploads { get; set; }

        public string AllowedFileExtensionsString { get; set; }

        [NotMapped]
        public ICollection<string> AllowedFileExtensions
        {
            get => AllowedFileExtensionsString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            set => AllowedFileExtensionsString = string.Join(',', value ?? new string[0]);
        }
    }
}
