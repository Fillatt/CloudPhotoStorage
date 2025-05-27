using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class RegistrationViewModel
{
    private RegistrationViewModelCommands _commands;

    public RegistrationViewModelCommands Commands
    {
        get
        {
            if (_commands == null) _commands = new RegistrationViewModelCommands(this);
            return _commands;
        }
    }

    public sealed class RegistrationViewModelCommands(RegistrationViewModel loginViewModel)
    {
        public ReactiveCommand<Unit, Task> Registration
            => ReactiveCommand.Create(loginViewModel.RegistrationAsync);

        public ReactiveCommand<Unit, Unit> GoBack 
            => ReactiveCommand.Create(loginViewModel.GoBack);
    }
}
