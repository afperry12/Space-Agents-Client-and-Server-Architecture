using Final_Space_Agents_Launcher.Commands;
using Final_Space_Agents_Launcher.Stores;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand NavigateProfileCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateProfileCommand = new NavigateProfileCommand(navigationStore);
        }

    }
}
