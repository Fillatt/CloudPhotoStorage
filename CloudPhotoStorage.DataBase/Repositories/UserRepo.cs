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

        public Task<User?> GetUserById(Guid? id, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);
        }
        public Task<Guid?> GetUserIdByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => u.Login == login)
                .Select(u => (Guid?)u.UserId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        }

        public Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .ToListAsync(cancellationToken);
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Remove(user);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> UserLoginExists(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .AnyAsync(u => u.Login == login, cancellationToken);
        }
    }
}
