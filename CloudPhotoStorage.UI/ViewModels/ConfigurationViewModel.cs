using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.Services;
using Microsoft.Extensions.Configuration;
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

        await ShowMessageAsync("Внимание", "Настройки сохранены");
    }

    private bool IsSettingsChanged()
    {
        return _apiUrl != _currentApiUrl;
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
