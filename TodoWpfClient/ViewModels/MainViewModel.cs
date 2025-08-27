using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoDAL;

namespace TodoWpfClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TodoItemViewModel> _todoItems;
        public ObservableCollection<TodoItemViewModel> TodoItems
        {
            get => _todoItems;
            set
            {
                _todoItems = value;
                OnPropertyChanged();
            }
        }

        private TodoItemViewModel? _selectedTodoItem;
        public TodoItemViewModel? SelectedTodoItem
        {
            get => _selectedTodoItem;
            set
            {
                _selectedTodoItem = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IEnumerable<TodoItemViewModel> items)
        {
            _todoItems = new ObservableCollection<TodoItemViewModel>(items);
            SelectedTodoItem = _todoItems.FirstOrDefault();
        }

        #region INotifyPropertyChanged Implementation
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}