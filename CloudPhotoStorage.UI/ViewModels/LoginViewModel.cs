using System;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class LoginViewModel : ViewModelBase, IRoutableViewModel
{
    #region Fields 
    private AuthenticationApiService _authenticationApiService;

    private string _login = string.Empty;

    private string _password = string.Empty;

    private bool _isEnabled = true;
    #endregion

    #region Properties
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

    public string? UrlPathSegment => "LoginViewModel";

    public IScreen HostScreen { get; }
    #endregion

    #region Events
    public event EventHandler<EventArgs>? Logined;
    #endregion

    #region Constructors
    public LoginViewModel(AuthenticationApiService authenticationApiService, IScreen hostSreen)
    {
        _authenticationApiService = authenticationApiService;

        HostScreen = hostSreen;
    }
    #endregion

    #region Public Methods
    public IObservable<IRoutableViewModel> Register()
    {
        MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)HostScreen;
        return mainWindowViewModel.RegistrationSelect();
    }

    public async Task LoginAsync()
    {
        IsEnabled = false;
        if (IsValidated())
        {
            var account = new AccountDTO
            {
                Login = Login,
                Password = Password
            };

            await TryLoginAsync(account);
        }
        else
        {
            await ShowMessageAsync("Ошибка", "Имя пользователя или пароль не могут быть пустыми.");
            IsEnabled = true;
        }
        IsEnabled = true;
    }
    #endregion

    #region Private Methods
    private bool IsValidated() => !IsLoginEmpty() && !IsPasswordEmpty();

    private bool IsLoginEmpty() => _login == string.Empty || _login == null;

    private bool IsPasswordEmpty() => _password == string.Empty || _password == null;

    private async Task ShowMessageAsync(string caption, string message)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var messageBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(caption, message);
            if (desktop.MainWindow != null) await messageBox.ShowWindowDialogAsync(desktop.MainWindow);
        }
    }

    private async Task TryLoginAsync(AccountDTO account)
    {
        try
        {
            var IsSuccess = await _authenticationApiService.LoginAsync(account);
            if (IsSuccess)
            {
                await ShowMessageAsync("Внимание", $"Вход в аккаунт \"{Login}\" прошел успешно.");
                Logined?.Invoke(this, new EventArgs());
                ResetLoginAndPassword();
            }
            else
            {
                await ShowMessageAsync("Ошибка", "Неверно указано имя пользователя или пароль");
            }
        }
        catch
        {
            await ShowMessageAsync("Ошибка", "Отсутсвует соединение с сервером.");
        }
    }

    private void ResetLoginAndPassword()
    {
        Login = string.Empty;
        Password = string.Empty;
    }
    #endregion
}
