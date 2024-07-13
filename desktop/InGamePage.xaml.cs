using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using LoRAPI.Controllers;
using LoRAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace desktop
{
    /// <summary>
    /// Interaction logic for InGamePage.xaml
    /// </summary>
    public partial class InGamePage : Page, ILoRHelperWindow
    {
        // CONSTANTS
        private const double cardScale = 0.18;
        private const double spellScale = 0.28;
        private const int cardListedHeight = 46;
        private const int cardPreviewHeight = 250;
        private const string assetsDataPath = "./assets/files/sets/data/";
        private const string assetsImagesPath = "./assets/files/sets/images/";

        Action<string> requireUpdate;
        ILoRApiHandler? loRAPI;
        ResourceDictionary? mergedDict;
        ErrorLogger logger;

        List<data.Models.Card> cards = new List<data.Models.Card>();
        List<data.Models.Card> allCards = new List<data.Models.Card>();

        public InGamePage(
            ILoRApiHandler? loRAPI,
            Action<string> onUpdateRequired,
            ErrorLogger errorLogger, double height = 0)
        {
            this.loRAPI = loRAPI;
            requireUpdate = onUpdateRequired;
            logger = errorLogger;
            Height = height == 0 ? Double.NaN : height;
            InitializeComponent();            
        }

        public async Task LoadCards()
        {
            try
            {
                ListBoxItem first = new ListBoxItem();
                if (cardsLB.Items.Count > 0)
                {
                    cardsLB.Items.Clear();
                    cards.Clear();
                }
                mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
                if (mergedDict != null)
                {
                    BitmapSource? source;
                    if (loRAPI != null)
                    {
                        Deck deck = await loRAPI.GetDeckAsync();
                        allCards = (await loRAPI.GetAllCards()).Select(card => new data.Models.Card
                        {
                            Name = card.Name,
                            CardCode = card.CardCode,
                            ManaCost = card.Cost,
                            CardImage = GetImageSource($"{assetsImagesPath}{card.CardCode}.png", out source) ? source : null
                    }).ToList();
                        if (deck.CardsInDeck != null)
                        {                            
                            using (StreamReader reader = new StreamReader($"{assetsDataPath}setsDummy.json"))
                            {
                                List<SetCard>? setCards = JsonConvert.DeserializeObject<List<SetCard>>(reader.ReadToEnd());
                                if (setCards != null)
                                {
                                    for (int i = 0; i < deck.CardsInDeck.Count; i++)
                                    {
                                        var setCard = setCards.Find(card => card.CardCode == deck.CardsInDeck.Keys.ElementAt(i));
                                        cards.Add(new data.Models.Card
                                        {
                                            CardImage = GetImageSource($"{assetsImagesPath}{deck.CardsInDeck.Keys.ElementAt(i)}-full.png", out source) 
                                            ?
                                            new TransformedBitmap(
                                                source,
                                                new ScaleTransform { ScaleY = setCard?.Type != "Zaklęcie" ? cardScale : spellScale, ScaleX = setCard?.Type != "Zaklęcie" ? cardScale : spellScale }
                                            )
                                            : null,
                                            Name = setCard?.Name ?? "",
                                            ManaCost = setCard?.Cost ?? 0,
                                            DrawProbability = "TODO",
                                            CardType = setCard?.Type ?? "",
                                            CardViewRect = setCard?.Type != "Zaklęcie" ? "100 30 260 50" : "15 100 260 50",
                                            CopiesRemaining = deck.CardsInDeck.Values.ElementAt(i),
                                            CardCode = setCard?.CardCode ?? "",
                                            Attack = setCard?.Attack ?? 0,
                                            Health = setCard?.Health ?? 0,
                                            Region = (setCard == null) ? Regions.All.ToString() : setCard!.RegionRef!.GetType() == typeof(string) ? setCard!.RegionRef!.ToString()! : (setCard!.RegionRef! as JArray)!.Count > 0 ? (setCard!.RegionRef! as JArray)!.ElementAt(0).ToString() : Regions.Freljord.ToString(),
                                        });
                                    }
                                }
                            }

                            foreach (var card in cards)
                            {
                                ListBoxItem newItem = new ListBoxItem();
                                ListBoxItem item = new ListBoxItem();
                                newItem.DataContext = card;
                                newItem.Style = Application.Current.FindResource("CardItem") as Style;
                                newItem.Background = (LinearGradientBrush)mergedDict[card.Region + "Region"];
                                newItem.Height = cardListedHeight;
                                newItem.Foreground = new SolidColorBrush(Colors.White);
                                newItem.FontWeight = FontWeights.Bold;
                                newItem.PreviewMouseLeftButtonDown += CardItem_MouseLeftButtonDown;
                                cardsLB.Items.Add(newItem);
                                item.Content = new TextBox
                                {
                                    AcceptsReturn = true,
                                    Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                                };
                                item.DataContext = card;
                                item.Height = 40;
                                item.Foreground = new SolidColorBrush(Colors.White);
                                item.FontWeight = FontWeights.Bold;
                                allCardsLB.Items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        first.DataContext = new data.Models.Card
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
            catch (HttpRequestException error)
            {
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private async void CardItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await LoadCard(((sender as ListBoxItem)!.DataContext as data.Models.Card)!.CardCode ?? "").WaitAsync(CancellationToken.None);
            cardPreviewGrid.Visibility = Visibility.Visible;
            cardPreview.Focus();
        }

        public Task LoadCard(string code)
        {
            try
            {
                BitmapSource? source;
                //cardInspect.Source = GetImageSource($"{assetsImagesPath}{code}.png", out source) ? source : null; // "error_loading_image.png";                
                //Trace.WriteLine(allCards.FirstOrDefault(card => card.CardCode == code)!.CardImage!.ToString());
                cardPreview.Children.Add(new CardPreview(allCards.FirstOrDefault(card => card.CardCode == code)!));
                return Task.CompletedTask;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task LoadGameData()
        {
            try
            {
                if(mergedDict == null)
                {
                    Trace.WriteLine("Zasoby nie załadowały się poprawnie.");
                    return;
                }
                if (loRAPI != null)
                {
                    CardPositions? positions = await loRAPI.GetCardPositionsAsync();
                    if (positions != null)
                    {
                        if (positions.OpponentName != null && positions.OpponentName!.StartsWith("card_"))
                        {                            
                            encounterLbl.Content = positions.OpponentName!.Substring(5, positions.OpponentName!.LastIndexOf('_') - 5);
                        }
                        List<data.Models.Card> drawnCards = cards.Where(card => positions.Rectangles.Any(rect => rect.CardCode == card.CardCode)).ToList();
                        List<data.Models.Card> boardCards = allCards.Where(card => positions.Rectangles.Where(rect => (double)rect.TopLeftY / (double)positions.Screen["ScreenWidth"] > 0.09 && (double)rect.TopLeftY / (double)positions.Screen["ScreenWidth"] < 0.9 && rect.CardCode != "face").Any(rect => rect.CardCode == card.CardCode))
                            .Select(card => new data.Models.Card
                            {
                                Name = card.Name,
                                ManaCost = card.ManaCost,
                                CardCode = card.CardCode,
                                CardId = positions.Rectangles.First(rect => rect.CardCode == card.CardCode).CardID,
                                Attack = card.Attack,
                                Health = card.Health,
                            }).ToList();
                        List<data.Models.Card> strongest = boardCards.OrderByDescending(card => card.Attack).ThenBy(card => card.Health).ThenBy(card => card.ManaCost).ToList();
                        List<data.Models.Card> boardNHandCards = positions.Rectangles.Select(rect => new data.Models.Card
                        {
                            CardId = rect.CardID,
                            CardCode = rect.CardCode
                        }).ToList();

                        gameStateLbl.Content = "Stan gry: " + positions.GameState + " " + positions.Screen["ScreenWidth"] + " " + positions.Screen["ScreenHeight"];
                        strongestLbl.Content = "Najsilniejsza jednostka: " + (strongest.Where((card, cardId) => strongest[0].Attack == card.Attack).Count() == 1
                            ?
                            allCards.First(el => strongest.First().CardCode == el.CardCode).Name
                            :
                            strongest.Where(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health).Count() == 1
                            ?
                            allCards.First(el => strongest.First(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health).CardCode == el.CardCode).Name
                            :
                            strongest.Where(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health && strongest[0].ManaCost == card.ManaCost).Count() == 1
                            ?
                            allCards.First(el => strongest.First(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health && strongest[0].ManaCost == card.ManaCost).CardCode == el.CardCode).Name
                            :
                            allCards.First(el => strongest.Where(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health && strongest[0].ManaCost == card.ManaCost).ElementAt(new Random().Next(strongest.Where(card => strongest[0].Attack == card.Attack && strongest[0].Health == card.Health && strongest[0].ManaCost == card.ManaCost).Count() - 1)).CardCode == el.CardCode).Name);
                        cardsLB.Items.Clear();
                        foreach (var card in drawnCards)
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
                        /*foreach (var card in strongest)
                        {
                            ListBoxItem item = new ListBoxItem();
                            item.DataContext = card;
                            item.Content = new TextBox
                            {
                                AcceptsReturn = true,
                                Text = card.CardId + " " + card.CardCode + " " + positions.Rectangles.First(rect => rect.CardID == card.CardId).TopLeftX + " " + positions.Rectangles.First(rect => rect.CardID == card.CardId).TopLeftY + "\n" + allCards.FirstOrDefault(el => el.CardCode == card.CardCode)?.Name
                            };
                            item.Height = 40;
                            item.Foreground = new SolidColorBrush(Colors.White);
                            item.FontWeight = FontWeights.Bold;
                            allCardsLB.Items.Add(item);
                        }*/
                    }                    
                }
            }
            catch (HttpRequestException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                searchBoxTB.Text = error.Message;
                //return new CardPositions();
            }
            catch (NullReferenceException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
                //return new CardPositions();
                //throw error;
            }
            catch (InvalidOperationException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
                //return new CardPositions();
            }
            catch (TaskCanceledException error)
            {
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
            }
            catch (NotSupportedException error)
            {
                searchBoxTB.Text = error.Message;
            }
            catch (ArgumentNullException error)
            {
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
            }
            catch (ArgumentOutOfRangeException error)
            {
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
            }
            catch (Exception error)
            {
                searchBoxTB.Text = error.Message;
                await logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
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
                //Trace.WriteLine("Succesfully got image source");
                return true;
            }
            catch (FileNotFoundException error)
            {
                Trace.WriteLine($"File {path} was not found.");
                result = null;
                logger.LogMessage(error.Message, MessageType.Error, error.GetType().Name).Wait();
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            requireUpdate("Profile Load");
        }

        private async void loadCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            /*ListBoxItem first = new ListBoxItem();
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
            }*/
            await LoadCards();
        }

        public static Brush GetBackground()
        {
            return new SolidColorBrush(Color.FromRgb(139,89,17));
        }

        private async void drawnCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            await LoadGameData();
        }

        private void cardPreviewGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            var location = Mouse.GetPosition(cardPreviewGrid);
            if(!cardPreview.IsMouseOver)
            {
                cardPreview.Children.RemoveAt(0);
                cardPreviewGrid.Visibility = Visibility.Hidden;
            }
        }

        private void cardPreview_LostFocus(object sender, RoutedEventArgs e)
        {
            cardPreviewGrid.Visibility = Visibility.Hidden;
        }

        private void FilterCards_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(FilterCards.Text))
            {
                bool IsSubtypeMatched = false;
                allCardsLB.Items.Clear();

                if(IsSubtypeMatched)
                {
                    // TODO: check if it's part of any subtype (elites, lurkers, etc.)
                }
                else
                {
                    foreach (var card in cards)
                    {
                        if (card.Name.ToLower().StartsWith(FilterCards.Text.Trim().ToLower()))
                        {
                            allCardsLB.Items.Add(new ListBoxItem
                            {
                                Content = new TextBox
                                {
                                    AcceptsReturn = true,
                                    Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                                },
                                DataContext = card,
                                Height = 40,
                                Foreground = new SolidColorBrush(Colors.White),
                                FontWeight = FontWeights.Bold
                            });
                        }
                    }
                }                
            }
            else if (string.IsNullOrWhiteSpace(FilterCards.Text))
            {
                allCardsLB.Items.Clear();

                foreach (var card in cards)
                {
                    allCardsLB.Items.Add(new ListBoxItem
                    {
                        Content = new TextBox
                        {
                            AcceptsReturn = true,
                            Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                        },
                        DataContext = card,
                        Height = 40,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold
                    });
                }
            }
        }

        private void allCardsLBL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            allCardSP.Visibility = allCardSP.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void inGameSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!(deckSV.IsMouseOver || allCardsSV.IsMouseOver))
            {
                var mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                mouseWheelEventArgs.RoutedEvent = MouseWheelEvent;
                mouseWheelEventArgs.Source = sender;
                inGameSV.RaiseEvent(mouseWheelEventArgs);
            }
        }

        private void deckSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            mouseWheelEventArgs.RoutedEvent = MouseWheelEvent;
            mouseWheelEventArgs.Source = sender;
            deckSV.RaiseEvent(mouseWheelEventArgs);
        }

        private void allCardsSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            mouseWheelEventArgs.RoutedEvent = MouseWheelEvent;
            mouseWheelEventArgs.Source = sender;
            allCardsSV.RaiseEvent(mouseWheelEventArgs);
        }

        private void spellCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((sender as ToggleButton)?.IsChecked == null ? false : (sender as ToggleButton)!.IsChecked!))
            {
                for (int i = allCardsLB.Items.Count - 1; i >= 0; i--)
                {
                    if (cards[i].CardType == "Zaklęcie")
                    {
                        //
                    }
                    else
                    {
                        allCardsLB.Items.RemoveAt(i);
                    }
                }
            }
            else
            {
                allCardsLB.Items.Clear();
                Trace.WriteLine(cards.Count);
                foreach (var card in cards)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = new TextBox
                    {
                        AcceptsReturn = true,
                        Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                    };
                    item.DataContext = card;
                    item.Height = 40;
                    item.Foreground = new SolidColorBrush(Colors.White);
                    item.FontWeight = FontWeights.Bold;
                    allCardsLB.Items.Add(item);
                }
            }            
        }

        private void unitCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((sender as ToggleButton)?.IsChecked == null ? false : (sender as ToggleButton)!.IsChecked!))
            {
                for (int i = allCardsLB.Items.Count - 1; i >= 0; i--)
                {
                    if (cards[i].CardType == "Jednostka")
                    {
                        //
                    }
                    else
                    {
                        allCardsLB.Items.RemoveAt(i);
                    }
                }
            }
            else
            {
                allCardsLB.Items.Clear();
                Trace.WriteLine(cards.Count);
                foreach (var card in cards)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = new TextBox
                    {
                        AcceptsReturn = true,
                        Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                    };
                    item.DataContext = card;
                    item.Height = 40;
                    item.Foreground = new SolidColorBrush(Colors.White);
                    item.FontWeight = FontWeights.Bold;
                    allCardsLB.Items.Add(item);
                }
            }
        }

        private void locationCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)((sender as ToggleButton)?.IsChecked == null ? false : (sender as ToggleButton)!.IsChecked!))
            {
                for (int i = allCardsLB.Items.Count - 1; i >= 0; i--)
                {
                    if (cards[i].CardType == "Lokacja")
                    {
                        //
                    }
                    else
                    {
                        allCardsLB.Items.RemoveAt(i);
                    }
                }
            }
            else
            {
                allCardsLB.Items.Clear();
                Trace.WriteLine(cards.Count);
                foreach (var card in cards)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = new TextBox
                    {
                        AcceptsReturn = true,
                        Text = $"{card.CardId} {card.CardCode} {card.Name} 0"
                    };
                    item.DataContext = card;
                    item.Height = 40;
                    item.Foreground = new SolidColorBrush(Colors.White);
                    item.FontWeight = FontWeights.Bold;
                    allCardsLB.Items.Add(item);
                }
            }
        }

        private void addNewBtn_Click(object sender, RoutedEventArgs e)
        {
            addDialogGrid.Visibility = Visibility.Visible;
        }

        private void closeAddDialogBtn_Click(object sender, RoutedEventArgs e)
        {
            addDialogGrid.Visibility = Visibility.Hidden;
        }
    }
}
