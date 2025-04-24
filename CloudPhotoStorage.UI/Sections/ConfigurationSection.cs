using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections;

public class ConfigurationSection : ISection
{
    private ConfigurationViewModel _configurationViewModel;

    public string Name { get; } = "Настройки конфигурации";

    public UserControl Control { get; }

    public ConfigurationSection(ConfigurationViewModel configurationViewModel)
    {
        _configurationViewModel = configurationViewModel;

        Control = new ConfigurationView()
        {
            DataContext = _configurationViewModel
        };
    }
}
