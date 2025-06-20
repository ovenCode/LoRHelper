namespace desktop.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string? username;
        public string? Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string? windowLocation;
        public string? WindowLocation
        {
            get => windowLocation;
            set
            {
                windowLocation = value;
                OnPropertyChanged(nameof(WindowLocation));
            }
        }
        private string? dimensions;
        public string? Dimensions
        {
            get => dimensions;
            set
            {
                dimensions = value;
                OnPropertyChanged(nameof(Dimensions));
            }
        }
        private string? language;
        public string? Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
    }
}
