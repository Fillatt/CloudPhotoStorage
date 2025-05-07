using Avalonia.Collections;
using CloudPhotoStorage.UI.APIClient.DTO;
using CloudPhotoStorage.UI.APIClient.Services;
using ReactiveUI;
using System.Collections.Generic;

namespace CloudPhotoStorage.UI.ViewModels;

public class PhotoViewModel : ViewModelBase
{
    private ImageApiService _imageApiService;

    private AvaloniaList<ImageInfoDTO> _imageInfoDTO;

    private ImageInfoDTO _selectedImageInfo;

    public AvaloniaList<ImageInfoDTO> ImagesInfo
    {
        get => _imageInfoDTO;
        set => this.RaiseAndSetIfChanged(ref _imageInfoDTO, value);
    }

    public ImageInfoDTO SelectedImageInfo 
    { 
        get => _selectedImageInfo; 
        set => this.RaiseAndSetIfChanged(ref _selectedImageInfo, value); 
    }

    public PhotoViewModel(ImageApiService imageApiService)
    {
        _imageApiService = imageApiService;

        Dictionary<string, string> dictionary = _imageApiService.GetImagesInfoAsync().Result;

    }

}
