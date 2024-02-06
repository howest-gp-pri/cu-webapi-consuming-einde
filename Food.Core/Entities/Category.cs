using System.ComponentModel.DataAnnotations;

namespace Food.Core.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
