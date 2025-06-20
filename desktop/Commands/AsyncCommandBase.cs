using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace desktop.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly Action<Exception>? _onException;

        private bool isExecuting;
        public bool IsExecuting
        {
            get { return isExecuting; }
            set
            {
                isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler? CanExecuteChanged;
        private readonly Predicate<object?>? _canExecute;

        public AsyncCommandBase(
            Action<Exception>? onException = null,
            Predicate<object?>? canExecute = null
        )
        {
            _onException = onException;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (!isExecuting)
            {
                return _canExecute == null || _canExecute(parameter);
            }
            return !IsExecuting;
        }

        public async void Execute(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (System.Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        public abstract Task ExecuteAsync(object? parameter);
    }
}
