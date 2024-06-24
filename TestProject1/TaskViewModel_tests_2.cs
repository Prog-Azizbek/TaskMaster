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
    public class TaskViewModelTests2
    {
        [Fact]
        public void TestLoadData_TasksLoadedFromJson()
        {
            // Arrange
            var initialTasks = new ObservableCollection<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1" },
                new TaskItem { Id = 2, Title = "Task 2" }
            };
            var json = JsonConvert.SerializeObject(initialTasks);
            File.WriteAllText("tasks.json", json);

            // Act
            var viewModel = new TaskViewModel();

            // Assert
            Assert.Equal(2, viewModel.Tasks.Count);
            Assert.Equal("Task 1", viewModel.Tasks[0].Title);
            Assert.Equal("Task 2", viewModel.Tasks[1].Title);

            // Clean up
            File.Delete("tasks.json");
        }

        [Fact]
        public void TestLoadData_CategoriesLoadedFromJson()
        {
            // Arrange
            var initialCategories = new ObservableCollection<Category>
            {
                new Category { Name = "Category 1" },
                new Category { Name = "Category 2" }
            };
            var json = JsonConvert.SerializeObject(initialCategories);
            File.WriteAllText("categories.json", json);

            // Act
            var viewModel = new TaskViewModel();

            // Assert
            Assert.Equal(2, viewModel.Collections.Count);
            Assert.Equal("Category 1", viewModel.Collections[0].Name);
            Assert.Equal("Category 2", viewModel.Collections[1].Name);

            // Clean up
            File.Delete("categories.json");
        }

        [Fact]
        public void TestLoadData_NoDataFile_EmptyCollections()
        {
            // Arrange: удаление файлов данных, чтобы убедиться, что загрузятся пустые коллекции
            File.Delete("tasks.json");
            File.Delete("categories.json");

            // Act
            var viewModel = new TaskViewModel();

            // Assert
            Assert.Empty(viewModel.Tasks);
            Assert.Empty(viewModel.Collections);
        }

    }
}