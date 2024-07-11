using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace desktop.data.Models
{
    public class AugmentObject
    {
        public int AugmentWidth { get; set; }
        public string AugmentTextWidth { get; set; } = null!;
        public string? AugmentImagePath { get; set; }
        public BitmapSource? AugmentImage { get; set; }
        public string AugmentName { get; set; } = null!;
        public string AugmentCode { get; set; } = null!;
        public string AugmentText { get; set; } = null!;
    }
}
