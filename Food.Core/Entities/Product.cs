using System.ComponentModel.DataAnnotations;

namespace Food.Core.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
        public string Image { get; set; }
    }
}
