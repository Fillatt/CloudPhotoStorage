using Microsoft.EntityFrameworkCore;
using CloudPhotoStorage.DataBase.Models;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class ImageRepo
    {
        private readonly ApplicationContext _dbContext;

        public ImageRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Получить изображение по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Image> GetById(int id)
        {
            return await _dbContext.Images
                .FirstOrDefaultAsync(i => i.ImageId == id);
        }
        /// <summary>
        /// Получить все изображения пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetByUserId(int userId)
        {
            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }
        /// <summary>
        /// Получить изображения по категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetByCategoryId(int categoryId)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();
        }

        /// <summary>
        /// Поиск изображений по имени файла
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> SearchByName(string searchTerm)
        {
            return await _dbContext.Images
                .Where(i => i.FileName.Contains(searchTerm))
                .ToListAsync();
        }

        /// <summary>
        /// Получить все изображения
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetAll()
        {
            return await _dbContext.Images.ToListAsync();
        }

        /// <summary>
        /// Добавить новое изображение
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Add(Image image)
        {
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Изменить информацию об изображении
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Update(Image image)
        {
            _dbContext.Images.Update(image);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить изображение
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Delete(Image image)
        {
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить изображение по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(int id)
        {
            var image = await GetById(id);
            if (image != null)
            {
                await Delete(image);
            }
        }

        /// <summary>
        /// Проверить существование изображения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Exists(int id)
        {
            return await _dbContext.Images
                .AnyAsync(i => i.ImageId == id);
        }

        /// <summary>
        /// Получить количество изображений пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetCountByUser(int userId)
        {
            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .CountAsync();
        }

        /// <summary>
        /// Получить последние добавленные изображения
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetRecentImages(int count)
        {
            return await _dbContext.Images
                .OrderByDescending(i => i.UploadDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
