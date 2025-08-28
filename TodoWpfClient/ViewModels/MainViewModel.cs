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
        private readonly TodoApiClient _apiClient;

        private ObservableCollection<TodoItemViewModel> _todoItemViewModels;
        public ObservableCollection<TodoItemViewModel> TodoItemViewModels
        {
            get => _todoItemViewModels;
            set
            {
                _todoItemViewModels = value;
                OnPropertyChanged();
            }
        }

        private TodoItemViewModel _selectedTodoItemViewModel;
        public TodoItemViewModel SelectedTodoItemViewModel
        {
            get => _selectedTodoItemViewModel;
            set
            {
                _selectedTodoItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTodoCommand { get; }

        public MainViewModel(IEnumerable<TodoItemViewModel> items, TodoApiClient apiClient)
        {
            _todoItemViewModels = new ObservableCollection<TodoItemViewModel>(items);
            _apiClient = apiClient;
            SelectedTodoItemViewModel = _todoItemViewModels.FirstOrDefault();

            AddTodoCommand = new RelayCommand(ExecuteAddTodo);

            foreach (TodoItemViewModel item in _todoItemViewModels)
            {
                if (item != null)
                {
                    SubscribeToTodoItemChanges(item);
                }
            }
        }

        private async void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TodoItemViewModel todoVm)
            {
                if (e.PropertyName == nameof(TodoItemViewModel.IsDone) && todoVm.TodoItem.IsDone)
                {
                    await HandleTodoItemCompleted(todoVm);
                }
            }
        }

        private async Task HandleTodoItemCompleted(TodoItemViewModel todoItem)
        {
            try
            {
                await _apiClient.MarkAsDone(todoItem.TodoItem.Id);
                todoItem.PropertyChanged -= Item_PropertyChanged;

                TodoItemViewModels.Remove(todoItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update todo item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                todoItem.TodoItem.IsDone = false;
            }
        }

        private void ExecuteAddTodo()
        {
            Window dialog = new Window
            {
                Title = "Add New Todo",
                Content = new NewTodoControl(),
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize
            };

            NewTodoControl newTodoControl = (NewTodoControl)dialog.Content;
            newTodoControl.TodoItemCreated += async (s, todoItem) =>
            {
                dialog.DialogResult = true;
                dialog.Close();

                TodoItem added = await _apiClient.AddTodoItem(todoItem);
                TodoItemViewModel vm = new TodoItemViewModel(added);
                SubscribeToTodoItemChanges(vm);
                TodoItemViewModels.Add(vm);

                TodoItemViewModels = new ObservableCollection<TodoItemViewModel>(TodoItemViewModels.OrderBy(t => t.TodoItem.Priority).ThenBy(t => t.TodoItem.CreatedAt));
            };

            newTodoControl.DialogCancelled += (s, args) =>
            {
                dialog.DialogResult = false;
                dialog.Close();
            };

            dialog.ShowDialog();
        }

        private void SubscribeToTodoItemChanges(TodoItemViewModel item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            item.PropertyChanged += Item_PropertyChanged;
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