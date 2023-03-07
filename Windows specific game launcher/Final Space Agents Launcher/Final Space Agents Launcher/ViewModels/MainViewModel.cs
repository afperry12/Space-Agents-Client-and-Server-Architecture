using Final_Space_Agents_Launcher.Commands;
using Final_Space_Agents_Launcher.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Final_Space_Agents_Launcher.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand NavigateHomeCommand { get; }
        
        public ICommand NavigateNewsCommand { get; }
        
        public ICommand NavigateStoreCommand { get; }

        public ICommand NavigateProfileCommand { get; }

        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateHomeCommand(navigationStore);

            NavigateNewsCommand = new NavigateNewsCommand(navigationStore);

            NavigateStoreCommand = new NavigateStoreCommand(navigationStore);

            NavigateProfileCommand = new NavigateProfileCommand(navigationStore);

            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
