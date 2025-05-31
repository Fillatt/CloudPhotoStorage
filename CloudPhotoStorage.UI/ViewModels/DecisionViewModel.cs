using Avalonia.Controls.ApplicationLifetimes;
using CloudPhotoStorage.UI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.ViewModels;

public class DecisionViewModel : ViewModelBase
{
    private DecisionDialogWindow? _dialogWindow;

    public string Message { get; }

    public bool IsOk { get; private set; }

    public ReactiveCommand<Unit, Unit> OkCommand { get; }

    public ReactiveCommand<Unit, Unit> CancelCommand { get; }


    public DecisionViewModel(string message)
    {
        Message = message;

        OkCommand = ReactiveCommand.Create(OnOk);
        CancelCommand = ReactiveCommand.Create(OnCancel);
    }

    public async Task ShowDialogAsync()
    {
        if(App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _dialogWindow = new DecisionDialogWindow() { DataContext = this };
            if(desktop.MainWindow != null) await _dialogWindow.ShowDialog(desktop.MainWindow);
        }
    }

    public void OnOk()
    {
        IsOk = true;
        _dialogWindow?.Close();
    }

    public void OnCancel()
    {
        IsOk = false;
        _dialogWindow?.Close();
    }
}
