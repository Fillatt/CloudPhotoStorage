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
        public async Task<LoginHistory> GetById(int id)
        {
            return await _dbContext.LoginHistories
                .FirstOrDefaultAsync(lh => lh.LoginId == id);
        }

        /// <summary>
        /// Получить все записи истории входов для пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LoginHistory>> GetByUserId(int userId)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync();
        }

        /// <summary>
        /// Получить последний вход пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LoginHistory> GetLastLogin(int userId)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .OrderByDescending(lh => lh.LoginDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получить все записи истории входов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<LoginHistory>> GetAll()
        {
            return await _dbContext.LoginHistories
                .OrderByDescending(lh => lh.LoginDate)
                .ToListAsync();
        }

        /// <summary>
        /// Добавить новую запись в историю входов
        /// </summary>
        /// <param name="loginHistory"></param>
        /// <returns></returns>
        public async Task Add(LoginHistory loginHistory)
        {
            await _dbContext.LoginHistories.AddAsync(loginHistory);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить запись из истории входов
        /// </summary>
        /// <param name="loginHistory"></param>
        /// <returns></returns>
        public async Task Delete(LoginHistory loginHistory)
        {
            _dbContext.LoginHistories.Remove(loginHistory);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить запись по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteById(int id)
        {
            var loginHistory = await GetById(id);
            if (loginHistory != null)
            {
                await Delete(loginHistory);
            }
        }

        /// <summary>
        /// Получить количество входов пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> GetLoginCount(int userId)
        {
            return await _dbContext.LoginHistories
                .Where(lh => lh.UserId == userId)
                .CountAsync();
        }
    }
}
