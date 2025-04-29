using Autofac;
using CloudPhotoStorage.UI.Sections;

namespace CloudPhotoStorage.UI.Autofac.Modules;

public class SectionsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder
            .RegisterType<AuthenticationSection>()
            .As<IMenuSection>();

        builder
            .RegisterType<PhotoSection>()
            .As<IMenuSection>();

        builder
            .RegisterType<ConfigurationSection>()
            .As<IMenuSection>();
    }
}
