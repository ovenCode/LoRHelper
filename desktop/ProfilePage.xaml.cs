using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using data.Hubs;
using desktop.data.Models;
using LoRAPI.Controllers;
using LoRAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace desktop
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page, ILoRHelperWindow
    {
        ObservableCollection<Match> Matches = new ObservableCollection<Match>();
        ILoRApiHandler? loR;
        Action<string> requireUpdate;
        private readonly IHubContext<LoRHub>? hub;

        public ProfilePage(ILoRApiHandler? loRAPI, Action<string> onUpdateRequired)
        {
            loR = loRAPI;           
            requireUpdate = onUpdateRequired;
            InitializeComponent();
            //LoadData().GetAwaiter().GetResult();
            Matches.Add(new Match
            {
                Id = 0,
                IsWin = false,
                DeckCode = "TEST",
                Opponent = "TEST",
                GameType = GameType.PvP,
                Regions = new List<Region> { new Region { RegionType = Regions.Ionia }, new Region { RegionType = Regions.Demacia } },
                OpponentRegions = new List<Region> { new Region { RegionType = Regions.Freljord }, new Region { RegionType = Regions.Noxus } }
            });
            matchesLB.Items.Add(new MatchItem(Matches[0]));
            DataContext = this;
        }

        private async Task LoadData()
        {
            try
            {
                CardPositions? cardPositions = await loR?.GetCardPositionsAsync()! ?? null;
                Deck? deck = await loR?.GetDeckAsync()! ?? null;
                GameResult? gameResult = await loR?.GetGameResultAsync()! ?? null;

                Matches.Add(
                    new Match
                    {
                        Id = gameResult?.GameID ?? -1,
                        IsWin = false,
                        DeckCode = deck?.DeckCode ?? "Błąd",
                        Opponent = cardPositions?.OpponentName ?? "Błąd",
                        GameType = GameType.PathOfChampions
                    }
                );
            }
            catch (HttpRequestException error)
            {
                CustomMessageBox messageBox = new CustomMessageBox(error.Message);
                messageBox.ShowDialog();
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Wystąpił błąd");
                throw;
            }
        }

        private async Task LoadCards()
        {
            // JUST A TEST FUNCTION
            try
            {
                Deck? deck = await loR?.GetDeckAsync()! ?? null;
                Image? cardImage;
                ImageSource? imageSource;

                if (deck != null && deck.CardsInDeck != null)
                {
                    using (StreamReader r = new StreamReader("./assets/files/set9-lite-pl_pl/pl_pl/data/set9-pl_pl.json"))
                    {
                        string json = r.ReadToEnd();
                        List<SetCard> setCards = JsonConvert.DeserializeObject<List<SetCard>>(json) ?? new List<SetCard>();

                        foreach (var card in deck.CardsInDeck)
                        {
                            if (setCards.Count != 0)
                            {
                                foreach (var setCard in setCards)
                                {
                                    if (card.Key == setCard.CardCode)
                                    {
                                        Trace.WriteLine($"Trying to add {card.Key} image to stackpanel.");
                                        cardImage = GetImageSource($"./assets/files/set9-lite-pl_pl/pl_pl/img/cards/{setCard.CardCode}.png", out imageSource) ? new Image { Width = 80, Height = 120, Source = imageSource } : null;
                                        if (cardImage != null)
                                        {
                                            infoSP.Children.Add(cardImage);
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }                    
                }
            }
            catch (HttpRequestException error)
            {
                CustomMessageBox messageBox = new CustomMessageBox(error.Message);
                messageBox.ShowDialog();
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Wystąpił błąd");
                throw;
            }
        }

        private bool GetImageSource(string path, out ImageSource? result)
        {
            if (path.Length == 0)
            {
                path = "image-help-1.png"; // change to image-not-found
            }
            try
            {
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
                result = bitmapSource;
                return true;
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine($"File {path} was not found.");
                result = null;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadCards();
        }

        private void inGameBtn_Click(object sender, RoutedEventArgs e)
        {
            requireUpdate("InGame");            
        }

        public static Brush GetBackground()
        {
            return new SolidColorBrush(Color.FromRgb(220, 132, 15));
        }
    }
}
