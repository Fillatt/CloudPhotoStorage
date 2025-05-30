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

        public async Task<WasteBasket> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        public async Task<WasteBasket> GetWithImageByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        public async Task<IEnumerable<WasteBasket>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WasteBasket>> GetWithImagesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .Include(w => w.Image)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            wasteBasket.DeleteDate = DateTime.UtcNow;
            await _dbContext.WasteBaskets.AddAsync(wasteBasket, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            _dbContext.WasteBaskets.Remove(wasteBasket);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> ClearByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var items = _dbContext.WasteBaskets.Where(w => w.UserId == userId);
            int count = await items.CountAsync(cancellationToken);

            _dbContext.WasteBaskets.RemoveRange(items);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return count;
        }

        public async Task<bool> ContainsImageAsync(Guid imageId, CancellationToken cancellationToken)
        {
            return await _dbContext.WasteBaskets
                .AnyAsync(w => w.ImageId == imageId, cancellationToken);
        }
    }
}