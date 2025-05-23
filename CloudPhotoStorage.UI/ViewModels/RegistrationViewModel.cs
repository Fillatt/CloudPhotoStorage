using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    private string _login;

    private string _password;

    private string _role;

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
    public string Role 
    {
        get => _role;
        set => this.RaiseAndSetIfChanged(ref _role, value);
    }

    public List<string> AvailableRoles { get; } = new()
    {
        "Пользователь",
        "Администратор"
    };

    public RegistrationViewModel(AuthenticationApiService authenticationApiService)
    {
        _authenticationApiService = authenticationApiService;
    }
    
    public async Task RegistrationAsync()
    {
        var account = new AccountDTO
        {
            Login = Login,
            Password = Password,
            Role = Role
        };

        await _authenticationApiService.RegistrationAsync(account);
    }
}