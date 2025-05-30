using Autofac;
using CloudPhotoStorage.UI.APIClient.Services;
using CloudPhotoStorage.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.Autofac.Modules;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        string configurationFile = "appsettings.json";
        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\CloudPhotoStorage.UI", configurationFile);

        IConfiguration configuration = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                   .AddJsonFile(configurationFile)
                   .Build();
        builder
            .RegisterInstance<string>(appSettingsPath);

        builder
            .RegisterInstance<IConfiguration>(configuration);

        builder
            .RegisterType<ConfigurationService>();

        builder
            .RegisterType<ImageApiService>();

        builder
            .RegisterType<AuthenticationApiService>();

        builder
            .RegisterType<FilesService>();
    }
}
