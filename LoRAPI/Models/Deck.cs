namespace LoRAPI.Models
{
    public class Deck
    {
        public string? DeckCode { get; set; }
        public Dictionary<string, string>? CardsInDeck { get; set; }
    }
}
