using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class RolesRepo
    {
        private readonly ApplicationContext _dbContext;

        public RolesRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить роль по ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<Roles> GetById(int roleId)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        /// <summary>
        /// Получить роль по названию
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Roles> GetByRoleName(string roleName)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Role == roleName);
        }

        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Roles>> GetAll()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        /// <summary>
        /// Добавить новую роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Add(Roles role)
        {
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Изменить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Update(Roles role)
        {
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Delete(Roles role)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить роль по ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteById(int roleId)
        {
            var role = await GetById(roleId);
            if (role != null)
            {
                await Delete(role);
            }
        }

        /// <summary>
        /// Проверить существование роли по ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsById(int roleId)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.RoleId == roleId);
        }

        /// <summary>
        /// Проверить существование роли по названию
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> ExistsByName(string roleName)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.Role == roleName);
        }

        /// <summary>
        /// Получить количество пользователей с данной ролью
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<int> GetUsersCountByRole(int roleId)
        {
            return await _dbContext.Users
                .Where(u => u.RoleID == roleId.ToString())
                .CountAsync();
        }
    }
}
