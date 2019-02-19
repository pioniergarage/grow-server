using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grow.Server.Model.Entities;
using Grow.Server.Model.Entities.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Grow.Server.Model.Utils;
using System.Threading;
using System.Diagnostics;

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
            // Adjust Contest navigation properties
            modelBuilder.Entity<Contest>()
                .HasOne(c => c.KickoffEvent)
                .WithMany();

            modelBuilder.Entity<Contest>()
                .HasOne(c => c.FinalEvent)
                .WithMany();

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Contest)
                .WithMany(c => c.Events);
            
            // Adjust many-to-many relationships
            modelBuilder.Entity<PartnerToContest>()
                .HasKey(t => new { t.PartnerId, t.ContestId });

            modelBuilder.Entity<MentorToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            modelBuilder.Entity<OrganizerToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            modelBuilder.Entity<JudgeToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });
            
        }


        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    entity.CurrentValues[nameof(BaseEntity.CreatedAt)] = now;
                }
                entity.CurrentValues[nameof(BaseEntity.UpdatedAt)] = now;
            }
        }
    }
}
