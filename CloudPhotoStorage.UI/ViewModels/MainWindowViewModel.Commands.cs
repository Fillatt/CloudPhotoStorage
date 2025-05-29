using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class MainWindowViewModel
{
    private MainWindowViewModelCommands _commands;

    public MainWindowViewModelCommands Commands
    {
        get
        {
            _commands ??= new MainWindowViewModelCommands(this);
            return _commands;
        }
    }

    public sealed class MainWindowViewModelCommands(MainWindowViewModel mainWindowViewModel)
    {
        public ReactiveCommand<Unit, IRoutableViewModel> Exit
            => ReactiveCommand.CreateFromObservable(mainWindowViewModel.Exit);

        public ReactiveCommand<Unit, IRoutableViewModel> LoginSelect
            => ReactiveCommand.CreateFromObservable(mainWindowViewModel.LoginSelect);

        public ReactiveCommand<Unit, IRoutableViewModel> RegistrationSelect
            => ReactiveCommand.CreateFromObservable(mainWindowViewModel.RegistrationSelect);

        public ReactiveCommand<Unit, IRoutableViewModel> PhotoSelect
            => ReactiveCommand.CreateFromObservable(mainWindowViewModel.PhotoSelect);

        public ReactiveCommand<Unit, IRoutableViewModel> ConfigurationSelect
            => ReactiveCommand.CreateFromObservable(mainWindowViewModel.ConfigurationSelect);
    }
}
