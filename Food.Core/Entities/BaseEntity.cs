using System.ComponentModel.DataAnnotations;

namespace Food.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastEditedOn { get; set; }
    }
}
