using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRAPI.Models
{
    public class Card
    {
        public List<string> AssociatedCards { get; set; } = new List<string>();
        public List<string> AssociatedCardsRefs { get; set; } = new List<string>();
        public List<Dictionary<string, string>> assets { get; set; } = new List<Dictionary<string, string>>();
        public object? Region { get; set; }
        public object? RegionRef { get; set; }
        public int Attack { get; set; }
        public int Cost { get; set; }
        public int Health { get; set; }
        public string Description { get; set; } = "";
        public string DescriptionRaw { get; set; } = "";
        public string LevelupDescription { get; set; } = "";
        public string LevelupDescriptionRaw { get; set; } = "";
        public string FlavorText { get; set; } = "";
        public string ArtistName { get; set; } = "";
        public string Name { get; set; } = "";
        public string CardCode { get; set; } = "";
        public List<string> Keywords { get; set; } = new List<string>();
        public List<string> KeywordsRefs { get; set; } = new List<string>();
        public string SpellSpeed { get; set; } = "";
        public string SpellSpeedRef { get; set; } = "";
        public string Rarity { get; set; } = "";
        public string RarityRef { get; set; } = "";
        public object? Subtype { get; set; }
        public string Supertype { get; set; } = "";
        public string Type { get; set; } = "";
        public string Collectible { get; set; } = "";
        public string? Set { get; set; } = null;
        public List<string>? Formats { get; set; } = null;
        public List<string>? FormatRefs { get; set; } = null;
    }
}
