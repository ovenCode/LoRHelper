using desktop.Services;
using desktop.ViewModels;
using Microsoft.VisualStudio.Threading;

namespace desktop.Commands
{
    public class StartGameCommand : AsyncCommandBase
    {
        private readonly InGameService _inGameService;
        private readonly ILoadingService _loadingService;

        private readonly JoinableTaskFactory _taskFactory;

        public StartGameCommand(
            InGameService inGameService,
            ILoadingService loadingService,
            JoinableTaskFactory taskFactory
        )
        {
            _inGameService = inGameService;
            _loadingService = loadingService;
            _taskFactory = taskFactory;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_loadingService == null)
            {
                throw new ArgumentNullException("Cannot run the loading service");
            }
            await _loadingService.ShowWhileAsync(
                async (token) =>
                    await _inGameService.StartAsync(
                        taskFactory: _taskFactory,
                        cancellationToken: token
                    )
            );
        }
    }
}
