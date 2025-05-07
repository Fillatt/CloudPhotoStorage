using CloudPhotoStorage.UI.APIClient.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CloudPhotoStorage.UI.APIClient.Services;

public class ImageApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private readonly string? _apiURL;

    public ImageApiService(IConfiguration configuration)
    {
        _apiURL = configuration.GetRequiredSection("BaseURL").ToString();
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

    public async Task GetImageNames()
    {
        var response = await _httpClient.GetAsync(_apiURL);

        var jsonString = await response.Content.ReadAsStringAsync();

        if(jsonString != null)
        {

        }
    }
}
