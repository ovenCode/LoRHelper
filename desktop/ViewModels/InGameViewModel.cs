using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using desktop.Commands;
using desktop.data.Models;
using desktop.Services;

namespace desktop.ViewModels
{
    public class InGameViewModel : ViewModelBase, IAsyncDisposable
    {
        private readonly InGameService _inGameService;

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
        public ICommand? AddNewCommand { get; }
        public ICommand? SearchCommand { get; }
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
            InGameService inGameService
        )
        {
            _inGameService = inGameService;
            GoBackCommand = new NavigateCommand<ProfileViewModel>(profilePageNavigationService);
            LoadCardsCommand = Deck is IEnumerable<ICard> cards
                ? new LoadCardsCommand(inGameService, ref cards)
                : null;
            ShowRemainingCardsCommand = RemainingCards is IEnumerable<ICard> remainingCards
                ? new ShowRemainingCardsCommand(Deck ?? new List<ICard>(), PlayedCards ?? new List<ICard>(), ref remainingCards)
                : null;
            AddNewCommand = new AsyncRelayCommand(
                (param) => Task.Run(() => ShowPopUp = Visibility.Visible)
            );
            ClosePopupCommand = new AsyncRelayCommand(
                (param) => Task.Run(() => ShowPopUp = Visibility.Hidden)
            );
            ShowLocationsCommand = new ShowLocationsCommand(Deck ?? new List<ICard>());
            OpponentName = inGameService.OpponentName;
            _ = new AsyncRelayCommand((param) => _inGameService.StartAsync());
            _inGameService.NewDataReceived += OnNewDataReceived;
        }

        public static InGameViewModel LoadViewModel(
            NavigationService<ProfileViewModel> profilePageNavigationService,
            InGameService inGameService
        )
        {
            return new InGameViewModel(
                profilePageNavigationService: profilePageNavigationService,
                inGameService: inGameService
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
