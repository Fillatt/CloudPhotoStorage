using System.Text.Json.Serialization;

namespace CloudPhotoStorage.API.DTOs;

public class GetImageDTO
{
    [JsonPropertyName(nameof(Login))]
    public required string Login { get; set; }

    [JsonPropertyName(nameof(Password))]
    public required string Password { get; set; }

    [JsonPropertyName(nameof(ImageName))]
    public required string ImageName { get; set; }
}
