﻿using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Model.ViewModels
{
    public class IndexViewModel
    {
        public Contest Contest { get; set; }
        public ICollection<Event> MainEvents { get; set; }
        public ICollection<Partner> Partners { get; set; }

        public DateTime ContestStart
        {
            get
            {
                return MainEvents.Count > 0
                    ? MainEvents.Min(e => e.Start)
                    : new DateTime(int.Parse(Contest.Year), 12, 31);
            }
        }

        public DateTime ContestEnd
        {
            get
            {
                return MainEvents.Count > 0
                    ? MainEvents.Max(e => e.End)
                    : new DateTime(int.Parse(Contest.Year), 12, 31);
            }
        }

        public DateTime RegistrationDeadline
        {
            get
            {
                return Contest.RegistrationDeadline.HasValue
                    ? Contest.RegistrationDeadline.Value
                    : ContestStart;
            }
        }
        
        public bool IsBeforeContest => ContestStart > DateTime.Now;

        public bool IsAfterContest => ContestEnd < DateTime.Now;
    }
}
