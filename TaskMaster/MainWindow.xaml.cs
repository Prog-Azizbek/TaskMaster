
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskMaster.Models;
using TaskMaster.ViewModels;
using TaskMaster.Views;

namespace TaskMaster
{
    public partial class MainWindow : Window
    {
        public TaskViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new TaskViewModel();
            DataContext = ViewModel;
        }

        public void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow(ViewModel.Collections, ViewModel);
            if (addTaskWindow.ShowDialog() == true)
            {
                var newTask = addTaskWindow.NewTask;
                // Убедимся, что задача не добавляется дважды
                if (!ViewModel.Tasks.Contains(newTask))
                {
                    ViewModel.Tasks.Add(newTask);
                    ViewModel.SaveData(ViewModel.Tasks, "tasks.json");
                    MessageBox.Show("Новая задача добавлена успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public void RemoveCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = ViewModel.Tasks.Count - 1; i >= 0; i--)
            {
                if (ViewModel.Tasks[i].IsCompleted)
                {
                    ViewModel.Tasks.RemoveAt(i);
                }
            }
            ViewModel.SaveData(ViewModel.Tasks, "tasks.json");
        }

        private void FilterCategory_MouseDown(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                ViewModel.FilterCategory = textBlock.Tag.ToString();
            }
        }

        public void CreateCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow(ViewModel);
            if (addCategoryWindow.ShowDialog() == true)
            {
                string newCategory = addCategoryWindow.NewCategory;
                if (!string.IsNullOrEmpty(newCategory))
                {
                    ViewModel.Collections.Add(new Category { Name = newCategory });
                    ViewModel.SaveData(ViewModel.Collections, "categories.json");
                    MessageBox.Show("Новая коллекция добавлена успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void FilterCollection_MouseDown(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                ViewModel.FilterCategory = textBlock.Text;
            }
        }

        public void ToggleImportanceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.DataContext is TaskItem task)
            {
                if (task.Priority == "Low")
                {
                    task.Priority = "Medium";
                }
                else if (task.Priority == "Medium")
                {
                    task.Priority = "High";
                }
                else
                {
                    task.Priority = "Low";
                }
                ViewModel.SaveData(ViewModel.Tasks, "tasks.json");
            }
        }

        public void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryPanel.Visibility = Visibility.Visible;
        }

        public void AddCategoryConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = NewCategoryNameTextBox.Text;
            if (!string.IsNullOrEmpty(categoryName))
            {
                ViewModel.Collections.Add(new Category { Name = categoryName });
                ViewModel.SaveData(ViewModel.Collections, "categories.json");
                MessageBox.Show("Новая коллекция добавлена успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AddCategoryPanel.Visibility = Visibility.Collapsed;
                NewCategoryNameTextBox.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название категории.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddCategoryCancelButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryPanel.Visibility = Visibility.Collapsed;
            NewCategoryNameTextBox.Text = string.Empty;
        }

        public void EditTask_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TaskItem taskItem)
            {
                var editTaskWindow = new EditTaskWindow(taskItem, ViewModel.Collections, ViewModel);
                if (editTaskWindow.ShowDialog() == true)
                {
                    ViewModel.SaveData(ViewModel.Tasks, "tasks.json");
                    ViewModel.RefreshFilteredTasks();
                }
            }
        }
    }
}
