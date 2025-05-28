using Autofac;
using CloudPhotoStorage.UI.Autofac.Modules;

namespace CloudPhotoStorage.UI.Autofac;

public static class ContainerRegistration
{
    public static ContainerBuilder RegisterModules(this ContainerBuilder containerBuilder)
    {
        containerBuilder
            .RegisterModule<ViewModelsModule>()
            .RegisterModule<ServicesModule>();

        return containerBuilder;
    }
}
