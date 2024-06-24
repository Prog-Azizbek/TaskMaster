//ViewModels/TaskViewModel
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Newtonsoft.Json;
using TaskMaster.Models;

namespace TaskMaster.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private const string TasksFileName = "tasks.json";
        private const string CategoriesFileName = "categories.json";

        public ObservableCollection<TaskItem> Tasks { get; set; }
        public ObservableCollection<Category> Collections { get; set; }
        public ICollectionView FilteredTasks { get; set; }
        private string _filterCategory;

        public string FilterCategory
        {
            get => _filterCategory;
            set
            {
                _filterCategory = value;
                OnPropertyChanged(nameof(FilterCategory));
                RefreshFilteredTasks();
            }
        }

        public TaskViewModel()
        {
            // Загружаем данные из файлов JSON
            Tasks = LoadData<ObservableCollection<TaskItem>>(TasksFileName) ?? new ObservableCollection<TaskItem>();
            Collections = LoadData<ObservableCollection<Category>>(CategoriesFileName) ?? new ObservableCollection<Category>();

            // Если файлы не существуют или пустые, добавляем задачи и коллекции по умолчанию
            if (!Tasks.Any())
            {
                Tasks.Add(new TaskItem { Id = 1, Title = "Оплатить обучение в университете", Description = "Оплата обучения за 2024 год", DueDate = DateTime.Now.AddDays(1), Priority = "High", Category = "Финансы", IsCompleted = false });
                Tasks.Add(new TaskItem { Id = 2, Title = "Пройти курс по разработке приложений для ПК", Description = "", DueDate = DateTime.Now.AddDays(2), Priority = "Medium", Category = "Личные", IsCompleted = false });
            }

            if (!Collections.Any())
            {
                Collections.Add(new Category { Name = "Финансы" });
                Collections.Add(new Category { Name = "Личные" });
            }

            FilteredTasks = CollectionViewSource.GetDefaultView(Tasks);
            FilteredTasks.Filter = FilterTasks;
        }

        private bool FilterTasks(object item)
        {
            if (item is TaskItem taskItem)
            {
                if (string.IsNullOrEmpty(FilterCategory) || FilterCategory == "All Tasks")
                {
                    return true;
                }
                return taskItem.Category == FilterCategory || (FilterCategory == "Due Today" && taskItem.DueDate?.Date == DateTime.Now.Date);
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void RefreshFilteredTasks()
        {
            FilteredTasks.Refresh();
        }

        public void AddCategory(string categoryName)
        {
            var newCategory = new Category { Name = categoryName };
            Collections.Add(newCategory);
            SaveData(Collections, CategoriesFileName);
        }

        public void SaveData<T>(T data, string fileName)
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(fileName, jsonData);
        }

        public T LoadData<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                var jsonData = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            return default;
        }

        public void CreateTask(string title, string description, DateTime dueDate, string category, string priority)
        {
            var newTask = new TaskItem
            {
                Id = Tasks.Count + 1,
                Title = title,
                Description = description,
                DueDate = dueDate,
                Category = category,
                Priority = priority,
                IsCompleted = false
            };

            Tasks.Add(newTask);
            SaveData(Tasks, TasksFileName);
        }

    }

    public class Category
    {
        public string Name { get; set; }
    }
}
