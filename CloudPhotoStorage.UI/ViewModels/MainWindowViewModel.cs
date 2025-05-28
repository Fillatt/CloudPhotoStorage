using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private AuthenticationViewModel _authenticationViewModel;

    private AuthenticationView _authenticationView;

    private PhotoViewModel _photoViewModel;

    private PhotoView _photoView;

    private ConfigurationViewModel _configurationViewModel;

    private ConfigurationView _configurationView;

    private UserControl _currentView;

    private bool _isLogined;

    private bool _isAuthenticationChecked;

    private bool _isPhotoChecked;

    private bool _isConfigurationChecked;

    private string _userName;

    private string _password;

    public bool IsLogined
    {
        get => _isLogined;
        set => this.RaiseAndSetIfChanged(ref _isLogined, value);
    }
    
    public UserControl CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public bool IsAuthenticationChecked
    {
        get => _isAuthenticationChecked;
        set => this.RaiseAndSetIfChanged(ref _isAuthenticationChecked, value);
    }

    public bool IsPhotoChecked
    {
        get => _isPhotoChecked;
        set => this.RaiseAndSetIfChanged(ref _isPhotoChecked, value);
    }

    public bool IsConfigurationChecked
    {
        get => _isConfigurationChecked;
        set => this.RaiseAndSetIfChanged(ref _isConfigurationChecked, value);
    }

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public ReactiveCommand<Unit, Unit> AuthenticationSelectCommand { get; }

    public ReactiveCommand<Unit, Unit> PhotoSelectCommand { get; }

    public ReactiveCommand<Unit, Unit> ConfigurationSelectCommand { get; }

    public MainWindowViewModel(
        AuthenticationViewModel authenticationViewModel,
        PhotoViewModel photoViewModel,
        ConfigurationViewModel configurationViewModel)
    {
        _authenticationViewModel = authenticationViewModel;
        _authenticationViewModel.Logined += OnLogined;
        _photoViewModel = photoViewModel;
        _configurationViewModel = configurationViewModel;

        _authenticationView = new AuthenticationView() { DataContext = _authenticationViewModel };
        _photoView = new PhotoView() { DataContext = _photoViewModel };
        _configurationView = new ConfigurationView() { DataContext = _configurationViewModel };

        AuthenticationSelectCommand = ReactiveCommand.Create(AuthenticationSelect);
        PhotoSelectCommand = ReactiveCommand.Create(PhotoSelect);
        ConfigurationSelectCommand = ReactiveCommand.Create(ConfigurationSelect);

        AuthenticationSelect();
    }

    public void OnLogined(object? sender, EventArgs args)
    {
        if (sender is LoginViewModel loginViewModel)
        {
            IsLogined = true;
            UserName = loginViewModel.Login;
            _password = loginViewModel.Password;

            PhotoSelect();
        }
    }

    public void AuthenticationSelect()
    {
        AuthenticationCheck();
        CurrentView = _authenticationView;
        IsLogined = false;
    }

    public void PhotoSelect()
    {
        PhotoCheck();
        CurrentView = _photoView;
    }

    public void ConfigurationSelect()
    {
        ConfigurationCheck();
        CurrentView = _configurationView;
    }

    private void AuthenticationCheck()
    {
        IsAuthenticationChecked = true;
        IsConfigurationChecked = false;
        IsPhotoChecked = false;
    }

    private void PhotoCheck()
    {
        IsAuthenticationChecked = false;
        IsConfigurationChecked = false;
        IsPhotoChecked = true;
    }

    private void ConfigurationCheck()
    {
        IsAuthenticationChecked = false;
        IsConfigurationChecked = true;
        IsPhotoChecked = false;
    }
}
