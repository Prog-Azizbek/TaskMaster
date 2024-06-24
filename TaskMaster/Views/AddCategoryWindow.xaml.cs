// Views/AddCategoryWindow.xaml.cs
using System;
using System.Windows;
using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class AddCategoryWindow : Window
    {
        public TaskViewModel ViewModel { get; set; }
        public string NewCategory { get; private set; }


        public AddCategoryWindow(TaskViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryNameTextBox.Text;
            if (!string.IsNullOrEmpty(categoryName))
            {
                ViewModel.AddCategory(categoryName);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название категории.");
            }
        }
    }
}