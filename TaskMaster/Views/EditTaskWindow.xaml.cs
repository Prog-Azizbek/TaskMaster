//Views/EditTaskWindow.xaml.cs
using System;
using System.Linq;
using System.Windows;
using TaskMaster.Models;
using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class EditTaskWindow : Window
    {
        private readonly TaskViewModel _taskViewModel;

        public TaskItem Task { get; private set; }

        public EditTaskWindow(TaskItem task, System.Collections.ObjectModel.ObservableCollection<Category> collections, TaskViewModel taskViewModel)
        {
            InitializeComponent();
            Task = task;
            TitleTextBox.Text = Task.Title;
            DescriptionTextBox.Text = Task.Description;
            DueDatePicker.SelectedDate = Task.DueDate;
            DueTimeTextBox.Text = Task.DueTime.HasValue ? Task.DueTime.Value.ToString("HH:mm") : string.Empty; // Преобразуем DateTime в строку
            PriorityComboBox.SelectedItem = Task.Priority;
            CategoryComboBox.ItemsSource = collections;
            CategoryComboBox.SelectedItem = collections.FirstOrDefault(c => c.Name == Task.Category);
            _taskViewModel = taskViewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Title = TitleTextBox.Text;
            Task.Description = DescriptionTextBox.Text;
            Task.DueDate = DueDatePicker.SelectedDate.GetValueOrDefault(); // Получаем значение или значение по умолчанию
            Task.DueTime = DateTime.TryParse(DueTimeTextBox.Text, out var dueTime) ? (DateTime?)dueTime : null; // Преобразуем строку в DateTime
            Task.Priority = PriorityComboBox.SelectedItem?.ToString();
            if (CategoryComboBox.SelectedItem != null)
            {
                Task.Category = (CategoryComboBox.SelectedItem as Category).Name;
            }

            _taskViewModel.SaveData(_taskViewModel.Tasks, "tasks.json");

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
