using System.Collections.Generic;

namespace desktop.data.Models.DTOs
{
    public class AdventureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<AdventureNodeDTO> Nodes { get; set; } = new List<AdventureNodeDTO>();
        public List<AdventurePowerDTO> Powers { get; set; } = new List<AdventurePowerDTO>();
        public AdventureType AdventureType { get; set; } = AdventureType.Default;
        public string? CompletionTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? Health { get; set; }
        public string? Grade { get; set; }
    }
}
