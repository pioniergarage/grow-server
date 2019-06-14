namespace Grow.Data.Entities
{
    public class Partner : ContestDependentEntity
    {
        public string Description { get; set; }

        public virtual File Image { get; set; }
        public int? ImageId { get; set; }
    }
}
