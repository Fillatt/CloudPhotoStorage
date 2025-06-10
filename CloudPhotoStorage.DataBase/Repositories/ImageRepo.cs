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

        public async Task AddImage(Image image, CancellationToken cancellationToken)
        {
            await _dbContext.Images.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteImage(Image image, CancellationToken cancellationToken)
        {
            _dbContext.Images.Remove(image);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}