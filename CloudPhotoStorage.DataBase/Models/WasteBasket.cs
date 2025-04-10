using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
