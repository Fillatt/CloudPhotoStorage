using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudPhotoStorage.DataBase.Models
{
    public class Image
    {
        [Key]
        public int ImageId {  get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId {  get; set; }

        public string? FileName {  get; set; }
        public string? FilePath {  get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
