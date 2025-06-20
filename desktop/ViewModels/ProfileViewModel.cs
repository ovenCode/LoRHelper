using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using desktop.Commands;
using desktop.data.Models;
using desktop.Services;
using desktop.Stores;

namespace desktop.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ProfileStore _profileStore;
        private string? profileName;

        public string? ProfileName
        {
            get => _profileStore.Profile?.ProfileName ?? profileName;
            set
            {
                profileName = value;
                _profileStore.Profile?.UpdateProfileName(value);
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        private ObservableCollection<Match>? matches;

        public IEnumerable<Match>? Matches => matches;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand? ShowProfileCommand { get; }
        public ICommand? ShowInGameCommand { get; }
        public ICommand? ShowInfoCommand { get; }
        public ICommand? ShowSettingsCommand { get; }
        public ICommand? ChangeGameTypeCommand { get; }
        public ICommand? ClearFiltersCommand { get; }
        public ICommand? RefreshMatchesCommand { get; }
        public ICommand? DownloadMatchesCommand { get; }

        public ProfileViewModel(
            NavigationStore navigationStore,
            ProfileStore profileStore,
            //NavigationService<ProfileViewModel> profileNavigationService,
            NavigationService<InGameViewModel> inGameNavigationService,
            NavigationService<InfoViewModel> infoNavigationService,
            NavigationService<SettingsViewModel> settingNavigationService
        )
        {
            _navigationStore = navigationStore;
            _profileStore = profileStore;
            matches = profileStore.Matches is ObservableCollection<Match> Matches ? Matches : [];
            //ShowProfileCommand = new NavigateCommand<ProfileViewModel>(profileNavigationService);
            ShowInGameCommand = new NavigateCommand<InGameViewModel>(inGameNavigationService);
            ShowInfoCommand = new NavigateCommand<InfoViewModel>(infoNavigationService);
            ShowSettingsCommand = new NavigateCommand<SettingsViewModel>(settingNavigationService);
            // ChangeGameTypeCommand = new ();

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        public static ProfileViewModel LoadViewModel(
            NavigationStore navigationStore,
            ProfileStore profileStore,
            NavigationService<InGameViewModel> inGameNavigationService,
            NavigationService<InfoViewModel> infoNavigationService,
            NavigationService<SettingsViewModel> settingNavigationService
        )
        {
            ProfileViewModel profileViewModel = new ProfileViewModel(
                navigationStore,
                profileStore,
                inGameNavigationService,
                infoNavigationService,
                settingNavigationService
            );

            return profileViewModel;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
