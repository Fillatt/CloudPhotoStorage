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

        public async Task<LoginHistory> GetLoginHistoryByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .FirstOrDefaultAsync(lh => lh.LoginId == id, cancellationToken);
        }

        public async Task<IEnumerable<LoginHistory>> GetLoginHistoryByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<LoginHistory> GetLastLoginHistoryLoginAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<LoginHistory>> GetAllLoginHistoryAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            await _dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLoginHistoryAsync(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            _dbContext.LoginHistories.Remove(loginHistory);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteLoginHistoryByIdAsync(Guid id, CancellationToken cancellationToken)
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
