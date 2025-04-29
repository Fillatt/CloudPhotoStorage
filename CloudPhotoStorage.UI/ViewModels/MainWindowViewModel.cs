using CloudPhotoStorage.UI.Sections;
using ReactiveUI;
using System.Collections.Generic;

namespace CloudPhotoStorage.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IMenuSection? _currentTab;

        public IEnumerable<IMenuSection> Tabs { get; }

        public IMenuSection? CurrentTab
        {
            get => _currentTab;
            set => this.RaiseAndSetIfChanged(ref _currentTab, value);
        }

        public MainWindowViewModel(IEnumerable<IMenuSection> sections)
        {
            Tabs = sections;
        }
    }
}
