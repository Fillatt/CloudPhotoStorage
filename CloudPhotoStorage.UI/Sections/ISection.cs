using Avalonia.Controls;

namespace CloudPhotoStorage.UI.Sections;

public interface ISection
{
    string Name { get; }

    UserControl Control { get; }
}
