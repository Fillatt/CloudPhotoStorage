using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class LoginHistory
    {
        [Key]
        public Guid LoginId { get; set; }
        
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
