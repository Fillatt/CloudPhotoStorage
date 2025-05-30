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

        public Task<LoginHistory> GetLoginHistoryById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.LoginHistories
                .FirstOrDefaultAsync(lh => lh.LoginId == id, cancellationToken);
        }

        public Task<List<LoginHistory>> GetLoginHistoriesByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public Task<LoginHistory> GetLastLoginHistoryByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<List<LoginHistory>> GetAllLoginHistories(CancellationToken cancellationToken)
        {
            return _dbContext.LoginHistories
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            await _dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteLoginHistory(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            _dbContext.LoginHistories.Remove(loginHistory);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLoginHistoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var loginHistory = await GetLoginHistoryById(id, cancellationToken);
            if (loginHistory != null)
            {
                await DeleteLoginHistory(loginHistory, cancellationToken);
            }
        }

        public Task<int> GetLoginCountByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .CountAsync(cancellationToken);
        }
    }
}
