using Food.Api.Dtos.Categories;
using Food.Api.Dtos.Products;
using Food.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Food.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProductService _productService;

        public SearchController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string searchQuery)
        {
            var searchResult = await _productService.SearchAsync(searchQuery);

            if (searchResult.Success)
            {
                var searchResultsDto = searchResult.Data.Select(s => new ProductResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = new CategoryResponseDto
                    {
                        Id = s.Category.Id,
                        Name = s.Category.Name
                    }
                });

                return Ok(searchResultsDto);
            }

            return BadRequest(searchResult.Success);
        }
    }
}
