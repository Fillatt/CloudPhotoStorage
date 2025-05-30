using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.Services;

public class ConfigurationService
{
    private IConfiguration _configuration;

    private string? _apiUrl;

    private string _filePath;

    private string _filePathBase;

    public ConfigurationService(IConfiguration configuration, string filePath)
    {
        _configuration = configuration;
        _filePath = filePath;
        _filePathBase = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    }

    public string? GetApiUrl()
    {
        InitConfiguration();
        return _configuration.GetRequiredSection("ApiUrl").Value;
    }

    public async Task SetApiUrlAsync(string? apiUrl)
    {
        var json = await File.ReadAllTextAsync(_filePath);
        var appSettings = JsonSerializer.Deserialize<AppSettings>(json);
        if (appSettings != null)
        {
            appSettings.ApiUrl = apiUrl;
            json = JsonSerializer.Serialize(appSettings);
            await File.WriteAllTextAsync(_filePath, json);
            await File.WriteAllTextAsync(_filePathBase, json);
        }
    }

    private void InitConfiguration()
    {
        _configuration = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                   .AddJsonFile("appsettings.json")
                   .Build();
    }

    public class AppSettings
    {
        public string? ApiUrl { get; set; }
    }
}
