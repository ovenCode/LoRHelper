using System.Collections.Generic;

namespace desktop.data.Models.DTOs
{
    public class AdventureNodeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<POCCardDTO> Deck { get; set; } = new List<POCCardDTO>();
        public List<AdventurePowerDTO> Powers { get; set; } = new List<AdventurePowerDTO>();
        public string? CompletionTime { get; set; }
    }
}
