using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using desktop.Commands;
using Discord;

namespace desktop.ViewModels
{
    public class CustomMessageBoxViewModel : ViewModelBase
    {
        private string? message;

        public string? Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private Window? _window;

        public ICommand? OkCommand { get; }
        public ICommand? CancelCommand { get; }
        public ICommand? YesCommand { get; }
        public ICommand? NoCommand { get; }

        private readonly TaskCompletionSource<MessageBoxResult> _messageBoxResult;
        public Task<MessageBoxResult> Result => _messageBoxResult.Task;

        public CustomMessageBoxViewModel(
            string message,
            MessageBoxType messageBoxType,
            Func<object?, Task>? okCallback = null,
            Func<object?, Task>? cancelCallback = null,
            Action<Exception>? exceptionCallback = null
        )
        {
            Message = message;
            _messageBoxResult = new TaskCompletionSource<MessageBoxResult>();
            if (okCallback != null && cancelCallback != null)
            {
                OkCommand = new AsyncRelayCommand(okCallback, exceptionCallback ?? HandleException);
                CancelCommand = new AsyncRelayCommand(cancelCallback, exceptionCallback ?? HandleException);
            }
            else
            {
                OkCommand = messageBoxType switch
                {
                    MessageBoxType.YesNo => null,
                    _ => new AsyncRelayCommand((param) => SetResultAsync(MessageBoxResult.OK), exceptionCallback ?? HandleException)                    
                };
                CancelCommand = messageBoxType switch
                {
                    MessageBoxType.Ok => null,
                    MessageBoxType.YesNo => null,
                    _ => new AsyncRelayCommand((param) => SetResultAsync(MessageBoxResult.Cancel), exceptionCallback ?? HandleException)                    
                };
                YesCommand = messageBoxType switch
                {
                    MessageBoxType.Ok => null,
                    MessageBoxType.OkCancel => null,
                    _ => new AsyncRelayCommand((param) => SetResultAsync(MessageBoxResult.Yes), exceptionCallback ?? HandleException)                    
                };
                NoCommand = messageBoxType switch
                {
                    MessageBoxType.Ok => null,
                    MessageBoxType.OkCancel => null,
                    _ => new AsyncRelayCommand((param) => SetResultAsync(MessageBoxResult.No), exceptionCallback ?? HandleException)                    
                };
            }
        }

        public void SetWindow(Window window) => _window = window;

        private Task SetResultAsync(MessageBoxResult result)
        {
            try
            {
                _messageBoxResult.TrySetResult(result);
                //if(_window != null) _window.DialogResult = result;
                _window?.Close();
                return Task.CompletedTask;
            }
            catch (System.Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private void HandleException(Exception exception)
        {
            try
            {
                Trace.WriteLine(exception.Message);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                //return Task.FromException(ex);
            }
            //return Task.CompletedTask;
        }
    }

    public enum MessageBoxType
    {
        Ok,
        OkCancel,
        YesNo
    }
}