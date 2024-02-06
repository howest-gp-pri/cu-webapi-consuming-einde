using Food.Api.Dtos.Categories;
using Food.Api.Dtos.Products;
using Food.Core.Entities;
using Food.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Food.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.ListAllAsync();

            if (result.Success)
            {
                var productDtos = result.Data.Select(c => new ProductResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Category = new CategoryResponseDto
                    {
                        Id = c.Category.Id,
                        Name = c.Category.Name
                    },
                    Image = $"{Request.Scheme}://{Request.Host}/img/{c.Image}"
                });

                return Ok(productDtos);
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

            var result = await _productService.GetByIdAsync(id);

            if (result.Success)
            {
                var productDto = new ProductResponseDto
                {
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    Category = new CategoryResponseDto
                    {
                        Id = result.Data.Category.Id,
                        Name = result.Data.Category.Name
                    },
                    Image = $"{Request.Scheme}://{Request.Host}/img/{result.Data.Image}"
                };

                return Ok(productDto);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductRequestDto productDto)
        {
            if (await _categoryService.DoesCategoryIdExistsAsync(productDto.CategoryId))
            {
                return BadRequest($"Cannot add new product because category with id {productDto.CategoryId} does not exists");
            }

            var product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                Image = "food/default.jpg"
            };

            var resultProduct = await _productService.AddAsync(product);


            if (resultProduct.Success)
            {
                var resultCategory = await _categoryService.GetByIdAsync(product.CategoryId);

                if (resultCategory.Success)
                {
                    var dto = new ProductResponseDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = new CategoryResponseDto
                        {
                            Id = resultCategory.Data.Id,
                            Name = resultCategory.Data.Name
                        },
                        Image = $"{Request.Scheme}://{Request.Host}/img/{product.Image}"
                    };

                    return CreatedAtAction(nameof(Get), new { id = product.Id }, dto);
                }
                return BadRequest(resultCategory.Errors);
            }
            return BadRequest(resultProduct.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductRequestDto productDto)
        {
            if (await _productService.DoesProductIdExistsAsync(productDto.Id) == false)
            {
                return BadRequest($"No product with id '{productDto.Id}' found");
            }

            var existingProductResult = await _productService.GetByIdAsync(productDto.Id);

            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }

            var existingEntity = existingProductResult.Data;
            existingEntity.CategoryId = productDto.CategoryId;
            existingEntity.Name = productDto.Name;

            var result = await _productService.UpdateAsync(existingEntity);

            if (result.Success)
            {
                return Ok($"Product {existingEntity.Id} updated");
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _productService.DoesProductIdExistsAsync(id) == false)
            {
                return NotFound($"No product with an id of {id}");
            }
            var existingProductResult = await _productService.GetByIdAsync(id);

            if (existingProductResult.Success == false)
            {
                return BadRequest(existingProductResult.Errors);
            }

            var result = await _productService.DeleteAsync(existingProductResult.Data);

            return Ok($"Product {existingProductResult.Data.Id} deleted");
        }
    }
}
