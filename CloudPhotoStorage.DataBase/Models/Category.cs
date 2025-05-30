using System.ComponentModel.DataAnnotations;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }

        public required string CategoryName { get; set; }
    }
}
