using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CloudPhotoStorage.UI.ViewModels;

namespace CloudPhotoStorage.UI.Views;

public partial class PhotoView : ReactiveUserControl<PhotoViewModel>
{
    public PhotoView()
    {
        InitializeComponent();
    }
}