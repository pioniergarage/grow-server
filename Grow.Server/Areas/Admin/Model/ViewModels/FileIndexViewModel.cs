using Grow.Data;
using Grow.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grow.Server.Areas.Admin.Model.ViewModels
{
    public class FileIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public string Extension { get; set; }

        public string Category { get; set; }

        public bool IsActive { get; set; }

        public ICollection<FileUsage> Uses { get; set; }

        public bool IsUsed => Uses?.Count > 0;

        public FileIndexViewModel()
        {
            Uses = new List<FileUsage>();
        }

        public FileIndexViewModel(File file) : this()
        {
            Id = file.Id;
            Name = file.Name;
            Url = file.Url;
            AltText = file.AltText;
            Extension = file.Extension;
            Category = file.Category;
            IsActive = file.IsActive;
        }

        public static IEnumerable<FileIndexViewModel> ConvertToViewModels(IEnumerable<File> files, GrowDbContext context)
        {
            foreach (var file in files)
            {
                var vm = new FileIndexViewModel(file);

                // crawl navigation properties for references to this file
                vm.Uses.AddRange(FindFileReferences<Event, int?>(e => e.ImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Event, int?>(e => e.SlidesId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Organizer, int?>(e => e.ImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Mentor, int?>(e => e.ImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Judge, int?>(e => e.ImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Partner, int?>(e => e.ImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Team, int?>(e => e.TeamPhotoId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<Team, int?>(e => e.LogoImageId, vm.Id, context));
                vm.Uses.AddRange(FindFileReferences<EventResponse, string>(e => (e as TeamResponse)?.FileUrl, vm.Url, context));

                yield return vm;
            }
        }

        private static IEnumerable<FileUsage> FindFileReferences<TEntity,TProperty>(Func<TEntity, TProperty> propertyExpression, TProperty referenceValue, GrowDbContext context) where TEntity : BaseEntity
        {
            return context
                .Set<TEntity>()
                .Where(e => referenceValue.Equals(propertyExpression(e)))
                .Select(e =>
                    new FileUsage()
                    {
                        ReferrerId = e.Id,
                        ReferrerName = (e is BaseNamedEntity ? (e as BaseNamedEntity).Name : e.Id.ToString()),
                        ReferrerType = typeof(TEntity).Name
                    }
                );
        }

        public class FileUsage
        {
            public string ReferrerType { get; set; }
            public int ReferrerId { get; set; }
            public string ReferrerName { get; set; }
        }
    }

    internal static class CollectionExtensions
    {
        public static void AddRange<TEntity>(this ICollection<TEntity> enumerable, IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                enumerable.Add(entity);
        }
    }
}
