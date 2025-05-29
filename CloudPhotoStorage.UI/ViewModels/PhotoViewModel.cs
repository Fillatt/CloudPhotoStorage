using Avalonia.Collections;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public partial class PhotoViewModel : ViewModelBase, IRoutableViewModel
{
    private ImageApiService _imageApiService;

    private List<ImageInfoDTO> _imageInfoDTO;

    private ImageInfoDTO _selectedImageInfo;

    public List<ImageInfoDTO> ImagesInfo
    {
        get => _imageInfoDTO;
        set => this.RaiseAndSetIfChanged(ref _imageInfoDTO, value);
    }

    public ImageInfoDTO SelectedImageInfo 
    { 
        get => _selectedImageInfo; 
        set => this.RaiseAndSetIfChanged(ref _selectedImageInfo, value); 
    }

    public ReactiveCommand<Unit, Task> GetImagesCommand { get; }

    public string? UrlPathSegment => "PhotoViewModel";

    public IScreen HostScreen { get; }

    public PhotoViewModel(ImageApiService imageApiService, IScreen screen)
    {
        _imageApiService = imageApiService;
        HostScreen = screen;
        
        GetImagesCommand = ReactiveCommand.Create(GetImagesAsync);
    }

    public async Task GetImagesAsync()
    {
        Dictionary<string, string> dictionary = await _imageApiService.GetImagesInfoAsync();
    }
}
