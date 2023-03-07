using System.Windows;
using Final_Space_Agents_Launcher.Stores;
using Final_Space_Agents_Launcher.ViewModels;

namespace Final_Space_Agents_Launcher
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

    }
}
