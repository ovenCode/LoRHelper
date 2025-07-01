using System.Windows.Input;
using desktop.Commands;
using desktop.Services;
using desktop.Stores;
using desktop.ViewModels;
using Microsoft.VisualStudio.Threading;
using Moq;

namespace LoR_Helper_UnitTests;

public class GlobalMessagingTests
{
    public GlobalMessagingTests() { }

    /// <summary>
    /// Test checking whether StatusMessage is displayed on error caught
    /// </summary>
    [Fact]
    public void StatusMessageErrorCaught_HasMessage_True()
    {
        GlobalMessagingStore messagingStore = new GlobalMessagingStore();
        FakeViewModel viewModel = new FakeViewModel(messagingStore);
        viewModel.AddNewCommand?.Execute(null);

        Assert.True(!string.IsNullOrEmpty(messagingStore.CurrentStatusMessage));
    }

    [Fact]
    public void StatusMessageNoError_HasMessage_False()
    {
        GlobalMessagingStore messagingStore = new GlobalMessagingStore();
        FakeViewModel viewModel = new FakeViewModel(messagingStore);
        viewModel.ShowSomethingCommand?.Execute(null);

        Assert.False(!string.IsNullOrEmpty(messagingStore.CurrentStatusMessage));
    }

    private class FakeViewModel
    {
        private readonly GlobalMessagingStore _store;

        public FakeViewModel(GlobalMessagingStore store)
        {
            _store = store;
            AddNewCommand = new AsyncRelayCommand(
                _ => ExecuteActionAsync(true),
                (ex) => _store.SetCurrentMessage(ex.Message, StatusMessageType.Error)
            );
            ShowSomethingCommand = new AsyncRelayCommand(
                _ => ExecuteActionAsync(false),
                (ex) => _store.SetCurrentMessage(ex.Message, StatusMessageType.Error)
            );
        }

        public ICommand AddNewCommand { get; }
        public ICommand ShowSomethingCommand { get; }

        private Task ExecuteActionAsync(bool value)
        {
            try
            {
                // Simulate failing behavior
                if (value) throw new ArgumentNullException("Could not call the command");
            }
            catch (Exception ex)
            {
                _store.SetCurrentMessage(ex.Message, StatusMessageType.Error);
            }
            return Task.CompletedTask;
        }
    }
}
