using System;
using System.Threading.Tasks;
using System.Windows.Input;

public abstract class AsyncCommandBase : ICommand
{
    private readonly Action<Exception> _onException;

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

    public AsyncCommandBase(Action<Exception> onException)
    {
        _onException = onException;
    }

    public bool CanExecute(object? parameter)
    {
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
