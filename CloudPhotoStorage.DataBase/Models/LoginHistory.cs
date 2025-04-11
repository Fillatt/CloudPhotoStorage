using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class LoginHistory
    {
        [Key]
        public int LoginId { get; set; }
        
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
