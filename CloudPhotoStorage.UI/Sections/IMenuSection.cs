using Avalonia.Controls;

namespace CloudPhotoStorage.UI.Sections;

public interface IMenuSection
{
    string Name { get; }

    UserControl View { get; }
}
