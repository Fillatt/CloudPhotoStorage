using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class LoginHistoryRepo
    {
        private readonly ApplicationContext _dbContext;

        public LoginHistoryRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LoginHistory> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .FirstOrDefaultAsync(lh => lh.LoginId == id, cancellationToken);
        }

        public async Task<IEnumerable<LoginHistory>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<LoginHistory> GetLastLoginAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<LoginHistory>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            await _dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            _dbContext.LoginHistories.Remove(loginHistory);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var loginHistory = await GetByIdAsync(id, cancellationToken);
            if (loginHistory != null)
            {
                await DeleteAsync(loginHistory, cancellationToken);
            }
        }

        public async Task<int> GetLoginCountAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .CountAsync(cancellationToken);
        }
    }
}
