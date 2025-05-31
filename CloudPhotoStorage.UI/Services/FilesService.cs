using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.Services;

public class FilesService
{
    #region Public Methods
    public async Task<IStorageFile?> OpenImageFileAsync()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
        {
            var files = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Выберите изображение",
                FileTypeFilter = [FilePickerFileTypes.ImageAll],
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }
        else return null;
    }

    public async Task SaveImageAsync(Bitmap image, string imageName)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
        {
            var folders = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Выберите директорию",
                AllowMultiple = false
            });
            var folder = folders.FirstOrDefault();
            if(folder != null)
            {
                image.Save(Path.Combine(folder.Path.LocalPath, imageName));
            }
        }
    }
    #endregion
}
