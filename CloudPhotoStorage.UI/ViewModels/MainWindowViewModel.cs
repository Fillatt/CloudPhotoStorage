using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace CloudPhotoStorage.UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<string> MenuItems { get; } = new()
        {
            "Авторизация",
            "Главная",
            "Фото",
            "Настройки",
            "Корзина"
        };

        public ReactiveCommand<Unit, Unit> UploadCommand { get; }

        public MainWindowViewModel()
        {
            UploadCommand = ReactiveCommand.Create(UploadPhoto);
        }

        private void UploadPhoto()
        {
            // Здесь будет логика загрузки фото
        }
    }
}