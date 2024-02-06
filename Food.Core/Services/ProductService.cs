using Food.Core.Data;
using Food.Core.Entities;
using Food.Core.Services.Interfaces;
using Food.Core.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Food.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Product> GetAll()
        {
            return dbContext.Products.Include(p => p.Category);
        }

        public async Task<ResultModel<IEnumerable<Product>>> ListAllAsync()
        {
            var products = await GetAll().ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Product>>
            {
                Data = products
            };

            return resultModel;
        }

        public async Task<ResultModel<Product>> GetByIdAsync(Guid id)
        {
            var resultModel = new ResultModel<Product>();
            var product = await dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(product => product.Id.Equals(id));


            if (product is null)
            {
                resultModel = new ResultModel<Product>();
                resultModel.Errors.Add($"Product does not exists");

                return resultModel;
            }

            resultModel = new ResultModel<Product> { Data = product };

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Product>>> GetByCategoryIdAsync(Guid id)
        {
            var products = await GetAll().Where(p => p.CategoryId.Equals(id)).ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Product>>
            {
                Data = products
            };

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Product>>> SearchAsync(string search)
        {
            var products = await GetAll()
                .Where(p => p.Name.Contains(search.Trim()) || p.Category.Name.Contains(search.Trim()))
                .ToListAsync();

            var resultModel = new ResultModel<IEnumerable<Product>>
            {
                Data = products
            };

            return resultModel;
        }

        public async Task<ResultModel<Product>> UpdateAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            if (await DoesProductIdExistsAsync(entity.Id) == false)
            {
                resultModel.Errors.Add($"There is no product with the ID {entity.Id}");

                return resultModel;
            }

            if (await DoesProductNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"There is already a product with the name {entity.Name}");

                return resultModel;
            }

            entity.LastEditedOn = DateTime.UtcNow;

            dbContext.Products.Update(entity);
            await dbContext.SaveChangesAsync();

            resultModel = new ResultModel<Product> { Data = entity };
            return resultModel;
        }

        public async Task<ResultModel<Product>> AddAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            if (await DoesProductNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"There is already a product with the name {entity.Name}");

                return resultModel;
            }

            DateTime now = DateTime.UtcNow;
            entity.CreatedOn = now;
            entity.LastEditedOn = now;

            dbContext.Products.Add(entity);
            await dbContext.SaveChangesAsync();

            resultModel = new ResultModel<Product> { Data = entity };

            return resultModel;
        }

        public async Task<ResultModel<Product>> DeleteAsync(Product entity)
        {
            var resultModel = new ResultModel<Product>();

            dbContext.Products.Remove(entity);
            await dbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }

        public async Task<bool> DoesProductIdExistsAsync(Guid id)
        {
            bool productExists = await dbContext.Products
                .AnyAsync(product => product.Id.Equals(id));

            return productExists;
        }

        private async Task<bool> DoesProductNameExistsAsync(Product entity)
        {
            bool productNameExists = await dbContext.Products
                .Where(product => product.Id != entity.Id)
                .AnyAsync(product => product.Name == entity.Name);

            return productNameExists;
        }
    }
}
