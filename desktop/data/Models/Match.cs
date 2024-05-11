using System.ComponentModel.DataAnnotations;
using System.Configuration;


public class Match
{
    [Key]
    public int Id { get; set; }
    public bool IsWin { get; set; }
    public string DeckCode { get; set; } = null!;
    public string Opponent { get; set; } = null!;
    public GameType GameType { get; set; }
}

public enum GameType
{
    PvP,
    PathOfChampions
}