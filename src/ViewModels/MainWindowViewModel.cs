using ReactiveUI;

namespace GradeManagement.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _content = null!;
        
        public MainWindowViewModel()
        {
            Content = new YearSelectorViewModel();
        }
        
        internal ViewModelBase Content
        {
            get => _content;
            private set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        internal void SwitchPage<T>() where T : ViewModelBase, new()
        {
            Content = new T();
        }
    }
}