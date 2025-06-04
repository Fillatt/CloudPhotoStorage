using CloudPhotoStorage.UI.Services;
using ReactiveUI;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class ConfigurationViewModel : ViewModelBase, IRoutableViewModel
{
    private ConfigurationService _configurationService;

    private string? _apiUrl;

    private string? _currentApiUrl;

    private bool _hasChanges;

    public string? UrlPathSegment => "ConfigurationViewModel";

    public IScreen HostScreen { get; }

    public bool HasChanges
    {
        get => _hasChanges;
        set => this.RaiseAndSetIfChanged(ref _hasChanges, value);
    }

    public string? CurrentApiUrl
    {
        get => _currentApiUrl;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentApiUrl, value);
            HasChanges = IsSettingsChanged();
        }
    }

    public ConfigurationViewModel(IScreen screen, ConfigurationService configurationService)
    {
        HostScreen = screen;

        _configurationService = configurationService;

        _apiUrl = _configurationService.GetApiUrl();
        _currentApiUrl = _apiUrl;
    }

    public async Task SaveConfigurationAsync()
    {
        await _configurationService.SetApiUrlAsync(_currentApiUrl);

        _apiUrl = _configurationService.GetApiUrl();
        CurrentApiUrl = _apiUrl;

        ShowNotification("Настройки сохранены", false);
    }

    private bool IsSettingsChanged()
    {
        return _apiUrl != _currentApiUrl;
    }

    private void ShowNotification(string message, bool isError)
    {
        if (HostScreen is MainWindowViewModel mainWindowViewModel)
            mainWindowViewModel.ShowNotification(message, isError);
    }
}
