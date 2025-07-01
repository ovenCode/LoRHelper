using System.ComponentModel;
using desktop.Stores;

namespace desktop.Services
{
    public class LoadingService : ILoadingService
    {
        private readonly ILoadingStore _loadingStore;

        public LoadingService(ILoadingStore loadingStore)
        {
            _loadingStore = loadingStore;
        }

        public void Hide()
        {
            _loadingStore.IsLoading = false;
            _loadingStore.LoadingMessage = string.Empty;
        }

        public void Show(string message = "Loading...")
        {
            _loadingStore.LoadingMessage = message;
            _loadingStore.IsLoading = true;
        }

        public async Task<T?> ShowWhileAsync<T>(
            Func<CancellationTokenSource?, Task<T>> work,
            string message = "Loading...",
            CancellationTokenSource? cancellationToken = null
        )
        {
            try
            {
                Show(message);
                return await work(cancellationToken);
            }
            catch (Exception)
            {
                if (cancellationToken != null)
                {
                    await cancellationToken.CancelAsync();
                }
                return default;
            }
            finally
            {
                Hide();
            }
        }

        public async Task ShowWhileAsync(
            Func<CancellationTokenSource?, Task> work,
            string message = "Loading...",
            CancellationTokenSource? cancellationToken = null
        )
        {
            try
            {
                Show(message);
                await work(cancellationToken);
            }
            finally
            {
                Hide();
            }
        }
    }
}
