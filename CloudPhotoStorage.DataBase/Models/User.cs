using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.DataBase.Models
{
    internal class User
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        private int UserId { get; set; }
        private string Login { get; set; }
        private string PasswordHash { get; set; }
        private string PasswordSalt { get; set; }
        /// <summary>
        /// Внешний ключ
        /// </summary>
        private string RoleID { get; set; }
    }
}
