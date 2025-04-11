using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string? Role { get; set; }
    }
}
