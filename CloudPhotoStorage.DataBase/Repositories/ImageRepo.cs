using Microsoft.EntityFrameworkCore;
using CloudPhotoStorage.DataBase.Models;
using System.Threading;

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
        public async Task<Image> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .FirstOrDefaultAsync(i => i.ImageId == id, cancellationToken);
        }

        /// <summary>
        /// Получить все изображения пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        /// <summary>
        /// Получить изображения по категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Поиск изображений по имени файла
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> SearchByName(string searchTerm, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.ImageName.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получить все изображения
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Images.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить новое изображение
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Add(Image image, CancellationToken cancellationToken)
        {
            await _dbContext.Images.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Изменить информацию об изображении
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Update(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Update(image);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить изображение
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task Delete(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить изображение по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var image = await GetById(id, cancellationToken);
            if (image != null)
            {
                await Delete(image, cancellationToken);
            }
        }

        /// <summary>
        /// Проверить существование изображения
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .AnyAsync(i => i.ImageId == id, cancellationToken);
        }

        /// <summary>
        /// Получить количество изображений пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetCountByUser(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .CountAsync(cancellationToken);
        }

        /// <summary>
        /// Получить последние добавленные изображения
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Image>> GetRecentImages(int count, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .OrderByDescending(i => i.UploadDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
