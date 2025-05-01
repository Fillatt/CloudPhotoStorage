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
        public async Task<WasteBasket> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        /// <summary>
        /// Получить элемент корзины с изображением
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WasteBasket> GetWithImageById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        /// <summary>
        /// Получить все элементы корзины пользователя (без изображений)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WasteBasket>> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получить элементы корзины пользователя с изображениями
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WasteBasket>> GetWithImagesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .Include(w => w.Image)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить элемент в корзину
        /// </summary>
        /// <param name="wasteBasket"></param>
        /// <returns></returns>
        public async Task Add(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            wasteBasket.DeleteDate = DateTime.UtcNow;
            await _dbContext.WasteBaskets.AddAsync(wasteBasket, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить элемент из корзины
        /// </summary>
        /// <param name="wasteBasket"></param>
        /// <returns></returns>
        public async Task Remove(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            _dbContext.WasteBaskets.Remove(wasteBasket);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Очистить корзину пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> ClearByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var items = _dbContext.WasteBaskets.Where(w => w.UserId == userId);
            int count = await items.CountAsync(cancellationToken);

            _dbContext.WasteBaskets.RemoveRange(items);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return count;
        }

        /// <summary>
        /// Проверить наличие изображения в корзине
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<bool> ContainsImage(Guid imageId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .AnyAsync(w => w.ImageId == imageId, cancellationToken);
        }
    }
}
