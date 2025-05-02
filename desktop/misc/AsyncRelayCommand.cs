using System;
using System.Threading.Tasks;

public class AsyncRelayCommand : AsyncCommandBase
{
    private readonly Func<object?,Task> _callback;

    public AsyncRelayCommand(Func<object?,Task> callback, Action<Exception> onException) : base(onException)
    {
        _callback = callback;
    }
    public override async Task ExecuteAsync(object? parameter) => await _callback(parameter);

}