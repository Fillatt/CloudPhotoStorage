using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class PhotoViewModel
{
    private PhotoViewModelCommands _commands;

    public PhotoViewModelCommands Commands
    {
        get
        {
            _commands ??= new PhotoViewModelCommands(this);
            return _commands;
        }
    }

    public sealed class PhotoViewModelCommands(PhotoViewModel photoViewModel)
    {
        public ReactiveCommand<Unit, Task> SendImage =>
            ReactiveCommand.Create(photoViewModel.SendImageAsync);

        public ReactiveCommand<Unit, Task> GetImagesInfo =>
           ReactiveCommand.Create(photoViewModel.GetImagesInfoAsync);

        public ReactiveCommand<Unit, Task> Delete =>
          ReactiveCommand.Create(photoViewModel.DeleteImageAsync);

        public ReactiveCommand<Unit, Task> Save =>
          ReactiveCommand.Create(photoViewModel.SaveImageAsync);
    }
}
