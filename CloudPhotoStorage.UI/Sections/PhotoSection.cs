using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections
{
    public class PhotoSection : ISection
    {
        private PhotoViewModel _photoViewModel;

        public string Name { get; } = "Фотографии";

        public UserControl Control { get; }

        public PhotoSection(PhotoViewModel photoViewModel)
        {
            _photoViewModel = photoViewModel;

            Control = new PhotoView()
            {
                DataContext = _photoViewModel
            };
        }
    }
}
