using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Input;

namespace CloudPhotoStorage.UI.ViewModels;

public class RegistrationViewModel : ViewModelBase
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
    public List<string> AvailableRoles { get; } = new()
    {
        "Пользователь",
        "Администратор",
        "Модератор"
    };

    public ICommand RegisterCommand { get; }
    public ICommand BackCommand { get; }

    public RegistrationViewModel()
    {
        RegisterCommand = ReactiveCommand.Create(OnRegister);
        BackCommand = ReactiveCommand.Create(OnBack);
    }

    private void OnRegister()
    {
        // Логика регистрации
    }

    private void OnBack()
    {
        // Логика возврата
    }
}