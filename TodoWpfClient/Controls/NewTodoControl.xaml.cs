using System;
using System.Windows;
using System.Windows.Controls;
using TodoDAL;

namespace TodoWpfClient.Controls
{
    public partial class NewTodoControl : UserControl
    {
        public event EventHandler<TodoItem>? TodoItemCreated;
        public event EventHandler? DialogCancelled;

        public NewTodoControl()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the todo item.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TodoItem todoItem = new TodoItem
            {
                Name = NameTextBox.Text.Trim(),
                Description = DescriptionTextBox.Text.Trim(),
                Priority = (byte)(PriorityComboBox.SelectedIndex),
                CreatedAt = DateTime.Now,
                IsDone = false
            };

            TodoItemCreated?.Invoke(this, todoItem);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogCancelled?.Invoke(this, EventArgs.Empty);
        }

        public void ClearInputs()
        {
            NameTextBox.Clear();
            DescriptionTextBox.Clear();
            PriorityComboBox.SelectedIndex = 0;
        }
    }
}