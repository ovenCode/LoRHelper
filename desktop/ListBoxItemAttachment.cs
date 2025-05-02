using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace desktop
{
    public class ListBoxItemAttachment : ListBoxItem
    {
        public string AttachmentName { get; set; } = null!;
        public BitmapSource Icon { get; set; } = null!;
    }
}