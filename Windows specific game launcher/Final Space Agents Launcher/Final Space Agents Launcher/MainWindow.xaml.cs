using System;
using System.Windows;
using System.Windows.Controls;
using Final_Space_Agents_Launcher.LauncherData;
using System.IO.Compression;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Final_Space_Agents_Launcher.ViewModels;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button MinimizeButton;
        public Button MaximizeButton;
        public Button CloseButton;
        public Button SystemMenu;
        public MainWindow()
        {
            InitializeComponent();
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            this.DataContext = this;


        }

        private void Minimize_Initialized(object sender, EventArgs e)
        {
            MinimizeButton = (Button)sender;
        }

        private void Maximize_Initialized(object sender, EventArgs e)
        {
            MaximizeButton = (Button)sender;
        }

        private void Close_Initialized(object sender, EventArgs e)
        {
            CloseButton = (Button)sender;
        }

        private void SystemMenuInitialized(object sender, EventArgs e)
        {
            SystemMenu = (Button)sender;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.WindowState ^= WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Close();
        }

        private void SystemMenuClick(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            SystemCommands.ShowSystemMenu(window, GetMousePosition(window));
        }

        private Point GetMousePosition(Window window)
        {
            var position = Mouse.GetPosition(window);

            return new Point(position.X + window.Left, position.Y + window.Top);
        }

    }
}
