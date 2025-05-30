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

        public Task<WasteBasket> GetWasteBasketById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.WasteBaskets
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        public Task<WasteBasket> GetWasteBasketWithImageById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.WasteBaskets
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.WasteBasketId == id, cancellationToken);
        }

        public Task<List<WasteBasket>> GetWasteBasketsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        public Task<List<WasteBasket>> GetWasteBasketsWithImagesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.WasteBaskets
                .Where(w => w.UserId == userId)
                .Include(w => w.Image)
                .OrderByDescending(w => w.DeleteDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddWasteBasketAsync(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            wasteBasket.DeleteDate = DateTime.UtcNow;
            await _dbContext.WasteBaskets.AddAsync(wasteBasket, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveWasteBasket(WasteBasket wasteBasket, CancellationToken cancellationToken)
        {
            _dbContext.WasteBaskets.Remove(wasteBasket);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> ClearWasteBasketsByUserId(Guid userId, CancellationToken cancellationToken)
        {
            var items = _dbContext.WasteBaskets.Where(w => w.UserId == userId);
            int count = await items.CountAsync(cancellationToken);

            _dbContext.WasteBaskets.RemoveRange(items);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return count;
        }

        public Task<bool> WasteBasketContainsImage(Guid imageId, CancellationToken cancellationToken)
        {
            return _dbContext.WasteBaskets
                .AnyAsync(w => w.ImageId == imageId, cancellationToken);
        }
    }
}