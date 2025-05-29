using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class ImageInfoDTO
{
    public required string  Name { get; set; }

    public required string CategoryName { get; set; }
}
