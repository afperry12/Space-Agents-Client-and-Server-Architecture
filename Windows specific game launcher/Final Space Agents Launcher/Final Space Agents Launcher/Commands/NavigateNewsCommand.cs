using Final_Space_Agents_Launcher.Stores;
using Final_Space_Agents_Launcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Space_Agents_Launcher.Commands
{
    public class NavigateNewsCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateNewsCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new NewsViewModel(_navigationStore);
        }
    }
}
