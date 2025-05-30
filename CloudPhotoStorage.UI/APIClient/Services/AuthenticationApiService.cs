using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.Services;

public class AuthenticationApiService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    private ConfigurationService _configuration;

    public AuthenticationApiService(ConfigurationService configuration)
    {
        _configuration = configuration;
    }

    public async Task<HttpStatusCode> RegistrationAsync(AccountDTO account)
    {
        var response = await _httpClient.PostAsJsonAsync<AccountDTO>($"{_configuration.GetApiUrl()}api/account/registration", account);
        return response.StatusCode;
    }

    public async Task<HttpStatusCode> LoginAsync(AccountDTO account)
    {
        var response = await _httpClient.PostAsJsonAsync<AccountDTO>($"{_configuration.GetApiUrl()}api/account/login", account);
        return response.StatusCode;
    }
}
