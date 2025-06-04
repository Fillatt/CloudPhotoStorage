using System;
using System.Text.Json.Serialization;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class SendImageDTO
{
    [JsonPropertyName(nameof(Name))]
    public required string Name { get; set; }

    [JsonPropertyName(nameof(ImageData))]
    public required byte[] ImageData { get; set; }

    [JsonPropertyName(nameof(UploadDate))]
    public required DateTime UploadDate { get; set; }

    [JsonPropertyName(nameof(CategoryName))]
    public required string CategoryName { get; set; }

    [JsonPropertyName(nameof(Login))]
    public required string Login { get; set; }

    [JsonPropertyName(nameof(Password))]
    public required string Password { get; set; }
}
