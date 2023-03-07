using Final_Space_Agents_Launcher.Commands;
using Final_Space_Agents_Launcher.Stores;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        public ICommand NavigateNewsCommand { get; }

        public StoreViewModel(NavigationStore navigationStore)
        {
            NavigateNewsCommand = new NavigateNewsCommand(navigationStore);
        }
    }
}
