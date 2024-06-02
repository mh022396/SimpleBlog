using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
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
            var currentBlogPost = await dbContext.BlogPosts.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == blogPost.Id);

            if (currentBlogPost != null)
            {
                dbContext.Entry(currentBlogPost).CurrentValues.SetValues(blogPost);
                currentBlogPost.Categories = blogPost.Categories;
                await dbContext.SaveChangesAsync();
                currentBlogPost = blogPost;
            }

            return currentBlogPost;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var toDelete = await dbContext.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
            if (toDelete != null)
            {
                dbContext.BlogPosts.Remove(toDelete);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
