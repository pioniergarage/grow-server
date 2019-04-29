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
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Grow.Server.Model
{
    public class GrowDbContext : IdentityDbContext<Account, IdentityRole, string>
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

        public DbSet<Prize> Prizes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contest>()
                .HasIndex(c => c.Year);

            // Adjust Contest navigation properties
            builder.Entity<Contest>()
                .HasOne(c => c.KickoffEvent)
                .WithMany();

            builder.Entity<Contest>()
                .HasOne(c => c.FinalEvent)
                .WithMany();

            builder.Entity<Event>()
                .HasOne(e => e.Contest)
                .WithMany(c => c.Events);

            // Adjust many-to-many relationships
            builder.Entity<PartnerToContest>()
                .HasKey(t => new { t.PartnerId, t.ContestId });

            builder.Entity<MentorToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            builder.Entity<OrganizerToContest>()
                .HasKey(t => new { t.PersonId, t.ContestId });

            builder.Entity<JudgeToContest>()
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
            try
            {
                AddTimestamps();
                return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateException e)
            {
                Debug.WriteLine(e);
                throw e;
            }
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void ResetDatabase()
        {
            this.GetService<IMigrator>().Migrate(Migration.InitialDatabase);
            Database.Migrate();
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
