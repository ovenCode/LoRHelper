using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace desktop.Stores
{
    public class ILoadingStore : INotifyPropertyChanged
    {
        public virtual bool IsLoading { get; set; }
        public virtual string LoadingMessage { get; set; } = string.Empty;
        public virtual event Action? LoadingStatusChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}