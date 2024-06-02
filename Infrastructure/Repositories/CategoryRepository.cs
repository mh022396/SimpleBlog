using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var currentCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (currentCategory != null)
            {
                dbContext.Entry(currentCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return currentCategory;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var currentCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (currentCategory != null)
            {
                dbContext.Remove(currentCategory);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
