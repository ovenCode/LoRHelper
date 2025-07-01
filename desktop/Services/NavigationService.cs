using System;
using System.Threading.Tasks;
using System.Windows;
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

        public async Task<bool?> NavigateAsync(CancellationTokenSource? cancellationToken = null)
        {
            try
            {
                //_loadingStore.IsLoading = true;
                _navigationStore.CurrentViewModel = _createViewModel();
                //_loadingStore.IsLoading = false;
                return true;
            }
            catch (System.Exception)
            {
                if (cancellationToken != null)
                {
                    await cancellationToken.CancelAsync();
                }
            }
            return null;
        }
    }
}
