using System;
using System.Threading.Tasks;
using desktop.Stores;
using desktop.ViewModels;

namespace desktop.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            try
            {
                _navigationStore.CurrentViewModel = _createViewModel();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Task NavigateAsync()
        {
            try
            {
                _navigationStore.CurrentViewModel = _createViewModel();
            }
            catch (System.Exception e)
            {
                return Task.FromException(e);
            }
            return Task.CompletedTask;
        }
    }
}
