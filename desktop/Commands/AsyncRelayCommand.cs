using System;
using System.Threading.Tasks;

namespace desktop.Commands
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<object?, Task> _callback;

        public AsyncRelayCommand(Func<object?, Task> callback)
            : this(callback, (ex) => CustomMessageBox.Show(ex.Message)) { }

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
