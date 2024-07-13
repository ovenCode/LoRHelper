using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace desktop.data.Models
{
    public interface ICard
    {
        public ImageSource? CardImage { get; set; }
        public int ManaCost { get; set; }
        public string Name { get; set; }
        public string DrawProbability { get; set; }
        public string Region { get; set; }
        public string CardType { get; set; }
        public string CardViewRect { get; set; }
        public CardStatus? CardStatus { get; set; }
        public string CardCode { get; set; }
        public int? CardId { get; set; }
        public int CopiesInDeck { get; set; }
        public int CopiesRemaining { get; set; }
        public int Attack { get; set; }
        public int Health { get; internal set; }
    }
    public class Card : ICard
    {
        public ImageSource? CardImage { get; set; }
        public int ManaCost { get; set; }
        public string Name { get; set; } = "";
        public string DrawProbability { get; set; } = "";
        public string Region { get; set; } = "";
        public string CardType { get; set; } = "";
        public string CardViewRect { get; set; } = "";
        public CardStatus? CardStatus { get; set; } = Models.CardStatus.InDeck;
        public string CardCode { get; set; } = "";
        public int? CardId { get; set; } = null;
        public int CopiesInDeck { get; set; }
        public int CopiesRemaining { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
    }

    public enum CardStatus
    {
        InDeck,
        InHand,
        OnBoard,
        InCombat
    }

    public class POCCard : Card, ICard
    {
        /*public ImageSource? CardImage { get; set; }
        public int ManaCost { get; set; }
        public string Name { get; set; } = "";
        public string DrawProbability { get; set; } = "";
        public string Region { get; set; } = "";
        public string CardType { get; set; } = "";
        public string CardCode { get; set; } = "";
        public int? CardId { get; set; } = null;
        public int CopiesRemaining { get; set; }
        public int Attack { get; internal set; }
        public int Health { get; internal set; }*/
        public List<IAttachment> Attachments { get; set; } = new List<IAttachment>();
    }

    public interface IAttachment
    {
        public string Name { get; set; }
        public AttachmentType Type { get; set; }
        public string Rarity { get; set; }
        public string RarityRef { get; set; }
        public string Description { get; set; }
        public string DescriptionRaw { get; set; }
        public string AssetAbsolutePath { get; set; }
        public string AssetFullAbsolutePath { get; set; }
    }

    public enum AttachmentType
    {
        Item,
        Relic,
        Unit,
        Weapon
    }

    public class Relic : IAttachment
    {
        public string Name { get; set; } = "";
        public AttachmentType Type { get; set; }
        public string Rarity { get; set; } = "";
        public string RarityRef { get; set; } = "";
        public string RelicCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string DescriptionRaw { get; set; } = "";
        public string AssetAbsolutePath { get; set; } = "";
        public string AssetFullAbsolutePath { get; set; } = "";
    }

    public class Item : IAttachment
    {
        public string Name { get; set; } = "";
        public AttachmentType Type { get; set; }
        public string Rarity { get; set; } = "";
        public string RarityRef { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public string Description { get; set; } = "";
        public string DescriptionRaw { get; set; } = "";
        public string AssetAbsolutePath { get; set; } = "";
        public string AssetFullAbsolutePath { get; set; } = "";
    }

    public class Adventure
    {
        public string Name { get; set; } = "";
        public List<AdventureNode> Nodes { get; set; } = new List<AdventureNode>();
        public List<AdventurePower> Powers { get; set; } = new List<AdventurePower>();
        public AdventureType AdventureType { get; set; } = AdventureType.Default;
        public TimeSpan CompletionTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? Health { get; set; }
        public string? Grade { get; set; }
    }

    public class AdventureNode
    {
        public string Name { get; set; } = "";
        public List<POCCard> Deck { get; set; } = new List<POCCard>();
        public List<AdventurePower> Powers { get; set; } = new List<AdventurePower>();
        public TimeSpan CompletionTime { get; set; }
    }

    public enum AdventureType
    {
        Default,
        Weekly,
        MonthlyChallenge
    }

    public class AdventurePower
    {
        public string Name { get; set; } = "";
        public string PowerCode { get; set; } = "";
        public string Rarity { get; set; } = "";
        public string RarityRef { get; set; } = "";
        public string Description { get; set; } = "";
        public string DescriptionRaw { get; set; } = "";
        public string AssetAbsolutePath { get; set; } = "";
        public string AssetFullAbsolutePath { get; set; } = "";
        public PowerState PowerState { get; set; } = PowerState.Undefined;
    }

    public enum PowerState
    {
        Player,
        Enemy,
        Both,
        Undefined
    }
}
