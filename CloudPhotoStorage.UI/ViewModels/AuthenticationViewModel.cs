using Avalonia.Controls;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CloudPhotoStorage.UI.ViewModels;

public class AuthenticationViewModel : ViewModelBase
{
    private LoginViewModel _loginViewModel;

    private RegistrationViewModel _registrationViewModel;

    private LoginView _loginView;

    private RegistrationView _registrationView;

    public UserControl CurrentView { get; set; }

    public AuthenticationViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel)
    {
        _loginViewModel = loginViewModel;
        _registrationViewModel = registrationViewModel;

        _loginView = new LoginView() { DataContext = _loginViewModel };
        _registrationView = new RegistrationView() { DataContext = _registrationViewModel };

        CurrentView = _loginView;
    }
}
