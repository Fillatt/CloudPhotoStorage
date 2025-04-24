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

        public async Task<User> GetById(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> GetByLogin(string login)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users
                .ToListAsync();
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        // Проверка есть ли такой логин
        public async Task<bool> HasLogin(string login)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Login == login);
        }

        // Изменение пароля
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
