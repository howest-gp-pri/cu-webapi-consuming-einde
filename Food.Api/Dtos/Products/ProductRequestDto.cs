using System.ComponentModel.DataAnnotations;

namespace Food.Api.Dtos.Products
{
    public class ProductRequestDto : BaseDto
    {
        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public Guid CategoryId { get; set; }
    }
}
