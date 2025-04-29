using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections;

public class AuthenticationSection : IMenuSection
{
    private AuthenticationViewModel _authenticationViewModel;

    public string Name { get; } = "Аутентификация";

    public UserControl View { get; }

    public AuthenticationSection(AuthenticationViewModel authenticationViewModel)
    {
        _authenticationViewModel = authenticationViewModel;

        View = new AuthenticationView()
        {
            DataContext = _authenticationViewModel
        };
    }
}
