using Avalonia.Collections;
using Avalonia.Media.Imaging;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using CloudPhotoStorage.UI.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class PhotoViewModel : ViewModelBase, IRoutableViewModel
{
    private ImageApiService _imageApiService;

    private ImageInfoDTO _selectedImageInfo;

    private FilesService _filesService;

    private List<string> _categories;

    private string? _currentCategory;

    private List<string> _imageNames;

    private string _currentImageName;

    private List<string> _filteredImageNames;

    private List<ImageInfoDTO>? _imagesInfo;

    private Bitmap _image;

    private bool _isPlaceholderVisible = true;

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
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    public string? CurrentCategory
    {
        get => _currentCategory;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentCategory, value);
            FilterImageNames(value);
        }
    }

    public List<string> ImageNames
    {
        get => _imageNames;
        set => this.RaiseAndSetIfChanged(ref _imageNames, value);
    }

    public string CurrentImageName
    {
        get => _currentImageName;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentImageName, value);
            if(value != null)
            {
                _ = GetImageAsync(value);
            }
        }
    }

    public List<string> FilteredImageNames
    {
        get => _imageNames;
        set => this.RaiseAndSetIfChanged(ref _filteredImageNames, value);
    }

    public Bitmap Image
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

    public PhotoViewModel(ImageApiService imageApiService, IScreen screen, FilesService filesService)
    {
        _imageApiService = imageApiService;
        _filesService = filesService;
        HostScreen = screen;

        _ = GetImagesInfoAsync();
    }

    public async Task SendImageAsync()
    {
        var file = await _filesService.OpenImageFileAsync();
        if (file != null)
        {
            string name = file.Name;

            var memoryStream = new MemoryStream();
            Stream stream = await file.OpenReadAsync();
            stream.CopyTo(memoryStream);

            byte[] imageData = memoryStream.ToArray();

            if (HostScreen is MainWindowViewModel mainWindowViewModel)
            {
                SendImageDTO sendImageDTO = new SendImageDTO
                {
                    Name = name,
                    ImageData = imageData,
                    UploadDate = System.DateTime.UtcNow,
                    CategoryName = "ВУЦ",
                    Login = mainWindowViewModel.UserName,
                    Password = mainWindowViewModel.Password
                };

                await _imageApiService.SendImageAsync(sendImageDTO);

                await GetImagesInfoAsync();
            }
        }
    }

    public async Task GetImagesInfoAsync()
    {
        if (HostScreen is MainWindowViewModel mainWindowViewModel)
        {
            AccountDTO account = new AccountDTO
            {
                Login = mainWindowViewModel.UserName,
                Password = mainWindowViewModel.Password
            };
            _imagesInfo = await _imageApiService.GetImagesInfoAsync(account);
            if (_imagesInfo != null) InitCategoriesAndImageNames(_imagesInfo);
        }
    }

    private void InitCategoriesAndImageNames(List<ImageInfoDTO> imageInfoDTOs)
    {
        List<string> categories = [];
        List<string> imageNames = [];

        foreach(var imageDTO in  imageInfoDTOs)
        {
            if(!categories.Any(x => x == imageDTO.Category)) categories.Add(imageDTO.Category);
            imageNames.Add(imageDTO.ImageName);
        }

        Categories = categories;
        CurrentCategory = Categories.FirstOrDefault();
        ImageNames = imageNames;
    }

    private void FilterImageNames(string? category)
    {
        if (category != null)
        {
            List<string> imageNames = _imagesInfo.Where(x => x.Category == category).Select(x => x.ImageName).ToList();
            FilteredImageNames = imageNames;
        }
        else FilteredImageNames = [];
    }

    private async Task GetImageAsync(string imageName)
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
}
