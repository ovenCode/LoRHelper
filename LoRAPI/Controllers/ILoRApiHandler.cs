using LoRAPI.Models;

namespace LoRAPI.Controllers
{
    public interface ILoRApiHandler
    {
        public bool IsAdventure { get; set; }
        public Task<Deck> GetDeckAsync();

        /// <summary>
        /// Async function to get GameData
        /// </summary>
        /// <returns>A task with card positions and additional data</returns>
        /// <exception cref="HttpRequestException" />
        /// <exception cref="NullReferenceException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="UriFormatException" />
        /// <exception cref="TaskCanceledException" />
        public Task<CardPositions> GetCardPositionsAsync();
        public Task<GameResult> GetGameResultAsync();
        public Task<IEnumerable<Card>> GetAllCardsAsync();
    }
}
