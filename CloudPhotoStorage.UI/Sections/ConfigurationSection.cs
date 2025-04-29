using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections;

public class ConfigurationSection : IMenuSection
{
    private ConfigurationViewModel _configurationViewModel;

    public string Name { get; } = "Настройки конфигурации";

    public UserControl View { get; }

    public ConfigurationSection(ConfigurationViewModel configurationViewModel)
    {
        _configurationViewModel = configurationViewModel;

        View = new ConfigurationView()
        {
            DataContext = _configurationViewModel
        };
    }
}
