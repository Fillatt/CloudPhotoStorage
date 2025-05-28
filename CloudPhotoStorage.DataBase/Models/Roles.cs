using System.ComponentModel.DataAnnotations;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Roles
    {
        [Key]
        public required Guid  RoleId { get; set; }
        public required string Role { get; set; }
    }
}
