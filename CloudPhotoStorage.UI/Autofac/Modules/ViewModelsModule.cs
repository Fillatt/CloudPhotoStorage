using Autofac;
using CloudPhotoStorage.UI.ViewModels;
using ReactiveUI;

namespace CloudPhotoStorage.UI.Autofac.Modules;

public class ViewModelsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterType<MainWindowViewModel>()
            .SingleInstance()
            .As<IScreen>()
            .AsSelf();

        builder
            .RegisterType<PhotoViewModel>()
            .As<IRoutableViewModel>()
            .AsSelf();

        builder
            .RegisterType<ConfigurationViewModel>()
            .As<IRoutableViewModel>()
            .AsSelf();

        builder
            .RegisterType<LoginViewModel>()
            .As<IRoutableViewModel>()
            .AsSelf();

        builder
            .RegisterType<RegistrationViewModel>()
            .As<IRoutableViewModel>()
            .AsSelf();
    }
}
