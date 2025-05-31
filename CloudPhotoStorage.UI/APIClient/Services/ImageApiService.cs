using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CloudPhotoStorage.UI.APIClient.Services;

public class ImageApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private ConfigurationService _configuration;

    public ImageApiService(ConfigurationService configuration)
    {
        _configuration = configuration;
    }

    public async Task<HttpStatusCode> SendImageAsync(SendImageDTO sendImageDTO)
    {
        using var multipartFormContent = new MultipartFormDataContent();

        multipartFormContent.Add(new StringContent(sendImageDTO.Login), "Login");
        multipartFormContent.Add(new StringContent(sendImageDTO.Password), "Password");

        multipartFormContent.Add(new StringContent(sendImageDTO.Name), "Name");
        multipartFormContent.Add(new StringContent(sendImageDTO.CategoryName), "CategoryName");
        multipartFormContent.Add(new StringContent(sendImageDTO.UploadDate.ToString()), "UploadDate");

        var memoruStream = new MemoryStream(sendImageDTO.ImageData);
        var imageContent = new StreamContent(memoruStream);

        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

        multipartFormContent.Add(imageContent, "ImageData", "ImageData");

        var response = await _httpClient.PostAsync($"{_configuration.GetApiUrl()}api/images/post", multipartFormContent);
        
        return response.StatusCode;
    }

    public async Task<List<ImageInfoDTO>?> GetImagesInfoAsync(AccountDTO accountDTO)
    {
        var response = await _httpClient.PostAsJsonAsync<AccountDTO>($"{_configuration.GetApiUrl()}api/images/get/names-with-categories", accountDTO);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await ShowMessageAsync("Ошибка", "Ошибка подключения");
            throw new Exception();
        }

        List<ImageInfoDTO>? list = await response.Content.ReadFromJsonAsync<List<ImageInfoDTO>>();
        return list;
    }

    public async Task<ImageDTO?> GetImageAsync(GetImageDTO getImageDTO)
    {
        var response = await _httpClient.PostAsJsonAsync<GetImageDTO>($"{_configuration.GetApiUrl()}api/image/get", getImageDTO);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await ShowMessageAsync("Ошибка", "Ошибка подключения");
            throw new Exception();
        }

        var imageDTO = await response.Content.ReadFromJsonAsync<ImageDTO>();

        return imageDTO;
    }

    public async Task<HttpStatusCode> DeleteImage(GetImageDTO dto)
    {
        var response = await _httpClient.PostAsJsonAsync<GetImageDTO>($"{_configuration.GetApiUrl()}api/images/delete", dto);
        return response.StatusCode;
    }

    private async Task ShowMessageAsync(string caption, string message)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var messageBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(caption, message);
            if (desktop.MainWindow != null) await messageBox.ShowWindowDialogAsync(desktop.MainWindow);
        }
    }
}
