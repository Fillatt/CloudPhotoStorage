using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class ConfigurationViewModel
{
    private ConfigurationViewModelCommands _commands;

    public ConfigurationViewModelCommands Commands
    {
        get
        {
            _commands ??= new ConfigurationViewModelCommands(this);
            return _commands;
        }
    }

    public sealed class ConfigurationViewModelCommands(ConfigurationViewModel configurationViewModel)
    {
        public ReactiveCommand<Unit, Task> SaveConfiguration =>
            ReactiveCommand.Create(configurationViewModel.SaveConfigurationAsync);
    }
}
