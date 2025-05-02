using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace CloudPhotoStorage.UI.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public string Login { get; set; }
    public string Password { get; set; }

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = ReactiveCommand.Create(OnLogin);
        RegisterCommand = ReactiveCommand.Create(OnRegister);
    }

    private void OnLogin()
    {
        // Авторизация
    }

    private void OnRegister()
    {
        // Переход к регистрации
    }
}
