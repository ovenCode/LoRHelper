using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using desktop.data.Models;
using LoRAPI.Controllers;
using LoRAPI.Models;
using Newtonsoft.Json;

namespace desktop
{
    /// <summary>
    /// Interaction logic for InGamePage.xaml
    /// </summary>
    public partial class InGamePage : Page, ILoRHelperWindow
    {
        private const double cardScale = 0.18;
        private const double spellScale = 0.28;
        private const int cardListedHeight = 40;
        Action<string> requireUpdate;
        ILoRApiHandler? loRAPI;

        public InGamePage(
            ILoRApiHandler? loRAPI,
            Action<string> onUpdateRequired
        )
        {
            this.loRAPI = loRAPI;
            requireUpdate = onUpdateRequired;
            InitializeComponent();            
        }

        public async Task LoadCards()
        {
            try
            {
                ListBoxItem first = new ListBoxItem();
                var mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
                if (mergedDict != null)
                {
                    BitmapSource? source;
                    if (loRAPI != null)
                    {
                        Deck deck = await loRAPI.GetDeckAsync();
                        if (deck.CardsInDeck != null)
                        {
                            List<Card> cards = new List<Card>();
                            using (StreamReader reader = new StreamReader("./assets/files/sets/data/sets.json"))
                            {
                                List<SetCard>? setCards = JsonConvert.DeserializeObject<List<SetCard>>(reader.ReadToEnd());
                                if (setCards != null)
                                {
                                    for (int i = 0; i < deck.CardsInDeck.Count; i++)
                                    {
                                        var setCard = setCards.Find(card => card.CardCode == deck.CardsInDeck.Keys.ElementAt(i));
                                        cards.Add(new Card
                                        {
                                            CardImage = GetImageSource($"./assets/files/sets/images/{deck.CardsInDeck.Keys.ElementAt(i)}-full.png", out source) 
                                            ?
                                            new TransformedBitmap(
                                                source,
                                                new ScaleTransform { ScaleY = setCard?.Type != "Zaklęcie" ? cardScale : spellScale, ScaleX = setCard?.Type != "Zaklęcie" ? cardScale : spellScale }
                                            )
                                            : null,
                                            Name = setCard?.Name ?? "",
                                            ManaCost = setCard?.Cost ?? 0,
                                            DrawProbability = "TODO",
                                            CardType = setCard?.Type != "Zaklęcie" ? "100 30 260 50" : "15 100 260 50",
                                            CopiesRemaining = deck.CardsInDeck.Values.ElementAt(i),
                                            Region = setCard?.Region ?? Regions.All.ToString(),
                                        });
                                    }
                                }
                            }

                            foreach (var card in cards)
                            {
                                ListBoxItem newItem = new ListBoxItem();
                                newItem.DataContext = card;
                                newItem.Style = Application.Current.FindResource("CardItem") as Style;
                                newItem.Background = (LinearGradientBrush)mergedDict[card.Region + "Region"];
                                newItem.Height = cardListedHeight;
                                newItem.Foreground = new SolidColorBrush(Colors.White);
                                newItem.FontWeight = FontWeights.Bold;
                                cardsLB.Items.Add(newItem);
                            }
                        }
                    }
                    else
                    {
                        first.DataContext = new Card
                        {
                            CardImage = GetImageSource(
                            "./assets/files/set9-lite-pl_pl/pl_pl/img/cards/06DE021T1-full.png",
                            out source
                        )
                            ? new TransformedBitmap(
                                source,
                                new ScaleTransform { ScaleY = cardScale, ScaleX = cardScale }
                            )
                            : null,
                            Name = "Vayne",
                            ManaCost = 5,
                            DrawProbability = "1/4",
                            CopiesRemaining = 2
                        };
                        first.Style = Application.Current.FindResource("CardItem") as Style;
                        first.Background = (LinearGradientBrush)mergedDict["DemaciaRegion"];
                        first.Height = 40;
                        cardsLB.Items.Add(first);
                    }
                }
                else
                {
                    first.DataContext = null;
                    DataContext = null;
                }
            }
            catch (Exception)
            {

                throw;
            }            
        }

        private bool GetImageSource(string path, out BitmapSource? result)
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
                Trace.WriteLine("Succesfully got image source");
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

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            requireUpdate("Profile");
        }

        private void loadCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem first = new ListBoxItem();
            var mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
            if (mergedDict != null)
            {
                BitmapSource? source;
                first.DataContext = new Card
                {
                    CardImage = GetImageSource(
                        "./assets/files/set9-lite-pl_pl/pl_pl/img/cards/06DE021T1-full.png",
                        out source
                    )
                        ? new TransformedBitmap(
                            source,
                            new ScaleTransform { ScaleY = cardScale, ScaleX = cardScale }
                        )
                        : null,
                    Name = "Vayne",
                    ManaCost = 5,
                    DrawProbability = "1/4",
                    CopiesRemaining = 2
                };
                first.Style = Application.Current.FindResource("CardItem") as Style;
                first.Background = (LinearGradientBrush)mergedDict["DemaciaRegion"];
                first.Height = 40;
                cardsLB.Items.Add(first);
            }
            else
            {
                first.DataContext = null;
                DataContext = null;
            }
        }

        public static Brush GetBackground()
        {
            return new SolidColorBrush(Color.FromRgb(139,89,17));
        }
    }
}
