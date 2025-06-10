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
    }
}
