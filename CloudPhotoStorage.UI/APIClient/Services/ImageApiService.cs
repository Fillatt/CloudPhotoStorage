using CloudPhotoStorage.UI.APIClient.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CloudPhotoStorage.UI.APIClient.Services;

public class ImageApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private readonly string? _apiURL;

    public ImageApiService(IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("BaseURL");
        _apiURL = section.Value;
    }

    public async Task SendImageAsync(ImageDTO imageDTO)
    {
        using var multipartFormContent = new MultipartFormDataContent();

        multipartFormContent.Add(new StringContent(imageDTO.Name), "Name");
        multipartFormContent.Add(new StringContent(imageDTO.UserLogin), "UserLogin");
        multipartFormContent.Add(new StringContent(imageDTO.CategoryName), "CategoryName");
        multipartFormContent.Add(new StringContent(imageDTO.UploadDate.ToString()), "UploadDate");

        var imageContent = new ByteArrayContent(imageDTO.Bytes);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

        multipartFormContent.Add(imageContent, "ImageBytes");

        var response = await _httpClient.PostAsync(_apiURL, multipartFormContent);
    }

    public async Task<Dictionary<string, string>?> GetImagesInfoAsync()
    {
        var userName = "Vlad";

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(userName));
        using var content = new StreamContent(stream);

        var response = await _httpClient.PostAsync($"{_apiURL}api/images/get/names-with-categories", content);
        var jsonString = await response.Content.ReadAsStringAsync();

        Dictionary<string, string>? dictionary = null;
        JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

        if (jsonString != null)
        {
            dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        }

        return dictionary;
    }
}
