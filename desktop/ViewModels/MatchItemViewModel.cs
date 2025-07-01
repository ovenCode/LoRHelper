using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using desktop.Commands;
using desktop.data.Models;

namespace desktop.ViewModels
{
    public class MatchItemViewModel : ViewModelBase
    {
        public string? Opponent { get; set; }
        public ImageSource? Region1 { get; }
        public ImageSource? Region2 { get; }
        public ImageSource? Region3 { get; }
        public ImageSource? Region4 { get; }
        public ImageSource? GameType { get; }

        private string _isWin = "#2B4F3B";
        public string IsWin => _isWin;
        private string _isWinContainer = "#223E2F";
        public string IsWinContainer => _isWinContainer;
        public AsyncCommandBase? MatchViewDetailsCommand { get; }

        public MatchItemViewModel(Match match)
        {
            Opponent = match.Opponent;
            SetIsWin(match.IsWin);
            GameType =
                match.GameType == data.Models.GameType.PvP
                    ? GetImageSource(
                        "./assets/files/core-en_us/en_us/img/formats/queue_select_standard_toggle_active.png"
                    )
                    : GetImageSource(
                        "./assets/files/core-en_us/en_us/img/formats/queue_select_standard_toggle_active.png"
                    );
            for (int i = 0; i < 2; i++)
            {
                Region1 = GetImageSource(
                    $"./assets/files/core-en_us/en_us/img/regions/icon-{match.Regions[0].RegionType.ToString().ToLower()}.png"
                );
                Region2 = GetImageSource(
                    $"./assets/files/core-en_us/en_us/img/regions/icon-{match.Regions[1].RegionType.ToString().ToLower()}.png"
                );
                Region3 = GetImageSource(
                    $"./assets/files/core-en_us/en_us/img/regions/icon-{match.OpponentRegions[0].RegionType.ToString().ToLower()}.png"
                );
                Region4 = GetImageSource(
                    $"./assets/files/core-en_us/en_us/img/regions/icon-{match.OpponentRegions[1].RegionType.ToString().ToLower()}.png"
                );
            }
        }

        private void SetIsWin(bool isWin)
        {
            _isWin = isWin ? "#2B4F3B" : "#612E40";
            _isWinContainer = isWin ? "#223E2F" : "#4F2433";
            OnPropertyChanged(nameof(IsWin));
            OnPropertyChanged(nameof(IsWinContainer));
        }

        private ImageSource? GetImageSource(string path)
        {
            try
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
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
