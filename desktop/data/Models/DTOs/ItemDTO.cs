namespace desktop.data.Models.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public AttachmentType Type { get; set; }
        public string? Rarity { get; set; }
        public string? RarityRef { get; set; }
        public string? ItemCode { get; set; }
        public string? Description { get; set; }
        public string? DescriptionRaw { get; set; }
        public string? AssetAbsolutePath { get; set; }
        public string? AssetFullAbsolutePath { get; set; }
    }
}
