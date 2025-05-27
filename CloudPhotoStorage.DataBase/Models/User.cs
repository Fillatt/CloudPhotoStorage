using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [ForeignKey("RoleID")]
        public Guid RoleID { get; set; }

        public required string Login { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
