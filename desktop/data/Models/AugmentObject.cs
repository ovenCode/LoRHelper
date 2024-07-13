using desktop.misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static bool TryParse(IAttachment attachment, string attachmentCode, out AugmentObject? augment)
        {
            try
            {
                BitmapSource? source;
                string attachmentType = attachment.GetType().ToString().ToLower();

                augment = new AugmentObject { AugmentImage = Tools.GetImageSource($"{Constants.AssetsAdventureImagesPath}{attachmentType}/{attachmentCode}.png", out source) ? source : null, AugmentImagePath = $"{Constants.AssetsAdventureImagesPath}{attachmentType}/{attachmentCode}.png", AugmentWidth = 50, AugmentTextWidth = "0", AugmentText = "", AugmentName = "", AugmentCode = attachmentCode };
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                //throw;
            }
            augment = null;
            return false;
        }
    }
}
