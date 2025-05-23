using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    public string _login;

    public string _password;

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

    public event EventHandler<EventArgs> RegistrationSelected;

    public LoginViewModel()
    {

    }

    public void Register()
    {
        RegistrationSelected.Invoke(this, EventArgs.Empty);
    }
}
