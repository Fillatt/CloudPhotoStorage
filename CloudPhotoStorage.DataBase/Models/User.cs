using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [ForeignKey("RoleID")]
        public string RoleID { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
