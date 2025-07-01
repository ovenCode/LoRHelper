using System.ComponentModel;

namespace desktop.Stores
{
    public enum StatusMessageType
    {
        Information,
        Warning,
        Error,
    }

    public class GlobalMessagingStore : INotifyPropertyChanged
    {
        private string? currentStatusMessage;

        public string? CurrentStatusMessage
        {
            get => currentStatusMessage;
            private set
            {
                currentStatusMessage = value;
                OnPropertyChanged(nameof(CurrentStatusMessage));
            }
        }

        private StatusMessageType messageType = StatusMessageType.Information;

        public StatusMessageType MessageType
        {
            get => messageType;
            private set
            {
                messageType = value;
                OnPropertyChanged(nameof(MessageType));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetCurrentMessage(string statusMessage, StatusMessageType statusMessageType)
        {
            CurrentStatusMessage = statusMessage;
            MessageType = statusMessageType;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
