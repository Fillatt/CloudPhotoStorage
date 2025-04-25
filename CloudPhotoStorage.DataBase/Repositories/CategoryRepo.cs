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
        public async Task<Category> GetById(int id)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        /// <summary>
        /// Получить категорию по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Category> GetByName(string name)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name);
        }

        /// <summary>
        /// Получить все категории
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        /// <summary>
        /// Добавить новую категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Add(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Изменить категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Update(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить категорию по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(int id)
        {
            var category = await GetById(id);
            if (category != null)
            {
                await Delete(category);
            }
        }

        /// <summary>
        /// Проверить существование категории по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Existsc(int id)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id);
        }

        /// <summary>
        /// Проверить существование категории по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> NameExists(string name)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryName == name);
        }

        /// <summary>
        /// Получить количество изображений в категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<int> GetImageCount(int categoryId)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .CountAsync();
        }
    }
}
