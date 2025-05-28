using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Input;
using Tmds.DBus.Protocol;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    #region Fields
    private string _login;

    private string _password;

    private string _role;

    private AuthenticationApiService _authenticationApiService;

    private bool _isEnabled = true;
    #endregion

    #region Events
    public event EventHandler<EventArgs> LoginSelected;
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
    public string Role 
    {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }

    public IEnumerable<string> AvailableRoles { get; } = ["Пользователь", "Администратор"];

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }
    #endregion

    #region Constructors
    public RegistrationViewModel(AuthenticationApiService authenticationApiService)
    {
        _authenticationApiService = authenticationApiService;

        _role = AvailableRoles.First();
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
                Password = Password,
                Role = Role
            };

            await TryRegisterAsync(account);
        }
        else
        {
            await ShowMessageAsync("Ошибка", "Пароль или имя не могут быть пустым");
        }
        IsEnabled = true;
    }

    public void GoBack()
    {
        LoginSelected.Invoke(this, new EventArgs());
        ResetLoginAndPassword();
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
            if(desktop.MainWindow != null) await messageBox.ShowWindowDialogAsync(desktop.MainWindow);
        }
    }

    private async Task TryRegisterAsync(AccountDTO account)
    {
        try
        {
            if (!await _authenticationApiService.RegistrationAsync(account))
            {
                string message = $"Пользователь с именем \"{Login}\" существует.";
                await ShowMessageAsync("Ошибка", message);
            }
            else
            {
                string message = $"Пользователь с именем \"{Login}\" успешно зарегистрирован.";
                await ShowMessageAsync("Внимание", message);
                ResetLoginAndPassword();
            }
        }
        catch { await ShowMessageAsync("Ошибка", "Отсуствует соединение с сервисом."); }
    }

    private void ResetLoginAndPassword()
    {
        Login = string.Empty;
        Password = string.Empty;
    }
    #endregion
}