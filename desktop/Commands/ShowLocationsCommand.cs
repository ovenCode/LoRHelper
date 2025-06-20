using desktop.data.Models;
using desktop.Services;

namespace desktop.Commands
{
    public class ShowLocationsCommand : AsyncCommandBase
    {
        private readonly IEnumerable<ICard> _cardsCollection;

        public ShowLocationsCommand(IEnumerable<ICard> cards) => _cardsCollection = cards;

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is not IEnumerable<ICard>)
            {
                await Task.FromException(
                    new ArgumentException("The passed argument is not a collection of cards")
                );
            }

            parameter = _cardsCollection.Where((card) => card.CardType == "Lokacja");
        }
    }
}
