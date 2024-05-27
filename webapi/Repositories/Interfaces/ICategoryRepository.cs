using webapi.Models.Domain;

namespace webapi.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetById(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}
