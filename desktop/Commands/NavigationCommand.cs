using System.Threading.Tasks;
using System.Windows;
using desktop.Services;
using desktop.ViewModels;

namespace desktop.Commands
{
    public class NavigateCommand<TViewModel> : AsyncCommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _navigationService.NavigateAsync();
            }
            catch (System.Exception e)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox(e.Message);
                customMessageBox.Show();
            }
        }
    }
}
