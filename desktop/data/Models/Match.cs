using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Windows.Documents;

namespace desktop.data.Models 
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public bool IsWin { get; set; }
        public string DeckCode { get; set; } = null!;
        public string Opponent { get; set; } = null!;
        public GameType GameType { get; set; }
        public List<Region> Regions { get; set; } = new List<Region>();
        public List<Region> OpponentRegions { get; set; } = new List<Region>();
    }

    public enum GameType
    {
        PvP,
        PathOfChampions
    }
}
