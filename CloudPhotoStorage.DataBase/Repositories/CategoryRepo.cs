﻿using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase.Repositories
{
    public class CategoryRepo
    {
        private readonly ApplicationContext _dbContext;

        public CategoryRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
        }

        public async Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var category = await GetByIdAsync(id, cancellationToken);
            if (category != null)
            {
                await DeleteAsync(category, cancellationToken);
            }
        }

        public async Task<bool> ExistscAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryId == id, cancellationToken);
        }

        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.CategoryName == name, cancellationToken);
        }

        public async Task<int> GetImageCountAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Images
                .Where(i => i.CategoryId == categoryId)
                .CountAsync(cancellationToken);
        }
    }
}
