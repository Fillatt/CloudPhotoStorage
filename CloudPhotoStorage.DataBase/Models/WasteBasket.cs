using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class WasteBasket
    {
        [Key]
        public int WasteBasketId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ImageId")]
        public int ImageId { get; set; }

        public DateTime? DeleteDate { get; set; }

        public virtual User User { get; set; }
        public virtual Image Image { get; set; }
    }
}
