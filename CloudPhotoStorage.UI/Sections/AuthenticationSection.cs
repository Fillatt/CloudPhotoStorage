using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections;

public class AuthenticationSection : ISection
{
    private AuthenticationViewModel _authenticationViewModel;

    public string Name { get; } = "Аутентификация";

    public UserControl Control { get; }

    public AuthenticationSection(AuthenticationViewModel authenticationViewModel)
    {
        _authenticationViewModel = authenticationViewModel;

        Control = new AuthenticationView()
        {
            DataContext = _authenticationViewModel
        };
    }
}
