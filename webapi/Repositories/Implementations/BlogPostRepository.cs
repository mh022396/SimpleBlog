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

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbContext.BlogPosts.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var currentBlogPost = await dbContext.BlogPosts.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id ==  blogPost.Id);

            if (currentBlogPost != null)
            {
                dbContext.Entry(currentBlogPost).CurrentValues.SetValues(blogPost);
                currentBlogPost.Categories = blogPost.Categories;
                await dbContext.SaveChangesAsync();
                currentBlogPost = blogPost;
            }

            return currentBlogPost;
        }
    }
}
