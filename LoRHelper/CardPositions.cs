public class CardPositions
{
    public string? PlayerName { get; set; }
    public string? OpponentName { get; set; }
    public string GameState { get; set; } = null!;
    public Dictionary<string, string> Screen { get; set; } = null!;
    public List<Rectangle> Rectangles { get; set; } = new List<Rectangle>();
}
