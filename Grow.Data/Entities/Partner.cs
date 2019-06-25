using Grow.Data.Helpers.Attributes;

namespace Grow.Data.Entities
{
    public class Partner : BaseContestSubEntity
    {
        public string Description { get; set; }

        [FileCategory(FileCategory.Partners)]
        public virtual File Image { get; set; }
        public int? ImageId { get; set; }
    }
}
