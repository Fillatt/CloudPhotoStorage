using Autofac;
using CloudPhotoStorage.UI.APIClient.Services;
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

        IConfiguration configuration = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                   .AddJsonFile("appsettings.json")
                   .Build();

        builder
            .RegisterInstance<IConfiguration>(configuration);

        builder
            .RegisterType<ImageApiService>();
    }
}
