using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using desktop.Services;
using desktop.ViewModels;
using Microsoft.VisualStudio.Threading;

namespace desktop.Commands
{
    public class NavigateCommand<TViewModel> : AsyncCommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;
        private readonly JoinableTaskFactory joinableTaskFactory;
        private readonly ILoadingService? _loadingService;
        private readonly CancellationTokenSource? cancellationToken;

        /// <summary>
        /// Creates a <c>NavigationCommand</c> instance
        /// </summary>
        /// <param name="navigationService">The service used for navigation</param>
        /// <param name="loadingService">A loading service instance</param>
        /// <param name="tokenSource">A token source allowing cancellation</param>
        public NavigateCommand(
            NavigationService<TViewModel> navigationService,
            JoinableTaskFactory taskFactory,
            ILoadingService? loadingService = null,
            CancellationTokenSource? tokenSource = null
        )
        {
            _navigationService = navigationService;
            _loadingService = loadingService;
            joinableTaskFactory = taskFactory;
            cancellationToken = tokenSource;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (
                    _loadingService != null
                    && parameter is Func<CancellationTokenSource?, Task> work
                )
                {
                    await _loadingService.ShowWhileAsync(
                        work: work,
                        cancellationToken: cancellationToken
                    );
                    return;
                }
                await _navigationService.NavigateAsync();
                Window window = Application.Current.MainWindow;
                window.SizeToContent = SizeToContent.WidthAndHeight;
                window.InvalidateMeasure();
                window.UpdateLayout();
            }
            catch (System.Exception e)
            {
                await CustomMessageBox.ShowAsync(e.Message);
            }
        }
    }
}
