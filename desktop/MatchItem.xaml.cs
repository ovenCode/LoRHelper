using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using desktop.data.Models;

namespace desktop
{
    /// <summary>
    /// Interaction logic for MatchItem.xaml
    /// </summary>
    public partial class MatchItem : UserControl
    {
        public Match? Match { get; set; }

        public MatchItem(Match? match = null)
        {
            InitializeComponent();
            DataBinder data = new DataBinder();
            Match = match;

            if (Match != null)
            {
                data.Opponent = Match.Opponent;
                data.GameType =
                    Match.GameType == GameType.PvP
                        ? GetImageSource("./assets/files/core-en_us/en_us/img/formats/queue_select_standard_toggle_active.png")
                        : GetImageSource("./assets/files/core-en_us/en_us/img/formats/queue_select_standard_toggle_active.png");
                if (Match.Regions.Count == 2)
                {
                    data.Region2 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.Regions[0].RegionType.ToString().ToLower()}.png"
                    );
                    data.Region3 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.OpponentRegions[0].RegionType.ToString().ToLower()}.png"
                    );
                }
                else if (Match.Regions.Count >= 2 && Match.OpponentRegions.Count >= 2)
                {
                    data.Region1 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.Regions[0].RegionType.ToString().ToLower()}.png"
                    );
                    data.Region2 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.Regions[1].RegionType.ToString().ToLower()}.png"
                    );
                    data.Region3 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.OpponentRegions[0].RegionType.ToString().ToLower()}.png"
                    );
                    data.Region4 = GetImageSource(
                        $"./assets/files/core-en_us/en_us/img/regions/icon-{Match.OpponentRegions[1].RegionType.ToString().ToLower()}.png"
                    );
                }
                else
                {
                    data.Region1 = GetImageSource("path/to/pvpiconfile");
                    data.Region2 = GetImageSource("");
                    data.Region3 = GetImageSource("");
                    data.Region4 = GetImageSource("");
                }
                data.IsWin = Match.IsWin ? "#2B4F3B" : "#612E40";
                data.IsWinContainer = Match.IsWin ? "#223E2F" : "#4F2433";
            }
            else
            {
                data.Region1 = GetImageSource("");
                data.Region2 = GetImageSource("");
                data.Region3 = GetImageSource("");
                data.Region4 = GetImageSource("");
                data.Opponent = "Error";
                data.GameType = GetImageSource("");
                data.IsWin = "#612E40";
                data.IsWinContainer = "#4F2433";
            }
            this.DataContext = data;
        }

        // LOSE #612E40
        // WIN #2B4F3B #223E2F

        private ImageSource GetImageSource(string path)
        {
            if (path.Length == 0)
            {
                path = "image-help-1.png"; // change to image-not-found
            }
            Stream imageStreamSource = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );
            PngBitmapDecoder decoder = new PngBitmapDecoder(
                imageStreamSource,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default
            );
            BitmapSource bitmapSource = decoder.Frames[0];

            return bitmapSource;
        }

        private void MatchTBTN_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleButton)?.IsChecked ?? false)
            {
                this.Height = 200;
                matchItemEXTGrid.Height = 140;
            }
            else
            {
                this.Height = 60;
                matchItemEXTGrid.Height = 0;
            }            
        }
    }

    class DataBinder
    {
        public string? Opponent { get; set; }
        public ImageSource? Region1 { get; set; }
        public ImageSource? Region2 { get; set; }
        public ImageSource? Region3 { get; set; }
        public ImageSource? Region4 { get; set; }
        public ImageSource? GameType { get; set; }
        public string IsWin { get; set; } = "#2B4F3B";
        public string IsWinContainer { get; set; } = "#223E2F";
    }
}
