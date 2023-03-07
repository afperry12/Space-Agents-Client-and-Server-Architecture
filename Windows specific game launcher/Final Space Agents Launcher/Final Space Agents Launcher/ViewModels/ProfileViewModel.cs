using Final_Space_Agents_Launcher.Commands;
using Final_Space_Agents_Launcher.Stores;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; }

        public ProfileViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);
        }
    }
}
