using ReactiveUI;

namespace CloudPhotoStorage.UI.ViewModels;

public class ConfigurationViewModel : ViewModelBase, IRoutableViewModel
{
    public string? UrlPathSegment => "ConfigurationViewModel";

    public IScreen HostScreen { get; }

    public ConfigurationViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}
