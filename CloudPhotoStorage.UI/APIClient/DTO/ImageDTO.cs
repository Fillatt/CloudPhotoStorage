using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class ImageDTO
{
    public required string Name { get; set; }
    public required byte[] ImageData { get; set; }
    public required DateTime UploadDate { get; set; }
    public required string CategoryName { get; set; }
}
