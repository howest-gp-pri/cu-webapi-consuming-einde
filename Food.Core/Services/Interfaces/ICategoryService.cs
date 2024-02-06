using Food.Core.Entities;
using Food.Core.Services.Models;

namespace Food.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAll();
        Task<ResultModel<IEnumerable<Category>>> ListAllAsync();
        Task<ResultModel<Category>> GetByIdAsync(Guid id);
        Task<bool> DoesCategoryIdExistsAsync(Guid id);
        Task<ResultModel<IEnumerable<Category>>> GetByName(string name);
        Task<ResultModel<Category>> UpdateAsync(Category entity);
        Task<ResultModel<Category>> AddAsync(Category entity);
        Task<ResultModel<Category>> DeleteAsync(Category entity);
    }
}
