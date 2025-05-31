using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class RegistrationViewModel : ViewModelBase, IRoutableViewModel
{
    #region Fields
    private string _login;

    private string _password;

    private AuthenticationApiService _authenticationApiService;

    private bool _isEnabled = true;
    #endregion

    #region Public Fields
    public string Login 
    { 
        get => _login; 
        set => this.RaiseAndSetIfChanged(ref _login, value); 
    }
    public string Password 
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public string? UrlPathSegment => "RegistrationViewModel";

    public IScreen HostScreen { get; }
    #endregion

    #region Constructors
    public RegistrationViewModel(AuthenticationApiService authenticationApiService, IScreen screen)
    {
        HostScreen = screen;

        _authenticationApiService = authenticationApiService;
    }
    #endregion

    #region Public Methods
    public async Task RegistrationAsync()
    {
        IsEnabled = false;
        if (IsValidated())
        {
            var account = new AccountDTO
            {
                Login = Login,
                Password = Password
            };

            await TryRegisterAsync(account);
        }
        else
        {
            ShowNotification("Пароль или имя не могут быть пустым", true);
        }
        IsEnabled = true;
    }

    public IObservable<IRoutableViewModel> GoBack()
    {
        MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)HostScreen;
        return mainWindowViewModel.LoginSelect();
    }
    #endregion

    #region Private Methods
    private bool IsValidated() => !IsLoginEmpty() && !IsPasswordEmpty();

    private bool IsLoginEmpty() => _login == string.Empty || _login == null;

    private bool IsPasswordEmpty() => _password == string.Empty || _password == null;

    private void ShowNotification(string message, bool isError)
    {
        if (HostScreen is MainWindowViewModel mainWindowViewModel) 
            mainWindowViewModel.ShowNotification(message, isError);
    }

    private async Task TryRegisterAsync(AccountDTO account)
    {
        var statusCode = await _authenticationApiService.RegistrationAsync(account);
        if (statusCode == System.Net.HttpStatusCode.OK)
        {
            string message = $"Пользователь с именем \"{Login}\" успешно зарегистрирован.";
            ShowNotification(message, false);
            GoBack();
        }
        else if (statusCode == System.Net.HttpStatusCode.NotFound)
        {
            string message = "Ошибка подключения.";
            ShowNotification(message, true);
        }
        else
        {
            string message = $"Пользователь с именем \"{Login}\" существует.";
            ShowNotification(message, true);
        }
    }
    #endregion
}