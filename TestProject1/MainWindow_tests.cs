using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xunit;
using TaskMaster.Models;
using TaskMaster.ViewModels;
using TaskMaster.Views;
using TaskMaster;

namespace TestProject1
{
    public class MainWindowTests
    {
        [Fact]
        public void TestNewTaskButton_Click_AddNewTask()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            var mainWindow = new MainWindow();
            mainWindow.ViewModel = viewModel;

            // Act
            mainWindow.NewTaskButton_Click(null, null);

            // Assert
            Assert.Single(viewModel.Tasks); // Проверяем, что задача была добавлена
            Assert.True(File.Exists("tasks.json")); // Проверяем, что данные сохранены в JSON файл
        }

        [Fact]
        public void TestRemoveCompleteButton_Click_RemoveCompletedTasks()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.Tasks.Add(new TaskItem { Id = 1, Title = "Task 1", IsCompleted = true });
            viewModel.Tasks.Add(new TaskItem { Id = 2, Title = "Task 2", IsCompleted = false });
            var mainWindow = new MainWindow();
            mainWindow.ViewModel = viewModel;

            // Act
            mainWindow.RemoveCompleteButton_Click(null, null);

            // Assert
            Assert.Single(viewModel.Tasks); // Проверяем, что завершенная задача была удалена
            Assert.False(viewModel.Tasks.Any(t => t.IsCompleted)); // Проверяем, что нет завершенных задач
            Assert.True(File.Exists("tasks.json")); // Проверяем, что данные сохранены в JSON файл
        }

        [Fact]
        public void TestCreateCollectionButton_Click_AddNewCategory()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            var mainWindow = new MainWindow();
            mainWindow.ViewModel = viewModel;

            // Act
            mainWindow.CreateCollectionButton_Click(null, null);

            // Assert
            Assert.Single(viewModel.Collections); // Проверяем, что категория была добавлена
            Assert.True(File.Exists("categories.json")); // Проверяем, что данные сохранены в JSON файл
        }

        [Fact]
        public void TestToggleImportanceButton_Click_ToggleTaskPriority()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.Tasks.Add(new TaskItem { Id = 1, Title = "Task 1", Priority = "Low" });
            var mainWindow = new MainWindow();
            mainWindow.ViewModel = viewModel;

            // Act
            mainWindow.ToggleImportanceButton_Click(new Button { DataContext = viewModel.Tasks[0] }, null);

            // Assert
            Assert.Equal("Medium", viewModel.Tasks[0].Priority); // Проверяем, что приоритет изменен
            Assert.True(File.Exists("tasks.json")); // Проверяем, что данные сохранены в JSON файл
        }
    }
}
