using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using desktop.Commands;
using desktop.data.Models;
using desktop.Services;
using desktop.Stores;
using Microsoft.VisualStudio.Threading;

namespace desktop.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ProfileStore _profileStore;
        private readonly ILoadingStore _loadingStore;
        private readonly ILoadingService _loadingService;
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

        public IEnumerable<MatchItemViewModel>? Matches =>
            matches?.Select(match => new MatchItemViewModel(match));

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
        public bool IsLoading => _loadingStore.IsLoading;

        public ICommand? ShowProfileCommand { get; }
        public ICommand? ShowInGameCommand { get; }
        public object? ShowInGameCommandParameter { get; }
        public ICommand? ShowInfoCommand { get; }
        public ICommand? ShowSettingsCommand { get; }
        public ICommand? ChangeGameTypeCommand { get; }
        public ICommand? ClearFiltersCommand { get; }
        public ICommand? RefreshMatchesCommand { get; }
        public ICommand? DownloadMatchesCommand { get; }

        public ProfileViewModel(
            NavigationStore navigationStore,
            ProfileStore profileStore,
            ILoadingStore loadingStore,
            ILoadingService loadingService,
            InGameService inGameService,
            JoinableTaskFactory taskFactory,
            NavigationService<InGameViewModel> inGameNavigationService,
            NavigationService<InfoViewModel> infoNavigationService,
            NavigationService<SettingsViewModel> settingNavigationService
        )
        {
            _navigationStore = navigationStore;
            _profileStore = profileStore;
            _loadingStore = loadingStore;
            _loadingService = loadingService;
            // matches = profileStore.Matches is ObservableCollection<Match> Matches ? Matches : [];
            matches =
            [
                new Match
                {
                    GameType = GameType.PvP,
                    IsWin = true,
                    Regions =
                    [
                        new Region { RegionType = Regions.BandleCity },
                        new Region { RegionType = Regions.Bilgewater },
                    ],
                    OpponentRegions =
                    [
                        new Region { RegionType = Regions.Demacia },
                        new Region { RegionType = Regions.Noxus },
                    ],
                    DeckCode = "Something",
                    Opponent = "Player X",
                },
                new Match
                {
                    GameType = GameType.PathOfChampions,
                    IsWin = true,
                    Regions = new List<Region>
                    {
                        new Region { RegionType = Regions.BandleCity },
                        new Region { RegionType = Regions.Bilgewater },
                    },
                    OpponentRegions = new List<Region>
                    {
                        new Region { RegionType = Regions.Demacia },
                        new Region { RegionType = Regions.Noxus },
                    },
                },
                new Match
                {
                    GameType = GameType.PathOfChampions,
                    IsWin = true,
                    Regions = new List<Region>
                    {
                        new Region { RegionType = Regions.BandleCity },
                        new Region { RegionType = Regions.Bilgewater },
                    },
                    OpponentRegions = new List<Region>
                    {
                        new Region { RegionType = Regions.Demacia },
                        new Region { RegionType = Regions.Noxus },
                    },
                },
                new Match
                {
                    GameType = GameType.PvP,
                    IsWin = true,
                    Regions =
                    [
                        new Region { RegionType = Regions.BandleCity },
                        new Region { RegionType = Regions.Bilgewater },
                    ],
                    OpponentRegions =
                    [
                        new Region { RegionType = Regions.Demacia },
                        new Region { RegionType = Regions.Noxus },
                    ],
                    DeckCode = "Something",
                    Opponent = "Player X",
                },
                new Match
                {
                    GameType = GameType.PvP,
                    IsWin = true,
                    Regions =
                    [
                        new Region { RegionType = Regions.BandleCity },
                        new Region { RegionType = Regions.Bilgewater },
                    ],
                    OpponentRegions =
                    [
                        new Region { RegionType = Regions.Demacia },
                        new Region { RegionType = Regions.Noxus },
                    ],
                    DeckCode = "Something",
                    Opponent = "Player X",
                },
            ];
            //ShowProfileCommand = new NavigateCommand<ProfileViewModel>(profileNavigationService);
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            ShowInGameCommand = new NavigateCommand<InGameViewModel>(
                inGameNavigationService,
                taskFactory: taskFactory,
                loadingService: loadingService,
                tokenSource: cancellationToken
            );
            ShowInGameCommandParameter = (object)inGameService.StartAsync;
            ShowInfoCommand = new NavigateCommand<InfoViewModel>(
                infoNavigationService,
                taskFactory: taskFactory
            );
            ShowSettingsCommand = new NavigateCommand<SettingsViewModel>(
                settingNavigationService,
                taskFactory: taskFactory
            );
            // ChangeGameTypeCommand = new ();
            // inGameNavigationService.IsLoaded += OnNavigationComplete;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _loadingStore.LoadingStatusChanged += OnLoadingStatusChanged;
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _loadingStore.LoadingStatusChanged -= OnLoadingStatusChanged;
            base.Dispose();
        }

        public static ProfileViewModel LoadViewModel(
            NavigationStore navigationStore,
            ProfileStore profileStore,
            ILoadingStore loadingStore,
            ILoadingService loadingService,
            InGameService inGameService,
            JoinableTaskFactory taskFactory,
            NavigationService<InGameViewModel> inGameNavigationService,
            NavigationService<InfoViewModel> infoNavigationService,
            NavigationService<SettingsViewModel> settingNavigationService
        )
        {
            ProfileViewModel profileViewModel = new ProfileViewModel(
                navigationStore: navigationStore,
                profileStore: profileStore,
                loadingStore: loadingStore,
                loadingService: loadingService,
                inGameService: inGameService,
                taskFactory: taskFactory,
                inGameNavigationService: inGameNavigationService,
                infoNavigationService: infoNavigationService,
                settingNavigationService: settingNavigationService
            );

            return profileViewModel;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnLoadingStatusChanged()
        {
            //OnPropertyChanged(nameof(IsLoading));
        }

        private void OnNavigationComplete(bool isComplete)
        {
            if (isComplete)
            {
                //
            }
            else { }
        }
    }
}
