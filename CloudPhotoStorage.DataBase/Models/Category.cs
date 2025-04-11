using System.ComponentModel.DataAnnotations;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
