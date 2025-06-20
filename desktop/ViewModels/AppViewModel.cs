using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using desktop.Commands;
using desktop.data.db;
using desktop.Services;
using desktop.Stores;
using LoRAPI.Controllers;

namespace desktop.ViewModels
{
    public class AppViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly NavigationStore _navigationStore;

        public ICommand? ShowProfile { get; }
        public ICommand? ShowInGame { get; }
        public ICommand? ShowSettings { get; }
        public ICommand? ShowWelcome { get; }
        public ICommand? ShowInfo { get; }

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public AppViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            // // Initialize with default view
            // _currentView = new pages.ProfilePage();
            // // Initialize commands
            // // ICommand test = new
            // ShowProfile = new AsyncRelayCommand(
            //     (param) => NavigateToPageAsync(AppPage.Profile, param),
            //     (ex) => ShowException(ex)
            // );
            // ShowInGame = new AsyncRelayCommand(
            //     (param) => NavigateToPageAsync(AppPage.InGame, param),
            //     (ex) => ShowException(ex)
            // );
            // ShowSettings = new AsyncRelayCommand(
            //     (param) => NavigateToPageAsync(AppPage.Settings, param),
            //     (ex) => ShowException(ex)
            // );
            // ShowWelcome = new AsyncRelayCommand(
            //     (param) => NavigateToPageAsync(AppPage.Welcome, param),
            //     (ex) => ShowException(ex)
            // );
            // ShowInfo = new AsyncRelayCommand(
            //     (param) => NavigateToPageAsync(AppPage.Info, param),
            //     (ex) => ShowException(ex)
            // );
        }

        // private Task NavigateToPageAsync(AppPage page, object? param)
        // {
        //     (
        //         ILoRApiHandler lorAPI,
        //         ICommand onUpdateRequired,
        //         ILoRDbContextFactory lorDb,
        //         ErrorLogger errorLogger
        //     )? parameters = DeconstructToPage(param);

        //     if (parameters == null)
        //     {
        //         return Task.FromException(new ArgumentException("Invalid parameters provided"));
        //     }

        //     CurrentView = page switch
        //     {
        //         AppPage.Profile => new pages.ProfilePage(),
        //         AppPage.InGame => new pages.InGamePage(),
        //         AppPage.Info => new pages.InfoPage(),
        //         AppPage.Settings => new pages.SettingsPage(),
        //         _ => Task.FromException(new ArgumentException("Incorrect page requested"))
        //     };
        //     return Task.CompletedTask;
        // }

        // public Task NavigateToSettingsAsync()
        // {
        //     CurrentView = new SettingsPage();
        //     return Task.CompletedTask;
        // }

        // private void ShowException(Exception ex)
        // {
        //     CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
        //     messageBox.ShowDialog();
        //     Console.WriteLine(ex.Message);
        //     Trace.WriteLine(ex.Message);
        // }

        // private (ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger)? DeconstructToPage(
        //     object? param
        // )
        // {
        //     if (param is not Tuple<ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger>)
        //     {
        //         return null;
        //     }

        //     ILoRApiHandler? loRApi = (
        //         param as Tuple<ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger>
        //     )?.Item1;
        //     ICommand? onUpdateRequired = (
        //         param as Tuple<ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger>
        //     )?.Item2;
        //     ILoRDbContextFactory? loRDb = (
        //         param as Tuple<ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger>
        //     )?.Item3;
        //     ErrorLogger? errorLogger = (
        //         param as Tuple<ILoRApiHandler, ICommand, ILoRDbContextFactory, ErrorLogger>
        //     )?.Item4;

        //     if (loRApi == null || onUpdateRequired == null || loRDb == null || errorLogger == null)
        //     {
        //         return null;
        //     }

        //     return (loRApi, onUpdateRequired, loRDb, errorLogger);
        // }

        public override void Dispose()
        {
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public enum AppPage
    {
        Profile,
        InGame,
        Info,
        Settings,
        Welcome
    }
}
