using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.DataBase.Models
{
    internal class LoginHistory
    {
        // Первичный ключ
        public int LoginId { get; set; }
        // Внешний ключ
        public int UserId { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
