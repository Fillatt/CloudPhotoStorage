using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using CloudPhotoStorage.UI.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class PhotoViewModel : ViewModelBase, IRoutableViewModel
{
    #region Fields
    private ImageApiService _imageApiService;

    private ImageInfoDTO _selectedImageInfo;

    private FilesService _filesService;

    private List<string> _categories;

    private string? _currentCategory;

    private string _currentImageName;

    private List<ImageInfoDTO>? _imagesInfo;

    private List<ImageInfoDTO> _filteredImagesInfo;

    private ImageInfoDTO _currentImageInfo;

    private Bitmap? _image;

    private bool _isPlaceholderVisible = true;

    private bool _isEnabled;

    private bool _isConnected = true;

    private bool _canDelete;

    private string _placeHolder = "Выберите фотографию";
    #endregion

    #region Properties

    public List<ImageInfoDTO>? ImagesInfo
    {
        get => _imagesInfo;
        set => this.RaiseAndSetIfChanged(ref _imagesInfo, value);
    }

    public ImageInfoDTO SelectedImageInfo 
    { 
        get => _selectedImageInfo; 
        set => this.RaiseAndSetIfChanged(ref _selectedImageInfo, value); 
    }

    public string? UrlPathSegment => "PhotoViewModel";

    public IScreen HostScreen { get; }

    public List<string> Categories
    {
        get => _categories;
        set
        {
            this.RaiseAndSetIfChanged(ref _categories, value);
            if(_categories.Any()) IsEnabled = true;
            else IsEnabled = false;
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }

    public string? CurrentCategory
    {
        get => _currentCategory;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentCategory, value);
            FilterImages(value);
        }
    }

    public string CurrentImageName
    {
        get => _currentImageName;
        set => this.RaiseAndSetIfChanged(ref _currentImageName, value);
    }

    public List<ImageInfoDTO> FilteredImagesInfo
    {
        get => _filteredImagesInfo;
        set => this.RaiseAndSetIfChanged(ref _filteredImagesInfo, value);
    }

    public ImageInfoDTO CurrentImageInfo
    {
        get => _currentImageInfo;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentImageInfo, value);
            if (value != null)
            {
                CurrentImageName = value.ImageName;
                _ = GetImageAsync(value.ImageName);
                CanDelete = true;
            }
            else
            {
                CanDelete = false;
                Image = null;
                IsPlaceholderVisible = true;
            }
        }
    }

    public Bitmap? Image
    {
        get => _image;
        set
        {
            this.RaiseAndSetIfChanged(ref _image, value);
            if (value != null) IsPlaceholderVisible = false;
        }
    }

    public bool IsPlaceholderVisible
    {
        get => _isPlaceholderVisible;
        set => this.RaiseAndSetIfChanged(ref _isPlaceholderVisible, value);
    }

    public bool CanDelete
    {
        get => _canDelete;
        set => this.RaiseAndSetIfChanged(ref _canDelete, value);
    }

    public string PlaceHolder
    {
        get => _placeHolder;
        set => this.RaiseAndSetIfChanged(ref _placeHolder, value);
    }
    #endregion

    #region Constructors
    public PhotoViewModel(ImageApiService imageApiService, IScreen screen, FilesService filesService)
    {
        _imageApiService = imageApiService;
        _filesService = filesService;
        HostScreen = screen;

        _ = GetImagesInfoAsync();
    }
    #endregion

    #region Public Methods
    public async Task SendImageAsync()
    {
        try
        {
            AddPhotoDialogViewModel addPhotoDialog = new(_categories, _filesService);
            await addPhotoDialog.ShowDialogAsync();
            if (addPhotoDialog.IsOK)
            {
                if (HostScreen is MainWindowViewModel mainWindowViewModel)
                {
                    ImageDTO imageDto = await addPhotoDialog.GetImageDTOAsync();
                    SendImageDTO sendImageDTO = new SendImageDTO
                    {
                        Name = imageDto.Name,
                        ImageData = imageDto.ImageData,
                        CategoryName = imageDto.CategoryName,
                        UploadDate = imageDto.UploadDate,
                        Login = mainWindowViewModel.UserName,
                        Password = mainWindowViewModel.Password
                    };

                    var statusCode = await _imageApiService.SendImageAsync(sendImageDTO);

                    if (statusCode == HttpStatusCode.NotFound)
                        await ShowMessageAsync("Ошибка", "Ошибка подключения");
                    else if (statusCode == HttpStatusCode.BadRequest) 
                        await ShowMessageAsync("Ошибка", $"Изображение с именем \"{sendImageDTO.Name}\" уже существует.");
                    else if(statusCode == HttpStatusCode.OK) 
                        await ShowMessageAsync("Внимание", "Изображение добавлено.");

                    await GetImagesInfoAsync();
                }
            }
        }
        catch
        {
            OnConnectionLost();
        }
    }

    public async Task GetImagesInfoAsync()
    {
        try
        {
            if (HostScreen is MainWindowViewModel mainWindowViewModel)
            {
                AccountDTO account = new AccountDTO
                {
                    Login = mainWindowViewModel.UserName,
                    Password = mainWindowViewModel.Password
                };
                _imagesInfo = await _imageApiService.GetImagesInfoAsync(account);
                if (_imagesInfo != null) InitCategories(_imagesInfo);
            }
        }
        catch
        {
            OnConnectionLost();
        }
    }

    public async Task SaveImageAsync()
    {
        if (Image != null)
        {
            await _filesService.SaveImageAsync(Image, CurrentImageName);
            await ShowMessageAsync("Внимание", "Изображение сохранено.");
        }
    }
    #endregion

    #region Private Methods
    private void InitCategories(List<ImageInfoDTO> imageInfoDTOs)
    {
        List<string> categories = [];
        List<string> imageNames = [];

        foreach(var imageDTO in  imageInfoDTOs)
        {
            if(!categories.Any(x => x == imageDTO.Category)) categories.Add(imageDTO.Category);
        }

        Categories = categories;
        CurrentCategory = Categories.FirstOrDefault();
    }

    private void FilterImages(string? category)
    {
        if (category != null)
        {
            var imagesInfo = _imagesInfo.Where(x => x.Category == category).ToList();
            FilteredImagesInfo = imagesInfo;
        }
        else FilteredImagesInfo = [];
    }

    private async Task GetImageAsync(string imageName)
    {
        try
        {
            if (HostScreen is MainWindowViewModel mainWindowViewModel)
            {
                GetImageDTO getImageDTO = new GetImageDTO
                {
                    ImageName = imageName,
                    Password = mainWindowViewModel.Password,
                    Login = mainWindowViewModel.UserName
                };

                var imageDTO = await _imageApiService.GetImageAsync(getImageDTO);

                if (imageDTO != null)
                {
                    MemoryStream memoryStream = new MemoryStream(imageDTO.ImageData);
                    Image = new Bitmap(memoryStream);
                }
            }
        }
        catch
        {
            OnConnectionLost();
        }
    }

    private void OnConnectionLost()
    {
        IsConnected = false;
        PlaceHolder = "Отсутсвует соединение с сервером";
    }

    private async Task DeleteImageAsync()
    {
        if (HostScreen is MainWindowViewModel mainWindowViewModel)
        {
            GetImageDTO dto = new GetImageDTO
            {
                ImageName = _currentImageName,
                Login = mainWindowViewModel.UserName,
                Password = mainWindowViewModel.Password,
            };

            DecisionViewModel decisionViewModel = new($"Подтвердите удаление изображения \"{dto.ImageName}\".");
            await decisionViewModel.ShowDialogAsync();
            if (decisionViewModel.IsOk)
            {
                var statusCode = await _imageApiService.DeleteImage(dto);
                if (statusCode == System.Net.HttpStatusCode.NotFound) await ShowMessageAsync("Ошибка", "Ошибка подключения");
                else
                {
                    await ShowMessageAsync("Внимание", $"Изображение \"{dto.ImageName}\" удалено.");
                    await GetImagesInfoAsync();
                }
            }
        }
    }

    private async Task ShowMessageAsync(string caption, string message)
    {
        if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var messageBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(caption, message);
            if (desktop.MainWindow != null) await messageBox.ShowWindowDialogAsync(desktop.MainWindow);
        }
    }
    #endregion
}
