using CloudPhotoStorage.UI.Sections;
using ReactiveUI;
using System.Collections.Generic;

namespace CloudPhotoStorage.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ISection? _currentTab;

        public IEnumerable<ISection> Tabs { get; }

        public ISection? CurrentTab
        {
            get => _currentTab;
            set => this.RaiseAndSetIfChanged(ref _currentTab, value);
        }

        public MainWindowViewModel(IEnumerable<ISection> sections)
        {
            Tabs = sections;
        }
    }
}
