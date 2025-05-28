using Autofac;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Autofac.Modules;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterType<MainWindowViewModel>();

        builder
            .RegisterType<AuthenticationViewModel>();

        builder
            .RegisterType<PhotoViewModel>();

        builder
            .RegisterType<ConfigurationViewModel>();

        builder
            .RegisterType<LoginViewModel>();

        builder
            .RegisterType<RegistrationViewModel>();
    }
}
