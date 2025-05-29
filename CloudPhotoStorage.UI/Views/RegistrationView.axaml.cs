using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CloudPhotoStorage.UI.ViewModels;

namespace CloudPhotoStorage.UI.Views;

public partial class RegistrationView : ReactiveUserControl<RegistrationViewModel>
{
    public RegistrationView()
    {
        InitializeComponent();
    }
}