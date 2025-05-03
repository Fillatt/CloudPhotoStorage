using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Image
    {
        [Key]
        public Guid ImageId {  get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("CategoryId")]
        public Guid CategoryId {  get; set; }

        public string ImageName {  get; set; }
        public byte[] ImageBytes{  get; set; }
        public DateTime UploadDate { get; set; }
    }
}
