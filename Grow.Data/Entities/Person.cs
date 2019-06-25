using Grow.Data.Helpers.Attributes;

namespace Grow.Data.Entities
{
    public abstract class Person : BaseContestSubEntity
    {
        public string JobTitle { get; set; }

        public string Description { get; set; }

        [FileCategory(FileCategory.People)]
        public virtual File Image { get; set; }
        public int? ImageId { get; set; }

        public string Email { get; set; }
    }
}
