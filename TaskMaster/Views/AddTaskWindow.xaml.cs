//Views/AddTaskWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TaskMaster.Models;
using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class AddTaskWindow : Window
    {
        public TaskItem NewTask { get; private set; }
        public ObservableCollection<Category> Collections { get; }
        private readonly TaskViewModel _taskViewModel;

        public AddTaskWindow(ObservableCollection<Category> collections, TaskViewModel taskViewModel)
        {
            InitializeComponent();
            Collections = collections;
            CategoryComboBox.ItemsSource = Collections;
            _taskViewModel = taskViewModel;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeSpan.TryParse(DueTimeTextBox.Text, out var dueTime))
            {
                var dueDate = DueDatePicker.SelectedDate ?? DateTime.Now;
                dueDate = dueDate.Date + dueTime;

                NewTask = new TaskItem
                {
                    Title = TitleTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    DueDate = dueDate,
                    DueTime = dueDate,
                    Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Category = (CategoryComboBox.SelectedItem as Category)?.Name,
                    IsCompleted = false
                };
                _taskViewModel.Tasks.Add(NewTask);
                _taskViewModel.SaveData(_taskViewModel.Tasks, "tasks.json");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста введите корректное время в формате ЧЧ:ММ.");
            }
        }
    }
}
