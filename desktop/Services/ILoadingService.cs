using System.ComponentModel;

namespace desktop.Services
{
    public interface ILoadingService
    {
        void Show(string message = "Loading...");
        void Hide();

        Task<T?> ShowWhileAsync<T>(
            Func<CancellationTokenSource?, Task<T>> work,
            string message = "Loading...",
            CancellationTokenSource? cancellationToken = null
        );
        Task ShowWhileAsync(
            Func<CancellationTokenSource?, Task> work,
            string message = "Loading...",
            CancellationTokenSource? cancellationToken = null
        );
    }
}
