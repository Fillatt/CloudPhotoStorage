using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class WasteBasket
    {
        [Key]
        public Guid WasteBasketId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("ImageId")]
        public Guid ImageId { get; set; }

        public DateTime? DeleteDate { get; set; }

        public virtual User User { get; set; }
        public virtual Image Image { get; set; }
    }
}
