namespace desktop.data.Models.DTOs
{
    public class AdventurePowerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PowerCode { get; set; }
        public string? Rarity { get; set; }
        public string? RarityRef { get; set; }
        public string? Description { get; set; }
        public string? DescriptionRaw { get; set; }
        public string? AssetAbsolutePath { get; set; }
        public string? AssetFullAbsolutePath { get; set; }
        public PowerState PowerState { get; set; } = PowerState.Undefined;
    }
}
