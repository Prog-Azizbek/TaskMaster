using System;
using System.IO;
using System.Linq;
using Xunit;
using TaskMaster.ViewModels;
using TaskMaster.Models;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace TaskMaster.Tests
{
    public class TaskViewModelTests3
    {
        [Fact]
        public void TestFilterTasks_AllTasksFilter()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.FilterCategory = "All Tasks";

            // Assert
            Assert.Equal(2, viewModel.FilteredTasks.Cast<TaskItem>().Count());
        }

        [Fact]
        public void TestFilterTasks_ByCategory()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.FilterCategory = "Финансы";

            // Assert
            Assert.Single(viewModel.FilteredTasks.Cast<TaskItem>());
            Assert.Equal("Финансы", viewModel.FilteredTasks.Cast<TaskItem>().First().Category);
        }

        [Fact]
        public void TestFilterTasks_DueToday()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.CreateTask("Task Today", "Description", DateTime.Now.Date, "Work", "Medium");

            viewModel.FilterCategory = "Due Today";

            // Assert
            Assert.Single(viewModel.FilteredTasks.Cast<TaskItem>());
            Assert.Equal(DateTime.Now.Date, viewModel.FilteredTasks.Cast<TaskItem>().First().DueDate?.Date);
        }

        [Fact]
        public void TestAddCategory_NewCategoryAdded()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            var initialCount = viewModel.Collections.Count;

            // Act
            viewModel.AddCategory("New Category");

            // Assert
            Assert.Equal(initialCount + 1, viewModel.Collections.Count);
            Assert.Contains(viewModel.Collections, c => c.Name == "New Category");
        }

        [Fact]
        public void TestSaveData_TasksSavedToJson()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.Tasks.Clear();
            viewModel.Tasks.Add(new TaskItem { Id = 1, Title = "Task 1" });
            viewModel.Tasks.Add(new TaskItem { Id = 2, Title = "Task 2" });

            // Act
            viewModel.SaveData(viewModel.Tasks, "test_tasks.json");

            // Assert
            var json = File.ReadAllText("test_tasks.json");
            var tasksFromJson = JsonConvert.DeserializeObject<ObservableCollection<TaskItem>>(json);
            Assert.Equal(2, tasksFromJson.Count);
            Assert.Equal("Task 1", tasksFromJson[0].Title);
            Assert.Equal("Task 2", tasksFromJson[1].Title);

            // Clean up
            File.Delete("test_tasks.json");
        }

        [Fact]
        public void TestSaveData_CategoriesSavedToJson()
        {
            // Arrange
            var viewModel = new TaskViewModel();
            viewModel.Collections.Clear();
            viewModel.Collections.Add(new Category { Name = "Category 1" });
            viewModel.Collections.Add(new Category { Name = "Category 2" });

            // Act
            viewModel.SaveData(viewModel.Collections, "test_categories.json");

            // Assert
            var json = File.ReadAllText("test_categories.json");
            var categoriesFromJson = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);
            Assert.Equal(2, categoriesFromJson.Count);
            Assert.Equal("Category 1", categoriesFromJson[0].Name);
            Assert.Equal("Category 2", categoriesFromJson[1].Name);

            // Clean up
            File.Delete("test_categories.json");
        }

    }
}