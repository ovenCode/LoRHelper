using desktop.data.Models;
using desktop.Services;

namespace desktop.Commands
{
    public class ShowRemainingCardsCommand : CommandBase
    {
        private IEnumerable<ICard>? _deck,
            _playedCards,
            _cards;

        public ShowRemainingCardsCommand(
            IEnumerable<ICard> deck,
            IEnumerable<ICard> playedCards,
            ref IEnumerable<ICard> cards
        )
        {
            _deck = deck;
            _playedCards = playedCards;
            _cards = cards;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _cards = Enumerable.Empty<ICard>();
                foreach (ICard card in _deck ?? [])
                {
                    if (!_playedCards?.Any((played) => played.CardCode == card.CardCode) ?? false)
                    {
                        _cards?.Append(card);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
