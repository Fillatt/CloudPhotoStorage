using System.ComponentModel.DataAnnotations;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        public string? Role { get; set; }
    }
}
