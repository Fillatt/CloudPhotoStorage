using System.Threading;
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

        /// <summary>
        /// Получить категорию по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
        }

        /// <summary>
        /// Получить категорию по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Category> GetByName(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить новую категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Add(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Изменить категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Update(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Delete(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить категорию по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var category = await GetById(id, cancellationToken);
            if (category != null)
            {
                await Delete(category, cancellationToken);
            }
        }

        /// <summary>
        /// Проверить существование категории по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Existsc(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id, cancellationToken);
        }

        /// <summary>
        /// Проверить существование категории по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> NameExists(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryName == name, cancellationToken);
        }

        /// <summary>
        /// Получить количество изображений в категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<int> GetImageCount(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .CountAsync(cancellationToken);
        }
    }
}
