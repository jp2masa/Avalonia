using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Sandbox
{
    public sealed class ExampleInfo
    {
        public ExampleInfo(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }

    public sealed class Category
    {
        public Category(string key, List<ExampleInfo> examples)
        {
            Key = key;
            Examples = examples ?? throw new ArgumentNullException(nameof(examples));
        }

        public string Key { get; }

        public List<ExampleInfo> Examples { get; }
    }

    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private ExampleInfo _example;

        public MainWindowViewModel(List<Category> categories)
        {
            Categories = categories;
        }

        public List<Category> Categories { get; }

        public ExampleInfo Example
        {
            get => _example;
            set => SetField(ref _example, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel(
                new List<Category>()
                {
                    new ("A", new List<ExampleInfo>() { new("1"), new("2"), new("3") }),
                    new ("B", new List<ExampleInfo>() { new("1"), new("2"), new("3") }),
                    new ("C", new List<ExampleInfo>() { new("1"), new("2"), new("3") }),
                    new ("D", new List<ExampleInfo>() { new("1"), new("2"), new("3") }),
                }
            );

            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
