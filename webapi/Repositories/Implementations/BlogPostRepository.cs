using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models.Domain;
using webapi.Repositories.Interfaces;

namespace webapi.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext dbContext;

        public BlogPostRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAysnc(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbContext.BlogPosts.Include(b => b.Categories).ToListAsync();
        }
    }
}
