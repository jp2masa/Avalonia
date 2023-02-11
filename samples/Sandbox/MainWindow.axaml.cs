using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Sandbox
{
    public sealed class MainWindowViewModel
    {
        public IBrush DemoBrush =>
            Brushes.DarkBlue;
    }

    public class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();

            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
