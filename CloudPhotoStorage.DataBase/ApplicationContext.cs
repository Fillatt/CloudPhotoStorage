using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CloudPhotoStorage.db");
        }
    }
}
