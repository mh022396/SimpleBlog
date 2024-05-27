using webapi.Models.Domain;

namespace webapi.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAysnc(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
    }
}
