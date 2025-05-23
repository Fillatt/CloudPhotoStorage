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

    public async Task RegistrationAsync(AccountDTO account)
    {
        var content = JsonContent.Create(account);  

        var test = await _httpClient.PostAsync($"{_apiURL}api/account/registration", content);
    }
}
