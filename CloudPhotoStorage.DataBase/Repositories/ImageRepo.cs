using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class ImageRepo
    {
        private readonly ApplicationContext _dbContext;

        public ImageRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Image?> GetImageByNameAsync(string imageName, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .FirstOrDefaultAsync(i => i.ImageName == imageName, cancellationToken);
        }

        public async Task<Dictionary<string, string>> GetUserImagesWithCategoriesAsync(string username, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Where(u => u.Login == username)
                .Join(
                    _dbContext.Images,
                    user => user.UserId,
                    image => image.UserId,
                    (user, image) => new { image.ImageName, image.CategoryId }
                )
                .Join(
                    _dbContext.Categories,
                    img => img.CategoryId,
                    category => category.CategoryId,
                    (img, category) => new { img.ImageName, category.CategoryName }
                )
                .ToDictionaryAsync(
                    img => img.ImageName,
                    img => img.CategoryName,
                    cancellationToken
                );
        }

        public Task<List<Image>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Image>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Image>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.ImageName.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Images.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Image image, CancellationToken cancellationToken)
        {
            await _dbContext.Images.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Update(image);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteImageByName(string name, CancellationToken cancellationToken)
        {
            var image = await GetImageByNameAsync(name, cancellationToken);
            if (image != null)
            {
                await DeleteAsync(image, cancellationToken);
            }
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .AnyAsync(i => i.ImageId == id, cancellationToken);
        }

        public async Task<int> GetCountByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<Image>> GetRecentImagesAsync(int count, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .OrderByDescending(i => i.UploadDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
