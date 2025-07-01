using System;
using System.Threading.Tasks;
using desktop.Services;

namespace desktop.Commands
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<object?, Task> _callback;

        public AsyncRelayCommand(Func<object?, Task> callback)
            : this(callback, async (ex) =>
            {
                try
                {
                    await CustomMessageBox.ShowAsync(ex.Message);
                }
                catch (System.Exception exception)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine(exception.Message);
                    throw;
                }
            }) { }

        public AsyncRelayCommand(
            Func<object?, Task> callback,
            Action<Exception> onException,
            Predicate<object?>? canExecute = null
        )
            : base(onException, canExecute)
        {
            _callback = callback;
        }

        public override async Task ExecuteAsync(object? parameter) => await _callback(parameter);
    }
}
