using System.ComponentModel;
using System.Windows.Media;
using desktop.Stores;

namespace desktop.ViewModels
{
    public class GlobalMessagingViewModel : ViewModelBase
    {
        private readonly GlobalMessagingStore _messagingStore;

        static Brush informationStatus = new SolidColorBrush(Color.FromArgb(255, 231, 158, 54)),
            informationStatusText = new SolidColorBrush(Color.FromArgb(255, 37, 25, 4)),
            warningStatus = new SolidColorBrush(Color.FromArgb(255, 110, 48, 13)),
            warningStatusText = new SolidColorBrush(Color.FromArgb(255, 247, 228, 226)),
            errorStatus = new SolidColorBrush(Color.FromArgb(255, 37, 25, 4)),
            errorStatusText = new SolidColorBrush(Color.FromArgb(255, 231, 158, 54));

        public string? Message => _messagingStore.CurrentStatusMessage;
        public StatusMessageType MessageType => _messagingStore.MessageType;

        public bool HasStatusMessage => !string.IsNullOrEmpty(_messagingStore.CurrentStatusMessage);

        public Brush StatusMessageBackground =>
            MessageType switch
            {
                StatusMessageType.Warning => warningStatus,
                StatusMessageType.Error => errorStatus,
                _ => informationStatus,
            };

        public Brush StatusMessageColor =>
            MessageType switch
            {
                StatusMessageType.Warning => warningStatusText,
                StatusMessageType.Error => errorStatusText,
                _ => informationStatusText,
            };

        public GlobalMessagingViewModel(GlobalMessagingStore messagingStore)
        {
            _messagingStore = messagingStore;
            _messagingStore.PropertyChanged += OnMessageChanged;
        }

        private void OnMessageChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(MessageType));
            OnPropertyChanged(nameof(HasStatusMessage));
        }

        public override void Dispose()
        {
            _messagingStore.PropertyChanged -= OnMessageChanged;
            base.Dispose();
        }
    }
}
