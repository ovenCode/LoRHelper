namespace LoRAPI.Models
{
    public class Rectangle
    {
        public int CardID { get; set; }
        public string CardCode { get; set; } = null!;

        // position top left corner of card to bottom edge of client
        public int TopLeftX { get; set; }

        // position top left corner of card to bottom edge of client
        public int TopLeftY { get; set; }

        // Card Width
        public int Width { get; set; }

        // Card Height
        public int Height { get; set; }

        // Is Card Yours
        public bool LocalPlayer { get; set; }
    }
}
