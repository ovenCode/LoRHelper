using System;
using System.Threading.Tasks;
using desktop.data.Models;
using desktop.Services;

namespace desktop.Commands
{
    public class LoadCardsCommand : AsyncCommandBase
    {
        private readonly InGameService? _inGameService;
        private IEnumerable<ICard>? _cards;

        public LoadCardsCommand(InGameService inGameService, ref IEnumerable<ICard> cards)
        {
            _inGameService = inGameService;
            _cards = cards;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_inGameService != null)
            {
                _cards = await _inGameService.LoadCardsAsync();
            }
        }
    }
}
