using Food.Api.Dtos.Categories;
using Food.Api.Dtos.Products;
using Food.Core.Entities;
using Food.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        protected readonly ICategoryService _categoryService;
        protected readonly IProductService _productService;

        public CategoriesController(
            ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.ListAllAsync();

            if (result.Success)
            {
                var categoryDtos = result.Data.Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                return Ok(categoryDtos);
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (await _categoryService.DoesCategoryIdExistsAsync(id) == false)
            {
                return NotFound();
            }
            var result = await _categoryService.GetByIdAsync(id);

            if (result.Success)
            {
                var categoryDto = new CategoryResponseDto
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name
                };

                return Ok(categoryDto);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CategoryRequestDto categoryDto)
        {
            Category category = new Category
            {
                Name = categoryDto.Name
            };

            var result = await _categoryService.AddAsync(category);

            if (result.Success)
            {
                var dto = new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return CreatedAtAction(nameof(Get), new { id = category.Id }, dto);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(CategoryRequestDto categoryDto)
        {
            if (await _categoryService.DoesCategoryIdExistsAsync(categoryDto.Id) == false)
            {
                return BadRequest($"No category with id '{categoryDto.Id}' found");
            }

            var existingCategoryResult = await _categoryService.GetByIdAsync(categoryDto.Id);

            if (existingCategoryResult.Success == false)
            {
                return BadRequest(existingCategoryResult.Errors);
            }

            var existingEntity = existingCategoryResult.Data;
            existingEntity.Name = categoryDto.Name;

            var result = await _categoryService.UpdateAsync(existingEntity);

            if (result.Success)
            {
                return Ok($"Category {existingEntity.Id} updated");
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _categoryService.DoesCategoryIdExistsAsync(id) == false)
            {
                return NotFound($"No category with an id of {id}");
            }

            var existingCategoryResult = await _categoryService.GetByIdAsync(id);

            if (existingCategoryResult.Success == false)
            {
                return BadRequest(existingCategoryResult.Errors);
            }

            var result = await _categoryService.DeleteAsync(existingCategoryResult.Data);

            if (result.Success)
            {
                return Ok($"Category {existingCategoryResult.Data.Id} deleted");
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsFromCategory(Guid id)
        {
            if (await _categoryService.DoesCategoryIdExistsAsync(id) == false)
            {
                return NotFound($"No category with an id of {id}");
            }

            var productsResult = await _productService.GetByCategoryIdAsync(id);

            if (productsResult.Success)
            {
                var productsDto = productsResult.Data.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Image = $"{Request.Scheme}://{Request.Host}/img/{p.Image}",
                    Category = new CategoryResponseDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                }).ToList();

                return Ok(productsDto);
            }

            return BadRequest(productsResult.Errors);
        }
    }
}
