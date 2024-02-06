using Food.Api.Dtos.Categories;

namespace Food.Api.Dtos.Products
{
    public class ProductResponseDto : BaseDto
    {
        public string Name { get; set; }
        public CategoryResponseDto Category { get; set; }
        public string Image { get; set; }
    }
}
