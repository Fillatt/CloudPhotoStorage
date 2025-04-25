using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class WasteBasketRepo
    {
        private readonly ApplicationContext _dbContext;

        public WasteBasketRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить элемент корзины по ID (без загрузки изображения)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WasteBasket> GetById(int id)
        {
            return await _dbContext.WasteBaskets
                .FirstOrDefaultAsync(w => w.WasteBasketId == id);
        }

        /// <summary>
        /// Получить элемент корзины с изображением
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WasteBasket> GetWithImageById(int id)
        {
            return await _dbContext.WasteBaskets
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.WasteBasketId == id);
        }

        /// <summary>
        /// Получить все элементы корзины пользователя (без изображений)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WasteBasket>> GetByUserId(int userId)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync();
        }

        /// <summary>
        /// Получить элементы корзины пользователя с изображениями
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WasteBasket>> GetWithImagesByUserId(int userId)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .Include(w => w.Image)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync();
        }

        /// <summary>
        /// Добавить элемент в корзину
        /// </summary>
        /// <param name="wasteBasket"></param>
        /// <returns></returns>
        public async Task Add(WasteBasket wasteBasket)
        {
            wasteBasket.DeleteDate = DateTime.UtcNow;
            await _dbContext.WasteBaskets.AddAsync(wasteBasket);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить элемент из корзины
        /// </summary>
        /// <param name="wasteBasket"></param>
        /// <returns></returns>
        public async Task Remove(WasteBasket wasteBasket)
        {
            _dbContext.WasteBaskets.Remove(wasteBasket);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Очистить корзину пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> ClearByUserId(int userId)
        {
            var items = _dbContext.WasteBaskets.Where(w => w.UserId == userId);
            int count = await items.CountAsync();

            _dbContext.WasteBaskets.RemoveRange(items);
            await _dbContext.SaveChangesAsync();

            return count;
        }

        /// <summary>
        /// Проверить наличие изображения в корзине
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<bool> ContainsImage(int imageId)
        {
            return await _dbContext.WasteBaskets
                .AnyAsync(w => w.ImageId == imageId);
        }
    }
}
