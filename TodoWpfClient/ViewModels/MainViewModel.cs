using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TodoDAL;
using TodoWpfClient.API;
using TodoWpfClient.Commands;
using TodoWpfClient.Controls;

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

        private readonly TodoApiClient _apiClient;

        public ICommand AddTodoCommand { get; }
        public ICommand SaveCommand { get; }

        public MainViewModel(IEnumerable<TodoItemViewModel> items, TodoApiClient apiClient)
        {
            _todoItems = new ObservableCollection<TodoItemViewModel>(items);
            _apiClient = apiClient;
            SelectedTodoItem = _todoItems.FirstOrDefault();

            AddTodoCommand = new RelayCommand(ExecuteAddTodo);
            SaveCommand = new RelayCommand(ExecuteSaveAsync);
        }

        private void ExecuteAddTodo()
        {
            var dialog = new Window
            {
                Title = "Add New Todo",
                Content = new NewTodoControl(),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize
            };

            var newTodoControl = (NewTodoControl)dialog.Content;
            newTodoControl.TodoItemCreated += async (s, todoItem) =>
            {
                dialog.DialogResult = true;
                dialog.Close();

                var added = await _apiClient.AddTodoItem(todoItem);
                var vm = new TodoItemViewModel(added);
                TodoItems.Add(vm);
            };

            newTodoControl.DialogCancelled += (s, args) =>
            {
                dialog.DialogResult = false;
                dialog.Close();
            };

            dialog.ShowDialog();
        }

        private async void ExecuteSaveAsync()
        {
            List<TodoItemViewModel> completedItems = TodoItems.Where(todoVm => todoVm.TodoItem.IsDone).ToList();
            
            foreach (var todoVm in completedItems)
            {
                await _apiClient.MarkAsDone(todoVm.TodoItem.Id);
                TodoItems.Remove(todoVm);
            }

            if (completedItems.Any())
            {
                MessageBox.Show($"{completedItems.Count} completed todos have been saved and removed.", 
                    "Save", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
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