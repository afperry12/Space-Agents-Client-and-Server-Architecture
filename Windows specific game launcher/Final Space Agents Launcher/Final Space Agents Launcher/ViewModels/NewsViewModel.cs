using Final_Space_Agents_Launcher.Commands;
using Final_Space_Agents_Launcher.Stores;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        public ICommand NavigateStoreCommand { get; }

        public NewsViewModel(NavigationStore navigationStore)
        {
            NavigateStoreCommand = new NavigateStoreCommand(navigationStore);
        }
    }
}
