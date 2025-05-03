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

        public async Task<Roles> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
        }

        public async Task<Roles> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Role == roleName, cancellationToken);
        }

        public async Task<IEnumerable<Roles>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Roles.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Roles role, CancellationToken cancellationToken)
        {
            await _dbContext.Roles.AddAsync(role, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Roles role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Update(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Roles role, CancellationToken cancellationToken)
        {
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var role = await GetByIdAsync(roleId, cancellationToken);
            if (role != null)
            {
                await DeleteAsync(role, cancellationToken);
            }
        }

        public async Task<bool> ExistsByIdAsync(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.RoleId == roleId, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string roleName, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .AnyAsync(r => r.Role == roleName, cancellationToken);
        }

        public async Task<int> GetUsersCountByRoleAsync(Guid roleId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Where(u => u.RoleID == roleId)
                .CountAsync(cancellationToken);
        }
    }
}
