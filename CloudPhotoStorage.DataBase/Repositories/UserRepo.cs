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
    }
}
