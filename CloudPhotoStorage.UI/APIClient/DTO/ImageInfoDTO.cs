using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class ImageInfoDTO
{
    [JsonPropertyName("imageName")]
    public required string  ImageName { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("uploadDate")]
    public required DateTime? UploadDate { get; set; }
}
