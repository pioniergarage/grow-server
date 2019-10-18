using Grow.Data;
using Grow.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Model.ViewModels
{
    public class DashboardViewModel
    {
        public string SelectedContestYear { get; }
        public string LatestContestYear { get; }
        public IDictionary<DateTime, int> EditData { get; }
        public int ParticipantCount { get; }
        public int VisitorCount { get; }
        public int EventCount { get; }
        public int PrizeSum { get; }

        public DashboardViewModel(GrowDbContext context, string selectedContestYear)
        {
            // Years
            SelectedContestYear = selectedContestYear;
            LatestContestYear = context.Contests.Max(c => c.Year);

            // Selected contest stats
            ParticipantCount = context
                .Teams
                .Where(t => t.Contest.Year == selectedContestYear)
                .Where(t => t.IsActive)
                .Select(t => t.Members)
                .Where(m => m != null)
                .Sum(m => m.Count);
            VisitorCount = context
                .EventResponses
                .Where(r => r.Event.Contest.Year == selectedContestYear)
                .Sum(r => r.ParticipantCount);
            EventCount = context
                .Events
                .Where(e => e.Contest.Year == selectedContestYear)
                .Where(e => e.IsActive)
                .Count();
            PrizeSum = context
                .Prizes
                .Where(p => p.Contest.Year == selectedContestYear)
                .Where(p => p.IsActive)
                .Sum(p => p.RewardValue);

            // Edit data
            EditData = GetEditStatistics(context);
        }

        private IDictionary<DateTime, int> GetEditStatistics(GrowDbContext context)
        {
            var editData = new SortedDictionary<DateTime, int>();
            var cutoffDate = DateTime.Now.AddDays(-30).Date;
            
            // fill with data
            var data = GetEditStatisticsForAllTables(context, cutoffDate)
                .SelectMany(enm => enm)
                .GroupBy(dt => dt)
                .Select(group => new KeyValuePair<DateTime, int>(group.Key, group.Count()));
            editData.AddRange(data);

            // fill holes in data
            for (var date = cutoffDate; date <= DateTime.Now; date += new TimeSpan(1, 0, 0, 0))
            {
                if (!editData.ContainsKey(date))
                    editData.Add(date, 0);
            }

            return editData;
        }

        private IEnumerable<IEnumerable<DateTime>> GetEditStatisticsForAllTables(GrowDbContext context, DateTime cutoffDate)
        {
            yield return GetEditStatisticsForTable(Splice(context.Contests, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Events, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Files, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Partners, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Organizers, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Judges, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Mentors, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Teams, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.Prizes, cutoffDate));
            yield return GetEditStatisticsForTable(Splice(context.CommonQuestion, cutoffDate));
        }

        private IQueryable<TEntity> Splice<TEntity>(DbSet<TEntity> query, DateTime cutoffDate) where TEntity : BaseTimestampedEntity
        {
            return query.Where(c => c.UpdatedAt >= cutoffDate);
        }
        
        private IEnumerable<DateTime> GetEditStatisticsForTable<TEntity>(IQueryable<TEntity> dataset) where TEntity : BaseTimestampedEntity
        {
            return dataset.Select(e => e.UpdatedAt.Date);
        }
    }
}
