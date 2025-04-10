using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.DataBase.Models
{
    public class User
    {
        // Первичный ключ
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        // Внешний ключ
        [ForeignKey("RoleID")]
        public string RoleID { get; set; }
    }
}
