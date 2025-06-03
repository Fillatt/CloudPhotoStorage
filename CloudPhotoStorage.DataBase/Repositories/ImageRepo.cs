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

        public async Task<Image?> GetImageByName(string imageName, string username, CancellationToken cancellationToken)
        {
            var userId = await _dbContext.Users
                .Where(u => u.Login == username)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userId == Guid.Empty)
                return null;

            return await _dbContext.Images
                .FirstOrDefaultAsync(i => i.ImageName == imageName && i.UserId == userId, cancellationToken);
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

        public async Task<List<Image>> GetImagesByUserLogin(string username, CancellationToken cancellationToken)
        {
            var userId = await _dbContext.Users
                .Where(u => u.Login == username)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userId == Guid.Empty)
                return new List<Image>();

            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Image>> GetImagesByCategoryId(Guid categoryId, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync(cancellationToken);
        }

        public Task<List<Image>> SearchImagesByName(string searchTerm, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .Where(i => i.ImageName.Contains(searchTerm))
                .ToListAsync(cancellationToken);
        }

        public Task<List<Image>> GetAllImages(CancellationToken cancellationToken)
        {
            return _dbContext.Images.ToListAsync(cancellationToken);
        }

        public async Task AddImage(Image image, CancellationToken cancellationToken)
        {
            await _dbContext.Images.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateImage(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Update(image);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteImage(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Remove(image);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteImageByName(string name, string username, CancellationToken cancellationToken)
        {
            var image = await GetImageByName(name, username, cancellationToken);
            if (image != null)
            {
                await DeleteImage(image, cancellationToken);
            }
        }

        public Task<bool> ImageExistsById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .AnyAsync(i => i.ImageId == id, cancellationToken);
        }

        public async Task<int> GetImageCountByUserLogin(string username, CancellationToken cancellationToken)
        {
            var userId = await _dbContext.Users
                .Where(u => u.Login == username)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userId == Guid.Empty)
                return 0;

            return await _dbContext.Images
                .Where(i => i.UserId == userId)
                .CountAsync(cancellationToken);
        }

        public Task<List<Image>> GetRecentImages(int count, CancellationToken cancellationToken)
        {
            return _dbContext.Images
                .OrderByDescending(i => i.UploadDate)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}