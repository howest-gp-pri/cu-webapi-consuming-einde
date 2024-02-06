using Food.Core.Entities;
using Food.Core.Services.Models;

namespace Food.Core.Services.Interfaces
{
    public interface IProductService
    {
        IQueryable<Product> GetAll();
        Task<bool> DoesProductIdExistsAsync(Guid id);
        Task<ResultModel<IEnumerable<Product>>> ListAllAsync();
        Task<ResultModel<Product>> GetByIdAsync(Guid id);
        Task<ResultModel<Product>> UpdateAsync(Product entity);
        Task<ResultModel<Product>> AddAsync(Product entity);
        Task<ResultModel<Product>> DeleteAsync(Product entity);

        Task<ResultModel<IEnumerable<Product>>> GetByCategoryIdAsync(Guid id);
        Task<ResultModel<IEnumerable<Product>>> SearchAsync(string search);
    }
}
