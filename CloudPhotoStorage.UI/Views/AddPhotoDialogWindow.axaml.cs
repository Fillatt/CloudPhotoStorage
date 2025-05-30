using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CloudPhotoStorage.UI.ViewModels;

namespace CloudPhotoStorage.UI.Views;

public partial class AddPhotoDialogWindow : Window
{
    public AddPhotoDialogWindow()
    {
        InitializeComponent();
    }

    private void ListBox_Tapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if(DataContext is AddPhotoDialogViewModel viewModel)
        {
            viewModel.OnCategotySelected();
        }
    }
}