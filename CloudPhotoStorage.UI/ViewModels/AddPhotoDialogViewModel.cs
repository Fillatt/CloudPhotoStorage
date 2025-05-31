using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.Services;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class AddPhotoDialogViewModel : ViewModelBase
{
    #region Fields
    private string _imagePath;

    private string _imageCategory;

    private List<string> _categories;

    private string? _currentCategory;

    private FilesService _filesService;

    private IStorageFile _imageFile;

    private AddPhotoDialogWindow? _dialogWindow;

    private bool _canSave;

    private bool _isEnabled;
    #endregion

    #region Properties
    public string ImagePath
    {
        get => _imagePath;
        set
        {
            this.RaiseAndSetIfChanged(ref _imagePath, value);
            if (value == null || value == string.Empty) IsEnabled = false;
            else IsEnabled = true;
            CanSave = IsImageParametersNullOrEmpty();
        }
    }

    public string ImageCategory
    {
        get => _imageCategory;
        set
        {
            this.RaiseAndSetIfChanged(ref _imageCategory, value);
            CanSave = IsImageParametersNullOrEmpty();
        }
    }

    public List<string> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    public string? CurrentCategory
    {
        get => _currentCategory;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentCategory, value);
        }
    }

    public IStorageFile ImageFile => _imageFile;

    public bool IsOK { get; private set; }

    public bool CanSave
    {
        get => _canSave;
        set => this.RaiseAndSetIfChanged(ref _canSave, value);
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }
    #endregion

    #region Constructors
    public AddPhotoDialogViewModel(List<string> categories, FilesService filesService)
    {
        _categories = categories;
        _filesService = filesService;
    }
    #endregion

    #region Public Methods
    public void OnCategotySelected()
    {
        if(_currentCategory != null) ImageCategory = _currentCategory;
        CurrentCategory = null;
    }

    public async Task OpenImageFileAsync()
    {
        var file = await _filesService.OpenImageFileAsync();
        if(file != null)
        {
            ImagePath = file.Path.LocalPath;
            _imageFile = file;
        }
    }

    public async Task ShowDialogAsync()
    {
        if(App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _dialogWindow = new() { DataContext = this };
            if (desktop.MainWindow != null) await _dialogWindow.ShowDialog(desktop.MainWindow);
        }
    }

    public void Cancel()
    {
        IsOK = false;
        _dialogWindow?.Close();
    }

    public void Save()
    {
        IsOK = true;
        _dialogWindow?.Close();
    }

    public async Task<ImageDTO> GetImageDTOAsync()
    {
        var memoryStream = new MemoryStream();
        Stream stream = await _imageFile.OpenReadAsync();
        stream.CopyTo(memoryStream);

        byte[] imageData = memoryStream.ToArray();

        ImageDTO imageDto = new()
        {
            Name = _imageFile.Name,
            ImageData = imageData,
            CategoryName = ImageCategory,
            UploadDate = DateTime.Now
        };

        return imageDto;
    }

    public bool IsImageParametersNullOrEmpty()
    {
        return ImageCategory != null && ImageCategory != string.Empty &&
            ImagePath != null && ImagePath != string.Empty;
    }
    #endregion
}
