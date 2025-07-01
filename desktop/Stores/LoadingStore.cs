namespace desktop.Stores
{
    public class LoadingStore : ILoadingStore
    {
        private bool isLoading = false;
        public override bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnIsLoadingChanged();
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string loadingMessage = string.Empty;

        public override string LoadingMessage
        {
            get => loadingMessage;
            set
            {
                if (loadingMessage != value)
                {
                    loadingMessage = value;
                    OnPropertyChanged(nameof(LoadingMessage));
                }
            }
        }

        public override event Action? LoadingStatusChanged;

        private void OnIsLoadingChanged()
        {
            LoadingStatusChanged?.Invoke();
        }
    }
}