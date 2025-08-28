using System.Windows;
using TodoWpfClient.API;
using TodoWpfClient.ViewModels;

namespace TodoWpfClient;

public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    public MainViewModel ViewModel
    {
        get => _viewModel;
        set { _viewModel = value; }
    }

    public MainWindow()
    {
        TodoApiClient apiClient = new("http://localhost:5005");
        IEnumerable<TodoItemViewModel> items = apiClient.GetTodoItems().Result.Select(t => new TodoItemViewModel(t));

        _viewModel = new MainViewModel(items, apiClient);

        InitializeComponent();

        DataContext = _viewModel;
    }
}