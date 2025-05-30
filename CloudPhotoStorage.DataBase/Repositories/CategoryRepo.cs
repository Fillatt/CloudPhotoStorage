using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class CategoryRepo
    {
        private readonly ApplicationContext _dbContext;

        public CategoryRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Category?> GetCategoryByIdAsync(Guid? id, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
        }
        public Task<Guid?> GetCategoryIdByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .Where(c => c.CategoryName == name)
                .Select(c => (Guid?)c.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Category> GetCategoryByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.ToListAsync(cancellationToken);
        }

        public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await GetCategoryByIdAsync(id, cancellationToken);
            if (category != null)
            {
                await DeleteCategoryAsync(category, cancellationToken);
            }
        }

        public async Task<bool> ExistscCategoryAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id, cancellationToken);
        }

        public async Task<bool> NameExistsCategoryAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryName == name, cancellationToken);
        }

        public async Task<int> GetImageCountAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .CountAsync(cancellationToken);
        }
    }
}
