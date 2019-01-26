using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model.Entities;
using Grow.Server.Model.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;

namespace Grow.Server.Model
{
    public class GrowDbContext : DbContext
    {
        public GrowDbContext(DbContextOptions<GrowDbContext> options)
            : base(options)
        { }


        public DbSet<Contest> Contests { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.KickoffEvent);

            modelBuilder.Entity<Contest>()
                .HasOne(c => c.FinalEvent);
            
            modelBuilder.Entity<PartnerToContest>()
                .HasKey(t => new { t.PartnerId, t.ContestId });

            modelBuilder.Entity<MentorToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            modelBuilder.Entity<OrganizerToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            modelBuilder.Entity<JudgeToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });
        }
    }
}
