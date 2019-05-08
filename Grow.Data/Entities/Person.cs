namespace Grow.Data.Entities
{
    public abstract class Person : BaseEntity
    {
        public string Name { get; set; }

        public string JobTitle { get; set; }

        public string Description { get; set; }

        public virtual Image Image { get; set; }

        public string Email { get; set; }

        public virtual Contest Contest { get; set; }
    }
}
