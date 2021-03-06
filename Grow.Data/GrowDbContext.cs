﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Grow.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Grow.Data
{
    public class GrowDbContext : IdentityDbContext<Account, IdentityRole, string>
    {
        public GrowDbContext(DbContextOptions<GrowDbContext> options)
            : base(options)
        { }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Organizer> Organizers { get; set; }

        public DbSet<Judge> Judges { get; set; }

        public DbSet<Mentor> Mentors { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Prize> Prizes { get; set; }

        public DbSet<EventResponse> EventResponses { get; set; }

        public DbSet<CommonQuestion> CommonQuestion { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contest>()
                .HasIndex(c => c.Year);

            builder.Entity<VisitorResponse>();
            builder.Entity<TeamResponse>();
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
                throw;
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
                .Where(x => x.Entity is BaseTimestampedEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    entity.CurrentValues[nameof(BaseTimestampedEntity.CreatedAt)] = now;
                }
                else
                {
                    // avoid resetting of CreatedAt timestamp
                    entity.Property(nameof(BaseTimestampedEntity.CreatedAt)).IsModified = false;
                }
                entity.CurrentValues[nameof(BaseTimestampedEntity.UpdatedAt)] = now;
            }
        }
    }
}
