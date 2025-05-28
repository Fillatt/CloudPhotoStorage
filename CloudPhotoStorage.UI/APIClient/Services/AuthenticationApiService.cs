using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.Services;

public class AuthenticationApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private readonly string? _apiURL;

    public AuthenticationApiService(IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("BaseURL");
        _apiURL = section.Value;
    }

    public async Task<bool> RegistrationAsync(AccountDTO account)
    {
        var response = await _httpClient.PostAsJsonAsync<AccountDTO>($"{_apiURL}api/account/registration", account);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LoginAsync(AccountDTO account)
    {
        var response = await _httpClient.PostAsJsonAsync<AccountDTO>($"{_apiURL}api/account/login", account);
        return response.IsSuccessStatusCode;
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
