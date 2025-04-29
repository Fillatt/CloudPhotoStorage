using Avalonia.Controls;
using CloudPhotoStorage.UI.ViewModels;
using CloudPhotoStorage.UI.Views;

namespace CloudPhotoStorage.UI.Sections
{
    public class PhotoSection : IMenuSection
    {
        private PhotoViewModel _photoViewModel;

        public string Name { get; } = "Фотографии";

        public UserControl View { get; }

        public PhotoSection(PhotoViewModel photoViewModel)
        {
            _photoViewModel = photoViewModel;

            View = new PhotoView()
            {
                DataContext = _photoViewModel
            };
        }
    }
}
