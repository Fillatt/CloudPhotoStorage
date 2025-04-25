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
        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
        }
        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<User> GetByLogin(string login)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);
        }
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users
                .ToListAsync();
        }
        /// <summary>
        /// Добавить нового пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Изменить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Проверка есть ли такой логин
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<bool> HasLogin(string login)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Login == login);
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPasswordHash"></param>
        /// <param name="newPasswordSalt"></param>
        /// <returns></returns>
        public async Task UpdatePassword(int userId, string newPasswordHash, string newPasswordSalt)
        {
            var user = await GetById(userId);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
                await Update(user);
            }
        }
    }
}
