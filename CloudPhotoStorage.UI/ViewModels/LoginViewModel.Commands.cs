using ReactiveUI;
using System.Reactive;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class LoginViewModel
{
    private LoginViewModelCommands _commands;

    public LoginViewModelCommands Commands
    {
        get
        {
            if(_commands == null) _commands = new LoginViewModelCommands(this);
            return _commands;
        }
    }

    public sealed class LoginViewModelCommands(LoginViewModel loginViewModel)
    {
        public ReactiveCommand<Unit, Unit> Register 
            => ReactiveCommand.Create(loginViewModel.Register);
    }
}
