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
        public async Task<Roles> GetById(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
        }

        /// <summary>
        /// Получить роль по названию
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Roles> GetByRoleName(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Role == roleName, cancellationToken);
        }

        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Roles>> GetAll(CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Добавить новую роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Add(Roles role, CancellationToken cancellationToken)
        {
            await _dbContext.Roles.AddAsync(role, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Изменить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Update(Roles role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить роль
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task Delete(Roles role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить роль по ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task DeleteById(Guid roleId, CancellationToken cancellationToken)
        {
            var role = await GetById(roleId, cancellationToken);
            if (role != null)
            {
                await Delete(role, cancellationToken);
            }
        }

        /// <summary>
        /// Проверить существование роли по ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsById(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.RoleId == roleId, cancellationToken);
        }

        /// <summary>
        /// Проверить существование роли по названию
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> ExistsByName(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.Role == roleName, cancellationToken);
        }

        /// <summary>
        /// Получить количество пользователей с данной ролью
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<int> GetUsersCountByRole(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Where(u => u.RoleID == roleId)
                .CountAsync(cancellationToken);
        }
    }
}
