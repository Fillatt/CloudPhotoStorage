using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }
}
