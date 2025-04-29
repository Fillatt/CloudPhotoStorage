using Autofac;
using CloudPhotoStorage.UI.ViewModels;

namespace CloudPhotoStorage.UI.Autofac.Modules;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterType<MainWindowViewModel>()
            .SingleInstance();

        builder
            .RegisterType<AuthenticationViewModel>()
            .SingleInstance();

        builder
            .RegisterType<PhotoViewModel>()
            .SingleInstance();

        builder
            .RegisterType<ConfigurationViewModel>()
            .SingleInstance();

        builder
            .RegisterType<LoginViewModel>();

        builder
            .RegisterType<RegistrationViewModel>();
    }
}
