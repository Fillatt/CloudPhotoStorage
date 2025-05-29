using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

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
        public ReactiveCommand<Unit, IRoutableViewModel> Register 
            => ReactiveCommand.CreateFromObservable(loginViewModel.Register);

        public ReactiveCommand<Unit, Task> Login
            => ReactiveCommand.Create(loginViewModel.LoginAsync);
    }
}
