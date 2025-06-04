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
        public Task<Category?> GetCategoryById(Guid? id, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
        }
        public async Task<Guid> GetCategoryIdByName(string name, CancellationToken cancellationToken)
        {
            var categoryId = await _dbContext.Categories
                .Where(c => c.CategoryName == name)
                .Select(c => c.CategoryId)
                .FirstOrDefaultAsync(cancellationToken);

            if (categoryId != Guid.Empty)
            {
                return categoryId;
            }

            var newCategory = new Category
            {
                CategoryName = name
            };

            await _dbContext.Categories.AddAsync(newCategory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newCategory.CategoryId;
        }

        public Task<Category?> GetCategoryByName(string name, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        public Task<List<Category>> GetAllCategories(CancellationToken cancellationToken)
        {
            return _dbContext.Categories.ToListAsync(cancellationToken);
        }

        public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Update(category);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Remove(category);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await GetCategoryById(id, cancellationToken);
            if (category != null)
            {
                await DeleteCategoryAsync(category, cancellationToken);
            }
        }

        public Task<bool> CategoryExistsById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id, cancellationToken);
        }

        public Task<bool> CategoryExistsByName(string name, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AnyAsync(c => c.CategoryName == name, cancellationToken);
        }

        public Task<int> GetImageCount(Guid categoryId, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .CountAsync(cancellationToken);
        }
    }
}
