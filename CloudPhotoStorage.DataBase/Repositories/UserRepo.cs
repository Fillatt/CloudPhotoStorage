using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationContext _dbContext;

        public UserRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetUserByIdAsync(Guid? id, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);
        }
        public Task<Guid?> GetUserIdByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => u.Login == login)
                .Select(u => (Guid?)u.UserId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .ToListAsync(cancellationToken);
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> HasLoginAsync(string login, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Login == login, cancellationToken);
        }
    }
}
