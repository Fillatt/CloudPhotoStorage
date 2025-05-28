using Avalonia.Controls;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System;
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

    private UserControl _currentView;

    private bool _isLogined = false;

    public event EventHandler? Logined;

    public UserControl CurrentView 
    { 
        get => _currentView; 
        set => this.RaiseAndSetIfChanged(ref _currentView, value); 
    }

    public bool IsLogined
    {
        get => _isLogined;
        set => this.RaiseAndSetIfChanged(ref _isLogined, value);
    }

    public AuthenticationViewModel(LoginViewModel loginViewModel, RegistrationViewModel registrationViewModel)
    {
        _loginViewModel = loginViewModel;
        _loginViewModel.RegistrationSelected += OnRegistrationSelected;
        _loginViewModel.Logined += OnLogined;

        _registrationViewModel = registrationViewModel;

        _loginView = new LoginView() { DataContext = _loginViewModel };
        _registrationViewModel.LoginSelected += OnLoginSelected;

        _registrationView = new RegistrationView() { DataContext = _registrationViewModel };

        CurrentView = _loginView;
    }

    private void _loginViewModel_Logined(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void OnRegistrationSelected(object? sender, EventArgs eventArgs)
    {
        CurrentView = _registrationView;
    }

    public void OnLoginSelected(object? sender, EventArgs eventArgs)
    {
        CurrentView = _loginView;
    }

    public void OnLogined(object? sender, EventArgs eventArgs)
    {
        Logined?.Invoke(sender, eventArgs);
    }
}
