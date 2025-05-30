﻿using Autofac;
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

public partial class MainWindowViewModel : ViewModelBase, IScreen
{
    #region Fields
    private LoginViewModel? _loginViewModel;

    private RegistrationViewModel _registrationViewModel;

    private PhotoViewModel _photoViewModel;

    private ConfigurationViewModel _configurationViewModel;

    private bool _isLogined;

    private bool _isAuthorizationButtonVisible = true;

    private bool _isAuthenticationChecked;

    private bool _isPhotoChecked;

    private bool _isConfigurationChecked;

    private string _userName;

    private string _password;

    private IComponentContext _componentContext;
    #endregion

    #region Properties
    public bool IsLogined
    {
        get => _isLogined;
        set
        {
            this.RaiseAndSetIfChanged(ref _isLogined, value);
            IsAuthorizationButtonVisible = !value;
        }
    }

    public bool IsAuthorizationButtonVisible
    {
        get => _isAuthorizationButtonVisible;
        set => this.RaiseAndSetIfChanged(ref _isAuthorizationButtonVisible, value);
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

    public string Password => _password;

    public RoutingState Router { get; } = new RoutingState();
    #endregion

    #region Constructors
    public MainWindowViewModel(IComponentContext componentContext)
    {
        _componentContext = componentContext;
    }
    #endregion

    #region Public Methods
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

    public IObservable<IRoutableViewModel> Exit()
    {
        IsLogined = false;
        _userName = string.Empty;
        _password = string.Empty; 

        return LoginSelect();
    }

    public IObservable<IRoutableViewModel> LoginSelect()
    {
        LoginCheck();

        if (_loginViewModel != null) _loginViewModel.Logined -= OnLogined;
        _loginViewModel = _componentContext.Resolve<LoginViewModel>();
        _loginViewModel.Logined += OnLogined;

        return Router.Navigate.Execute(_loginViewModel);
    }

    public IObservable<IRoutableViewModel> RegistrationSelect()
    {
        _registrationViewModel = _componentContext.Resolve<RegistrationViewModel>();

        return Router.Navigate.Execute(_registrationViewModel);
    }

    public IObservable<IRoutableViewModel> PhotoSelect()
    {
        PhotoCheck();

        _photoViewModel = _componentContext.Resolve<PhotoViewModel>();

        return Router.Navigate.Execute(_photoViewModel);
    }

    public IObservable<IRoutableViewModel> ConfigurationSelect()
    {
        ConfigurationCheck();

        _configurationViewModel = _componentContext.Resolve<ConfigurationViewModel>();

        return Router.Navigate.Execute(_configurationViewModel);
    }
    #endregion

    #region Private Methods
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

    private void LoginCheck()
    {
        IsAuthenticationChecked = true;
        IsConfigurationChecked = false;
        IsPhotoChecked = false;
    }
    #endregion
}
