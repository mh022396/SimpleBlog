using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAysnc(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<bool> DeleteAsync(Guid id);

    }
}
