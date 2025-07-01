using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using desktop.Commands;
using desktop.data.db;
using desktop.Services;
using desktop.Stores;
using LoRAPI.Controllers;
using Microsoft.VisualStudio.Threading;

namespace desktop.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ILoadingStore _loadingStore;
        private readonly GlobalMessagingStore _globalMessagingStore;

        public GlobalMessagingViewModel? GlobalMessagingViewModel { get; }

        public bool IsLoading => _loadingStore.IsLoading;

        public ICommand? ShowProfile { get; }
        public ICommand? ShowInGame { get; }
        public ICommand? ShowSettings { get; }
        public ICommand? ShowWelcome { get; }
        public ICommand? ShowInfo { get; }
        public ICommand? WindowResize { get; }

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public AppViewModel(
            NavigationStore navigationStore,
            ILoadingStore loadingStore,
            GlobalMessagingStore globalMessagingStore,
            GlobalMessagingViewModel globalMessagingViewModel,
            ILoadingService loadingService,
            JoinableTaskFactory joinableTaskFactory
        )
        {
            _navigationStore = navigationStore;
            _loadingStore = loadingStore;
            _globalMessagingStore = globalMessagingStore;
            GlobalMessagingViewModel = globalMessagingViewModel;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _loadingStore.LoadingStatusChanged += OnLoadingStatusChanged;
            // Initialize with default view
        }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            _loadingStore.LoadingStatusChanged -= OnLoadingStatusChanged;
            GlobalMessagingViewModel?.Dispose();
            base.Dispose();
        }

        private ProfileViewModel? CreateProfileViewModel(
            NavigationStore navigationStore,
            ProfileStore profileStore,
            NavigationService<ProfileViewModel> navigationService,
            ILoadingService loadingService
        )
        {
            return null;
            // return new ProfileViewModel(
            //     navigationStore,
            //     profileStore,
            //     new NavigationService<InGameViewModel>(
            //         navigationStore,
            //         () => CreateInGameViewModel(navigationStore, navigationService, taskFactory, loadingService)
            //     ),
            //     new NavigationService<InfoViewModel>(
            //         navigationStore,
            //         () => CreateInfoViewModel(navigationStore, navigationService)
            //     ),
            //     new NavigationService<SettingsViewModel>(
            //         navigationStore,
            //         () => CreateSettingsViewModel(navigationStore, navigationService)
            //     )
            // );
        }

        private InGameViewModel? CreateInGameViewModel(
            NavigationStore navigationStore,
            NavigationService<ProfileViewModel> profileNavigationService,
            JoinableTaskFactory taskFactory,
            ILoadingService loadingService
        )
        {
            return null;
            // return new InGameViewModel(
            //     profileNavigationService,
            //     new InGameService(null, null, null, new ErrorLogger()),
            //     loadingService,
            //     taskFactory: taskFactory
            // );
        }

        private InfoViewModel? CreateInfoViewModel(
            NavigationStore navigationStore,
            NavigationService<ProfileViewModel> profileNavigationService
        )
        {
            return null;
            // return new InfoViewModel();
        }

        private SettingsViewModel? CreateSettingsViewModel(
            NavigationStore navigationStore,
            NavigationService<ProfileViewModel> profileNavigationService
        )
        {
            return null;
            // return new SettingsViewModel();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnLoadingStatusChanged()
        {
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public enum AppPage
    {
        Profile,
        InGame,
        Info,
        Settings,
        Welcome,
    }
}
