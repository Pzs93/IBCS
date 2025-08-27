using System.Windows;
using TodoWpfClient.API;
using TodoWpfClient.ViewModels;
using TodoWpfClient.Controls;
using TodoDAL;

namespace TodoWpfClient;

public partial class MainWindow : Window
{
    private TodoApiClient _apiClient;

    private MainViewModel _viewModel;
    public MainViewModel ViewModel
    {
        get => _viewModel;
        set { _viewModel = value; }
    }

    public MainWindow()
    {
        _apiClient = new TodoApiClient("http://localhost:5000");
        IEnumerable<TodoItemViewModel> items = _apiClient.GetTodoItems().Result.Select(t => new TodoItemViewModel(t));
        _viewModel = new MainViewModel(items);

        InitializeComponent();
    }

    private void AddTodoButton_Click(object sender, RoutedEventArgs e)
    {
        Window dialog = new Window
        {
            Title = "Add New Todo",
            Content = new NewTodoControl(),
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Owner = this,
            ResizeMode = ResizeMode.NoResize
        };

        NewTodoControl newTodoControl = (NewTodoControl)dialog.Content;
        newTodoControl.TodoItemCreated += async (s, todoItem) =>
        {
            dialog.DialogResult = true;
            dialog.Close();

            TodoItem added = await _apiClient.AddTodoItem(todoItem);
            TodoItemViewModel vm = new TodoItemViewModel(added);
            ViewModel.TodoItems.Add(vm);
        };

        newTodoControl.DialogCancelled += (s, args) =>
        {
            dialog.DialogResult = false;
            dialog.Close();
        };

        dialog.ShowDialog();
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        foreach (TodoItemViewModel todoVm in ViewModel.TodoItems)
        {
            if (todoVm.TodoItem.IsDone)
            {
                await _apiClient.MarkAsDone(todoVm.TodoItem.Id);
            }
        }

        MessageBox.Show("Completed todos have been saved.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}