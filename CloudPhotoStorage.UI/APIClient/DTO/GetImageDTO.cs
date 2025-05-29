using System.Text.Json.Serialization;

namespace CloudPhotoStorage.UI.APIClient.DTO;

public class GetImageDTO
{
    [JsonPropertyName(nameof(Login))]
    public required string Login { get; set; }

    [JsonPropertyName(nameof(Password))]
    public required string Password { get; set; }

    [JsonPropertyName(nameof(ImageName))]
    public required string ImageName { get; set; }
}
