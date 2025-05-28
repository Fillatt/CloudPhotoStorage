using CloudPhotoStorage.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<LoginHistory> LoginHistories => Set<LoginHistory>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<WasteBasket> WasteBaskets => Set<WasteBasket>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               // optionsBuilder.UseNpgsql("Data Source=CloudPhotoStorage.db");
                optionsBuilder.UseNpgsql(
                    "Host=localhost;" +
                    "Database=CloudPhotoStorage;" +
                    "Username=postgres;" +
                    "Password=123");
            }           
            base.OnConfiguring(optionsBuilder);
        }
    }
}
