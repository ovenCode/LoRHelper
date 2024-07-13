namespace LoRAPI.Models
{
    public class CardPositions
    {
        public string? PlayerName { get; set; }
        public string? OpponentName { get; set; }
        public string GameState { get; set; } = null!;
        public Dictionary<string, int> Screen { get; set; } = new Dictionary<string, int> { { "ScreenWidth", 1 }, { "ScreenHeight", 1 } };
        public List<Rectangle> Rectangles { get; set; } = new List<Rectangle>();
    }
}
