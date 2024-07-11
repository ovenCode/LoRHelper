using LoRAPI.Models;

namespace LoRAPI.Controllers
{
    public interface ILoRApiHandler
    {
        public Task<Deck> GetDeckAsync();
        public Task<CardPositions> GetCardPositionsAsync();
        public Task<GameResult> GetGameResultAsync();
    }
}