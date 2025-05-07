using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class ImageDTO
{
    public string Name { get; set; }
    public byte[] Bytes { get; init; }
    public DateTime UploadDate { get; init; }
    public string UserLogin { get; init; }
    public string CategoryName { get; init; }
}
