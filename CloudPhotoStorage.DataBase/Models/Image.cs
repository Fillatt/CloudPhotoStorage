using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
