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

        /// <summary>
        /// Получить запись истории входа по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LoginHistory> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .FirstOrDefaultAsync(lh => lh.LoginId == id, cancellationToken);
        }

        /// <summary>
        /// Получить все записи истории входов для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LoginHistory>> GetByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Получить последний вход пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LoginHistory> GetLastLogin(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Получить все записи истории входов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoginHistory>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить новую запись в историю входов
        /// </summary>
        /// <param name="loginHistory"></param>
        /// <returns></returns>
        public async Task Add(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            await _dbContext.LoginHistories.AddAsync(loginHistory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить запись из истории входов
        /// </summary>
        /// <param name="loginHistory"></param>
        /// <returns></returns>
        public async Task Delete(LoginHistory loginHistory, CancellationToken cancellationToken)
        {
            _dbContext.LoginHistories.Remove(loginHistory);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить запись по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(Guid id, CancellationToken cancellationToken)
        {
            var loginHistory = await GetById(id, cancellationToken);
            if (loginHistory != null)
            {
                await Delete(loginHistory, cancellationToken);
            }
        }

        /// <summary>
        /// Получить количество входов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetLoginCount(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .CountAsync(cancellationToken);
        }
    }
}
