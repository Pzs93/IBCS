using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoDAL;

namespace TodoWpfClient.ViewModels
{
    public class TodoItemViewModel : INotifyPropertyChanged
    {
        private TodoItem _todoItem;
        public TodoItem TodoItem
        {
            get => _todoItem;
            set
            {
                _todoItem = value;
                OnPropertyChanged();
            }
        }

  
        private bool _isDone;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                _todoItem.IsDone = value;
                OnPropertyChanged();
            }
        }

        public TodoItemViewModel(TodoItem item)
        {
            _todoItem = item;
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