using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using desktop.Commands;
using desktop.data.Models;
using desktop.Services;
using desktop.Stores;
using Microsoft.VisualStudio.Threading;
using IAsyncDisposable = System.IAsyncDisposable;

namespace desktop.ViewModels
{
    public class InGameViewModel : ViewModelBase, IAsyncDisposable
    {
        private readonly InGameService _inGameService;
        private readonly ILoadingService _loadingService;
        protected readonly GlobalMessagingStore _globalMessagingStore;

        //// PROPERTIES

        private string? strongestUnit;

        /// <summary>
        /// The strongest unit on the board
        /// </summary>
        public string? StrongestUnit
        {
            get => strongestUnit;
            set
            {
                strongestUnit = value;
                OnPropertyChanged(nameof(StrongestUnit));
            }
        }

        private string? opponentName;

        public string? OpponentName
        {
            get => opponentName;
            set
            {
                opponentName = value;
                OnPropertyChanged(nameof(OpponentName));
            }
        }

        private string? deckName;

        /// <summary>
        /// Name of the deck
        /// </summary>
        public string? DeckName
        {
            get => deckName;
            set
            {
                deckName = value;
                OnPropertyChanged(nameof(DeckName));
            }
        }

        private List<ICard>? deck;

        public List<ICard>? Deck
        {
            get => deck;
            set
            {
                deck = value;
                OnPropertyChanged(nameof(Deck));
            }
        }

        private List<ICard>? playedCards;

        public List<ICard>? PlayedCards
        {
            get => playedCards;
            set
            {
                playedCards = value;
                OnPropertyChanged(nameof(PlayedCards));
            }
        }

        private List<ICard>? remainingCards;

        public List<ICard>? RemainingCards
        {
            get => remainingCards;
            set
            {
                remainingCards = value;
                OnPropertyChanged(nameof(RemainingCards));
            }
        }

        private ICard? cardPreview;

        public ICard? CardPreview
        {
            get => cardPreview;
            set
            {
                cardPreview = value;
                OnPropertyChanged(nameof(CardPreview));
            }
        }

        private Visibility _showPopUp = Visibility.Hidden;

        public Visibility ShowPopUp
        {
            get => _showPopUp;
            set
            {
                _showPopUp = value;
                OnPropertyChanged(nameof(ShowPopUp));
            }
        }

        public ICommand? GoBackCommand { get; }
        public ICommand? AddNewCommand { get; protected set; }
        public ICommand? SearchCommand { get; }
        public ICommand? StartGameCommand { get; }
        public ICommand? EndGameCommand { get; }
        public ICommand? ShowDrawCardsCommand { get; }
        public ICommand? ShowRemainingCardsCommand { get; }
        public ICommand? LoadCardsCommand { get; }
        public ICommand? ShowSpellsCommand { get; }
        public ICommand? ShowUnitsCommand { get; }
        public ICommand? ShowLocationsCommand { get; }
        public ICommand? AddPowersCommand { get; }
        public ICommand? AddMissionsCommand { get; }
        public ICommand? ClosePopupCommand { get; }

        public InGameViewModel(
            NavigationService<ProfileViewModel> profilePageNavigationService,
            InGameService inGameService,
            ILoadingService loadingService,
            JoinableTaskFactory taskFactory,
            GlobalMessagingStore messagingStore
        )
        {
            _inGameService = inGameService;
            _loadingService = loadingService;
            _globalMessagingStore = messagingStore;

            try
            {
                GoBackCommand = new NavigateCommand<ProfileViewModel>(
                    profilePageNavigationService,
                    taskFactory: taskFactory
                );
                LoadCardsCommand = Deck is IEnumerable<ICard> cards
                    ? new LoadCardsCommand(inGameService, ref cards)
                    : null;
                ShowRemainingCardsCommand = RemainingCards is IEnumerable<ICard> remainingCards
                    ? new ShowRemainingCardsCommand(
                        Deck ?? new List<ICard>(),
                        PlayedCards ?? new List<ICard>(),
                        ref remainingCards
                    )
                    : null;
                AddNewCommand = new AsyncRelayCommand(
                    (param) => Task.Run(() => ShowPopUp = Visibility.Visible)
                );
                ClosePopupCommand = new AsyncRelayCommand(
                    (param) => Task.Run(() => ShowPopUp = Visibility.Hidden)
                );
                ShowLocationsCommand = new ShowLocationsCommand(Deck ?? new List<ICard>());
                OpponentName = inGameService.OpponentName;
                StartGameCommand = new StartGameCommand(
                    inGameService: inGameService,
                    loadingService: loadingService,
                    taskFactory: taskFactory
                );
                _inGameService.NewDataReceived += OnNewDataReceived;
            }
            catch (System.Exception ex)
            {
                _globalMessagingStore.SetCurrentMessage(
                    statusMessage: ex.Message,
                    statusMessageType: StatusMessageType.Error
                );
            }
        }

        public static InGameViewModel LoadViewModel(
            NavigationService<ProfileViewModel> profilePageNavigationService,
            InGameService inGameService,
            ILoadingService loadingService,
            JoinableTaskFactory taskFactory,
            GlobalMessagingStore globalMessagingStore
        )
        {
            return new InGameViewModel(
                profilePageNavigationService: profilePageNavigationService,
                inGameService: inGameService,
                loadingService: loadingService,
                taskFactory: taskFactory,
                messagingStore: globalMessagingStore
            );
        }

        private void OnNewDataReceived(object? sender, EventArgs eventArgs)
        {
            // Handle new data received from service
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            _inGameService.NewDataReceived -= OnNewDataReceived;
            await _inGameService.StopAsync();
            base.Dispose();
        }
    }
}
