namespace Grow.Data.Entities
{
    public abstract class Person : ContestDependentEntity
    {
        public string JobTitle { get; set; }

        public string Description { get; set; }

        public virtual File Image { get; set; }
        public int? ImageId { get; set; }

        public string Email { get; set; }
    }
}
