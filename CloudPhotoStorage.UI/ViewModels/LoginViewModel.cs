using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    public string _login = string.Empty;

    public string _password = string.Empty;

    private AuthenticationApiService _authenticationApiService;

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

    public event EventHandler<EventArgs>? RegistrationSelected;

    public LoginViewModel(AuthenticationApiService authenticationApiService)
    {
        _authenticationApiService = authenticationApiService;
    }

    public void Register()
    {
        RegistrationSelected?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoginAsync()
    {
        if (IsValidated())
        {
            var account = new AccountDTO
            {
                Login = Login,
                Password = Password,
                Role = "Пользователь"
            };

            var IsSuccess = await _authenticationApiService.LoginAsync(account);
            if(IsSuccess)
            {
                await ShowMessageAsync("Внимание", $"Вход в аккаунт \"{Login}\" прошел успешно.");
            }
            else
            {
                await ShowMessageAsync("Ошибка", "Неверно указано имя пользователя или пароль");
            }
        }
        else
        {
            await ShowMessageAsync("Ошибка", "Имя пользователя или пароль не могут быть пустыми.");
        }
    }

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
}
