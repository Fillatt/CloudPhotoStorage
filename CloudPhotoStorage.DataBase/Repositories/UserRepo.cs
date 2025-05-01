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
        public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);
        }
        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<User> GetByLogin(string login, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        }
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .ToListAsync(cancellationToken);
        }
        /// <summary>
        /// Добавить нового пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Add(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Изменить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Update(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Delete(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Проверка есть ли такой логин
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<bool> HasLogin(string login, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Login == login, cancellationToken);
        }

        /// <summary>
        /// Изменение пароля
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPasswordHash"></param>
        /// <param name="newPasswordSalt"></param>
        /// <returns></returns>
        public async Task UpdatePassword(Guid userId, string newPasswordHash, string newPasswordSalt, CancellationToken cancellationToken)
        {
            var user = await GetById(userId, cancellationToken);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                user.PasswordSalt = newPasswordSalt;
                await Update(user, cancellationToken);
            }
        }
    }
}
