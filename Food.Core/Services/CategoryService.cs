using Food.Core.Data;
using Food.Core.Entities;
using Food.Core.Services.Interfaces;
using Food.Core.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Food.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IQueryable<Category> GetAll()
        {
            return dbContext.Categories.AsQueryable();
        }
        public async Task<ResultModel<IEnumerable<Category>>> ListAllAsync()
        {
            var categories = await dbContext.Categories.ToListAsync();
            var resultModel = new ResultModel<IEnumerable<Category>>
            {
                Data = categories
            };

            return resultModel;
        }

        public async Task<ResultModel<Category>> GetByIdAsync(Guid id)
        {
            var resultModel = new ResultModel<Category>();
            var category = await dbContext.Categories
                .FirstOrDefaultAsync(category => category.Id.Equals(id));


            if (category is null)
            {
                resultModel = new ResultModel<Category>();
                resultModel.Errors.Add($"Category does not exists");

                return resultModel;
            }

            resultModel = new ResultModel<Category> { Data = category };

            return resultModel;

        }

        public async Task<ResultModel<Category>> UpdateAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            if (await DoesCategoryIdExistsAsync(entity.Id) == false)
            {
                resultModel.Errors.Add($"There is no category with the ID {entity.Id}");

                return resultModel;
            }

            if (await DoesCategoryNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"There is already a category with the name {entity.Name}");

                return resultModel;
            }

            entity.LastEditedOn = DateTime.UtcNow;

            dbContext.Categories.Update(entity);
            await dbContext.SaveChangesAsync();

            resultModel = new ResultModel<Category> { Data = entity };
            return resultModel;
        }

        public async Task<ResultModel<Category>> AddAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            if (await DoesCategoryNameExistsAsync(entity))
            {
                resultModel.Errors.Add($"There is already a category with the name {entity.Name}");

                return resultModel;
            }

            DateTime now = DateTime.UtcNow;
            entity.CreatedOn = now;
            entity.LastEditedOn = now;

            dbContext.Categories.Add(entity);
            await dbContext.SaveChangesAsync();

            resultModel = new ResultModel<Category> { Data = entity };

            return resultModel;
        }

        public async Task<ResultModel<Category>> DeleteAsync(Category entity)
        {
            var resultModel = new ResultModel<Category>();

            if (await DoesCategoryHasProductsAsync(entity))
            {
                resultModel.Errors.Add($"Category {entity.Name} has products and cannot be deleted");

                return resultModel;
            }

            dbContext.Categories.Remove(entity);
            await dbContext.SaveChangesAsync();

            resultModel.Data = entity;

            return resultModel;
        }

        public async Task<ResultModel<IEnumerable<Category>>> GetByName(string name)
        {
            var entities = await dbContext.Categories
                .Where(entity => entity.Name.Contains(name))
                .ToListAsync();

            return new ResultModel<IEnumerable<Category>> { Data = entities };
        }

        public async Task<bool> DoesCategoryIdExistsAsync(Guid id)
        {
            bool categoryExists = await dbContext.Categories
                .AnyAsync(category => category.Id.Equals(id));

            return categoryExists;
        }

        private async Task<bool> DoesCategoryNameExistsAsync(Category entity)
        {
            bool categoryNameExists = await dbContext.Categories
                .Where(category => category.Id != entity.Id)
                .AnyAsync(category => category.Name == entity.Name);

            return categoryNameExists;
        }

        private async Task<bool> DoesCategoryHasProductsAsync(Category category)
        {
            bool categoryHasProducts = await dbContext.Products
                .AnyAsync(product => product.CategoryId.Equals(category.Id));

            return categoryHasProducts;
        }   
    }
}
