using System.Collections.Generic;

namespace desktop.data.Models.DTOs
{
    public class POCCardDTO
    {
        public int Id { get; set; }
        public string? CardImage { get; set; }
        public int ManaCost { get; set; }
        public string? Name { get; set; }
        public string? DrawProbability { get; set; }
        public string? Region { get; set; }
        public string? CardType { get; set; }
        public string? CardViewRect { get; set; }
        public CardStatus? CardStatus { get; set; } = Models.CardStatus.InDeck;
        public string? CardCode { get; set; }
        public int? CardId { get; set; } = null;
        public int CopiesInDeck { get; set; }
        public int CopiesRemaining { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public List<AttachmentDTO> Attachments { get; set; } = new List<AttachmentDTO>();
    }
}
