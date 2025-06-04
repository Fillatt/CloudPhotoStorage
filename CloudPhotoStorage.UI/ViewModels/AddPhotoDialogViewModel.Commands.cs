using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels
{
    public partial class AddPhotoDialogViewModel
    {
        private AddPhotoDialogViewModelCommands _commands;

        public AddPhotoDialogViewModelCommands Commands
        {
            get
            {
                _commands ??= new AddPhotoDialogViewModelCommands(this);
                return _commands;
            }
        }

        public sealed class AddPhotoDialogViewModelCommands(AddPhotoDialogViewModel addPhotoDialogViewModel)
        {
            public ReactiveCommand<Unit, Unit> Save =>
                ReactiveCommand.Create(addPhotoDialogViewModel.Save);

            public ReactiveCommand<Unit, Unit> Cancel =>
                ReactiveCommand.Create(addPhotoDialogViewModel.Cancel);

            public ReactiveCommand<Unit, Task> OpenFile =>
                ReactiveCommand.Create(addPhotoDialogViewModel.OpenImageFileAsync);
        }
    }
}
