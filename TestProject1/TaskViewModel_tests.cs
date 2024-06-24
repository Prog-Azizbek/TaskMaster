using System;
using System.IO;
using System.Linq;
using Xunit;
using TaskMaster.ViewModels;
using TaskMaster.Models;
using System.Collections.ObjectModel;

namespace TaskMaster.Tests
{
    public class TaskViewModelTests
    {
        [Fact]
        public void TestCreateTask_ValidData_TaskCreated()
        {
            // Arrange
            var viewModel = new TaskViewModel();

            // Act
            viewModel.CreateTask("New Task", "Description", DateTime.Now, "Category", "High");

            // Assert
            Assert.Equal(1, viewModel.Tasks.Count);
            Assert.Equal("New Task", viewModel.Tasks[0].Title);
        }

        [Fact]
        public void TestCreateTask_InvalidData_TaskNotCreated()
        {
            // Arrange
            var viewModel = new TaskViewModel();

            // Act
            viewModel.CreateTask("", "Description", DateTime.Now, "Category", "High");

            // Assert
            Assert.Empty(viewModel.Tasks);
        }

        [Fact]
        public void TestFilterTasks_ByCategory_Filtered()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.CreateTask("Task1", "Description", DateTime.Now, "Work", "High");
            viewModel.CreateTask("Task2", "Description", DateTime.Now, "Personal", "Low");

            // Act
            viewModel.FilterCategory = "Work";

            // Assert
            Assert.Single(viewModel.FilteredTasks.Cast<TaskItem>());
            Assert.Equal("Work", viewModel.FilteredTasks.Cast<TaskItem>().First().Category);
        }

        [Fact]
        public void TestAddCategory_CategoryAdded()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            string newCategoryName = "Shopping";

            // Act
            viewModel.AddCategory(newCategoryName);

            // Assert
            Assert.Contains(viewModel.Collections, c => c.Name == newCategoryName);
        }

        [Fact]
        public void TestSaveAndLoadData_DataSavedAndLoadedCorrectly()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.CreateTask("Task1", "Description", DateTime.Now, "Work", "High");

            // Act
            viewModel.SaveData(viewModel.Tasks, "test_tasks.json");
            var loadedTasks = viewModel.LoadData<ObservableCollection<TaskItem>>("test_tasks.json");

            // Assert
            Assert.NotNull(loadedTasks);
            Assert.Single(loadedTasks);
            Assert.Equal("Task1", loadedTasks[0].Title);

            // Clean up
            File.Delete("test_tasks.json");
        }


    }
}
