using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System;

namespace CloudPhotoStorage.UI;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
    {
        LoginViewModel context => new LoginView { DataContext = context },
        RegistrationViewModel context => new RegistrationView { DataContext = context },
        PhotoViewModel context => new PhotoView { DataContext = context },
        ConfigurationViewModel context => new ConfigurationView { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}
