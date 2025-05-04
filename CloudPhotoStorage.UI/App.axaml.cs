using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CloudPhotoStorage.UI.Autofac;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;
using System.Linq;

namespace CloudPhotoStorage.UI
{
    public partial class App : Application
    {
        private IContainer? Container { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /*
        public override void OnFrameworkInitializationCompleted()
        {
            Container = RegisterContainer();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Container.Resolve<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        */
        public override void OnFrameworkInitializationCompleted()
        {
            Container = RegisterContainer();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();
        
                // Временный код для теста RegistrationView
                desktop.MainWindow = new MainWindow
                {
                    Content = new RegistrationView
                    {
                        DataContext = new RegistrationViewModel()
                    },
                    Width = 400,
                    Height = 450,
                    Title = "Тест регистрации"
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private IContainer RegisterContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterModules();

            return containerBuilder.Build();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}