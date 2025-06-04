using Avalonia.ReactiveUI;
using CloudPhotoStorage.UI.ViewModels;

namespace CloudPhotoStorage.UI.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }
}