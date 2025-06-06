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
using System.Windows.Shapes;
using desktop.data.Models;
using Discord;
using LoRAPI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace desktop
{
    /// <summary>
    /// Interaction logic for AdventureSetup.xaml
    /// </summary>
    public partial class AdventureSetup : Window
    {
        // CONSTANTS
        private const double powerSize = 50;
        private const double cardListedHeight = 46;
        private const string assetsAdventureDataPath = "./assets/files/adventure-pl_pl/pl_pl/data/";
        private const string assetsAdventureImagesPath = "./assets/files/adventure-pl_pl/pl_pl/img/";
        private Brush defaultBrush = new LinearGradientBrush(
       Color.FromArgb(255, 255, 0, 0),   // Opaque red
       Color.FromArgb(255, 0, 0, 255), 3);
        private Brush defaultBrush2 = new LinearGradientBrush(
       Color.FromArgb(255, 255, 0, 0),   // Opaque red
       Color.FromArgb(255, 0, 0, 255), 4);

        public List<AugmentObject?>? PowerIcons { get; set; }
        public List<AugmentObject?>? ItemIcons { get; set; }
        public List<AugmentObject?>? RelicIcons { get; set; }
        public List<AdventurePower>? allPowers { get; set; }
        public List<Item>? allItems { get; set; }
        public List<Relic>? allRelics { get; set; }
        public List<AdventurePower>? adventurePowers { get; set; }
        public List<ICard> Cards { get; set; }
        private POCCard? ChosenCard { get; set; }
        private int? ChosenCardId { get; set; }
        private Adventure Adventure { get; set; }
        private ErrorLogger? logger;

        public AdventureSetup(
            List<ICard> cards,
            Adventure? adventure,
            ResourceDictionary? mergedDict,
            ErrorLogger? errorLogger = null
        )
        {
            Cards = cards;
            if (adventure != null)
            {
                Adventure = adventure;
            }
            else
            {
                Adventure = new Adventure();
            }
            InitializeComponent();
            LoadAllPowers();
            LoadAllItems();
            LoadAllRelics();
            for (int i = 0; i < Cards.Count; i++)
            {
                ListBoxItemCard newItem = new ListBoxItemCard(),
                    newItem2 = new ListBoxItemCard();
                newItem.DataContext = cards[i];
                newItem.Style = Application.Current.FindResource("CardItem") as Style;
                newItem.Background = (LinearGradientBrush)(mergedDict?[cards[i].Region + "Region"] ?? defaultBrush);
                newItem.Height = cardListedHeight;
                newItem.Foreground = new SolidColorBrush(Colors.White);
                newItem.FontWeight = FontWeights.Bold;
                newItem.PreviewMouseLeftButtonDown += Card_MouseLeftButtonDown;
                newItem.SetIndex(i);
                newItem2.DataContext = cards[i];
                newItem2.Style = Application.Current.FindResource("CardItem") as Style;
                newItem2.Background = (LinearGradientBrush)(mergedDict?[cards[i].Region + "Region"] ?? defaultBrush2);
                newItem2.Height = cardListedHeight;
                newItem2.Foreground = new SolidColorBrush(Colors.White);
                newItem2.FontWeight = FontWeights.Bold;
                newItem2.PreviewMouseLeftButtonDown += Card_MouseLeftButtonDown;
                newItem2.SetIndex(i);
                DeckLB.Items.Add(newItem);
                DeckLB2.Items.Add(newItem2);
            }
            logger = errorLogger;
        }

        public void PowersTabBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement tab in displayGrid.Children)
            {
                tab.Visibility = Visibility.Collapsed;
            }

            displayGrid.Children[0].Visibility = Visibility.Visible;
        }

        public void ItemsTabBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement tab in displayGrid.Children)
            {
                tab.Visibility = Visibility.Collapsed;
            }

            displayGrid.Children[1].Visibility = Visibility.Visible;
        }

        public void RelicsTabBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement tab in displayGrid.Children)
            {
                tab.Visibility = Visibility.Collapsed;
            }

            displayGrid.Children[2].Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Function adds AdventureAugments to the UI for Relics and Items that are attatched to the selected card
        /// </summary>
        /// <param name="sender">ListBoxItemCard containing the card itself</param>
        /// <param name="e">MouseButton Event</param>
        /// <exception cref="NullReferenceException"></exception>
        private void Card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ChosenCard = ToPOCCard(
                (sender as ListBoxItemCard)!.DataContext as data.Models.Card
                    ?? new data.Models.Card()
            );
            ChosenCardId = (sender as ListBoxItemCard)?.GetIndex() ?? -1;
            ChosenCardLBL.Content = ChosenCard?.Name ?? "";
            ChosenCardLBL2.Content = ChosenCard?.Name ?? "";
            AdventureAugment augment;
            if (AddedItemsSP.Children.Count > 0)
            {
                AddedItemsSP.Children.Clear();
            }
            else if (AddedRelicsSP.Children.Count > 0)
            {
                AddedRelicsSP.Children.Clear();
            }
            for (int i = 0; i < (ChosenCard?.Attachments.Count ?? 0); i++)
            {
                if (ItemIcons != null && RelicIcons != null && ChosenCard != null)
                {
                    if (ChosenCard.Attachments[i].GetType() == typeof(Relic))
                    {
                        AugmentObject? augmentObject = RelicIcons.FirstOrDefault(relic =>
                            (ChosenCard.Attachments[i] as Relic)?.RelicCode == relic?.AugmentCode
                        );

                        if (augmentObject == null)
                        {
                            throw new NullReferenceException(
                                "Nie znaleziono reliktu odpowiadającego reliktowi na karcie"
                            );
                        }
                        augment = new AdventureAugment(augmentObject);

                        if (augment != null)
                        {
                            AddedRelicsSP.Children.Add(augment);
                        }
                        else
                            throw new NullReferenceException(
                                "Nie znaleziono reliktu odpowiadającego reliktowi na karcie"
                            );
                    }
                    else
                    {
                        AugmentObject? augmentObject = ItemIcons.Single(item =>
                            (ChosenCard.Attachments[i] as Item)?.ItemCode == item?.AugmentCode
                        );
                        if (augmentObject == null)
                        {
                            throw new NullReferenceException(
                                "Nie znaleziono przedmiotu odpowiadającemu przedmiotowi na karcie"
                            );
                        }
                        augment = new AdventureAugment(augmentObject);

                        if (augment != null)
                        {
                            AddedItemsSP.Children.Add(augment);
                        }
                        else
                            throw new NullReferenceException(
                                "Nie znaleziono przedmiotu odpowiadającego przedmiotowi na karcie"
                            );
                    }
                }
                else
                    throw new NullReferenceException("Błąd z ikonami");
            }
        }

        private POCCard ToPOCCard(data.Models.Card card)
        {
            //POCCard result = new POCCard
            //{
            //    CardImage = card.CardImage,
            //    Name = card.Name,
            //    ManaCost = card.ManaCost,
            //    DrawProbability = card.DrawProbability,
            //    CardType = card.CardType,
            //    CardViewRect = card.CardViewRect,
            //    CopiesRemaining = card.CopiesRemaining,
            //    CardCode = card.CardCode,
            //    Attack = card.Attack,
            //    Health = card.Health,
            //    Region = card.Region
            //};
            ICard selectedCard = Cards.Where(poc => poc.CardCode == card.CardCode).Single();
            POCCard result =
                (
                    selectedCard is POCCard
                        ? selectedCard as POCCard
                        : new POCCard
                        {
                            CardImage = selectedCard.CardImage,
                            Name = selectedCard.Name,
                            ManaCost = selectedCard.ManaCost,
                            DrawProbability = selectedCard.DrawProbability,
                            CardType = selectedCard.CardType,
                            CardViewRect = selectedCard.CardViewRect,
                            CopiesRemaining = selectedCard.CopiesRemaining,
                            CardCode = selectedCard.CardCode,
                            Attack = selectedCard.Attack,
                            Health = selectedCard.Health,
                            Region = selectedCard.Region
                        }
                ) ?? new POCCard();

            return result;
        }

        private void LoadAllPowers()
        {
            using (var fileReader = new StreamReader($"{assetsAdventureDataPath}powers-pl_pl.json"))
            {
                var json = fileReader.ReadToEnd();
                allPowers =
                    JsonConvert.DeserializeObject<List<AdventurePower>>(json)
                    ?? new List<AdventurePower>();
                PowerIcons = new List<AugmentObject?>();
                AddPowersToGrid();
            }
        }

        private void LoadAllItems()
        {
            using (var fileReader = new StreamReader($"{assetsAdventureDataPath}items-pl_pl.json"))
            {
                var json = fileReader.ReadToEnd();
                allItems = JsonConvert.DeserializeObject<List<Item>>(json) ?? new List<Item>();
                ItemIcons = new List<AugmentObject?>();
                AddItemsToGrid();
            }
        }

        private void LoadAllRelics()
        {
            using (var fileReader = new StreamReader($"{assetsAdventureDataPath}relics-pl_pl.json"))
            {
                var json = fileReader.ReadToEnd();
                allRelics = JsonConvert.DeserializeObject<List<Relic>>(json) ?? new List<Relic>();
                RelicIcons = new List<AugmentObject?>();
                AddRelicsToGrid();
            }
        }

        private void AddPowersToGrid()
        {
            if (allPowers != null && allPowers.Count > 0)
            {
                AdventureAugment augment;
                BitmapSource? source;
                for (int i = 0; i < allPowers.Count; i++)
                {
                    if (i % 6 == 0 && AllPowersGrid.RowDefinitions.Count != allPowers.Count / 6)
                    {
                        AllPowersGrid.RowDefinitions.Add(
                            new RowDefinition { Height = new GridLength(powerSize) }
                        );
                    }
                    augment = new AdventureAugment(
                        new AugmentObject
                        {
                            AugmentImage = GetImageSource(
                                $"{assetsAdventureImagesPath}powers/{allPowers[i].PowerCode}.png",
                                out source
                            )
                                ? source
                                : null,
                            AugmentImagePath =
                                $"{assetsAdventureImagesPath}powers/{allPowers[i].PowerCode}.png",
                            AugmentWidth = 50,
                            AugmentTextWidth = "0",
                            AugmentText = "",
                            AugmentName = "",
                            AugmentCode = allPowers[i].PowerCode
                        }
                    );
                    augment.MouseLeftButtonDown += PowerOnMouseLeftButtonDown;
                    PowerIcons!.Add((augment.DataContext as AugmentObject));

                    AllPowersGrid.Children.Add(augment);
                    Grid.SetRow(augment, i / 6);
                    Grid.SetColumn(augment, i % 6);
                }
            }
        }

        private void AddItemsToGrid()
        {
            if (allItems != null && allItems.Count > 0)
            {
                AdventureAugment augment;
                BitmapSource? source;
                for (int i = 0; i < allItems.Count; i++)
                {
                    if (i % 6 == 0 && AllItemsGrid.RowDefinitions.Count != allItems.Count / 6)
                    {
                        AllItemsGrid.RowDefinitions.Add(
                            new RowDefinition { Height = new GridLength(powerSize) }
                        );
                    }
                    augment = new AdventureAugment(
                        new AugmentObject
                        {
                            AugmentImage = GetImageSource(
                                $"{assetsAdventureImagesPath}items/{(allItems[i] as Item)!.ItemCode}.png",
                                out source
                            )
                                ? source
                                : null,
                            AugmentImagePath =
                                $"{assetsAdventureImagesPath}items/{(allItems[i] as Item)!.ItemCode}.png",
                            AugmentWidth = 50,
                            AugmentTextWidth = "0",
                            AugmentText = "",
                            AugmentName = "",
                            AugmentCode = allItems[i].ItemCode
                        }
                    );
                    augment.MouseLeftButtonDown += ItemOnMouseLeftButtonDown;
                    ItemIcons!.Add((augment.DataContext as AugmentObject));

                    AllItemsGrid.Children.Add(augment);
                    Grid.SetRow(augment, i / 6);
                    Grid.SetColumn(augment, i % 6);
                }
            }
        }

        private void AddRelicsToGrid()
        {
            if (allRelics != null && allRelics.Count > 0)
            {
                AdventureAugment augment;
                BitmapSource? source;
                for (int i = 0; i < allRelics.Count; i++)
                {
                    if (i % 6 == 0 && AllRelicsGrid.RowDefinitions.Count != allRelics.Count / 6)
                    {
                        AllRelicsGrid.RowDefinitions.Add(
                            new RowDefinition { Height = new GridLength(powerSize) }
                        );
                    }
                    augment = new AdventureAugment(
                        new AugmentObject
                        {
                            AugmentImage = GetImageSource(
                                $"{assetsAdventureImagesPath}relics/{(allRelics[i] as Relic)!.RelicCode}.png",
                                out source
                            )
                                ? source
                                : null,
                            AugmentImagePath =
                                $"{assetsAdventureImagesPath}relics/{(allRelics[i] as Relic)!.RelicCode}.png",
                            AugmentWidth = 50,
                            AugmentTextWidth = "0",
                            AugmentText = "",
                            AugmentName = "",
                            AugmentCode = allRelics[i].RelicCode
                        }
                    );
                    augment.MouseLeftButtonDown += RelicOnMouseLeftButtonDown;
                    RelicIcons!.Add(augment.DataContext as AugmentObject);

                    AllRelicsGrid.Children.Add(augment);
                    Grid.SetRow(augment, i / 6);
                    Grid.SetColumn(augment, i % 6);
                }
            }
        }

        private async void ItemOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (allItems != null)
                {
                    Item? selected = allItems.FirstOrDefault(item =>
                        item.ItemCode
                        == (
                            (sender as AdventureAugment)!.DataContext as AugmentObject
                        )!.AugmentImagePath!.Substring(
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('/') + 1,
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('.')
                                - 1
                                - (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath!.LastIndexOf('/')
                        )
                    );
                    if (selected != null && ChosenCard != null)
                    {
                        AdventureAugment newItem = new AdventureAugment(
                            new AugmentObject
                            {
                                AugmentImage = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImage,
                                AugmentImagePath = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath,
                                AugmentWidth = 250,
                                AugmentName = selected.Name,
                                AugmentText = selected.DescriptionRaw,
                                AugmentTextWidth = "4*",
                                AugmentCode = selected.ItemCode
                            }
                        );
                        ChosenCard.Attachments.Add(selected);
                        //(Cards[ChosenCardId ?? 0] as POCCard)!.Attachments.Add(selected);
                        newItem.MouseLeftButtonDown += NewItem_MouseLeftButtonDown;
                        AddedItemsSP.Children.Add(newItem);
                    }
                    else if (ChosenCard == null)
                    {
                        throw new InvalidOperationException(
                            "Proszę wybrać kartę, aby dodać przedmiot."
                        );
                    }
                    else if (selected == null)
                    {
                        throw new InvalidOperationException(
                            "Nie udało się odnaleźć przedmiotu. Spróbuj ponownie, i poinformuj administratora"
                        );
                    }
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private async void NewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AddedItemsSP.Children.Count > 0)
                {
                    AddedItemsSP.Children.Remove(sender as AdventureAugment);
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private async void PowerOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (allPowers != null)
                {
                    AdventurePower? power = allPowers.FirstOrDefault(item =>
                        item.PowerCode
                        == (
                            (sender as AdventureAugment)!.DataContext as AugmentObject
                        )!.AugmentImagePath!.Substring(
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('/') + 1,
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('.')
                                - 1
                                - (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath!.LastIndexOf('/')
                        )
                    );

                    if (power != null)
                    {
                        AdventureAugment newPower = new AdventureAugment(
                            new AugmentObject
                            {
                                AugmentImage = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImage,
                                AugmentImagePath = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath,
                                AugmentWidth = 250,
                                AugmentName = power.Name,
                                AugmentText = power.DescriptionRaw,
                                AugmentTextWidth = "4*",
                                AugmentCode = power.PowerCode
                            }
                        );
                        newPower.MouseLeftButtonDown += NewPower_MouseLeftButtonDown;
                        PowersSP.Children.Add(newPower);
                        Adventure.Powers.Add(power);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private void NewPower_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (PowersSP.Children.Count > 0)
                {
                    PowersSP.Children.Remove(sender as AdventureAugment);
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private void RelicOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (allRelics != null)
                {
                    Relic? selected = allRelics.FirstOrDefault(relic =>
                        relic.RelicCode
                        == (
                            (sender as AdventureAugment)!.DataContext as AugmentObject
                        )!.AugmentImagePath!.Substring(
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('/') + 1,
                            (
                                (sender as AdventureAugment)!.DataContext as AugmentObject
                            )!.AugmentImagePath!.LastIndexOf('.')
                                - 1
                                - (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath!.LastIndexOf('/')
                        )
                    );
                    if (selected != null && ChosenCard != null)
                    {
                        AdventureAugment newRelic = new AdventureAugment(
                            new AugmentObject
                            {
                                AugmentImage = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImage,
                                AugmentImagePath = (
                                    (sender as AdventureAugment)!.DataContext as AugmentObject
                                )!.AugmentImagePath,
                                AugmentWidth = 250,
                                AugmentName = selected.Name,
                                AugmentText = selected.DescriptionRaw,
                                AugmentTextWidth = "4*",
                                AugmentCode = selected.RelicCode
                            }
                        );
                        //ChosenCard.Attachments.Add(selected);
                        if (ChosenCardId > -1)
                            (Cards.ElementAt((int)ChosenCardId) as POCCard)!.Attachments.Add(
                                selected
                            );
                        newRelic.MouseLeftButtonDown += NewRelic_MouseLeftButtonDown;
                        AddedRelicsSP.Children.Add(newRelic);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private void NewRelic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AddedRelicsSP.Children.Count > 0)
                {
                    AddedRelicsSP.Children.Remove(sender as AdventureAugment);
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
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
                /*BitmapImage image = new BitmapImage();
                Uri? uri;
                image.BeginInit();
                image.UriSource = Uri.TryCreate(path, UriKind.Relative, out uri) ? uri : new Uri(@"");
                image.EndInit();*/

                result = bitmapSource;
                //Trace.WriteLine("Succesfully got image source");
                return true;
            }
            catch (FileNotFoundException error)
            {
                Trace.WriteLine($"File {path} was not found.");
                result = null;
                logger?.LogMessage(error.Message, MessageType.Error, error.GetType().Name);
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PowersTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(PowersTB.Text) && allPowers != null)
                {
                    bool IsSubtypeMatched = false;
                    AllPowersGrid.Children.Clear();

                    if (IsSubtypeMatched)
                    {
                        // TODO: check by rarity type
                    }
                    else
                    {
                        BitmapSource? source = null;
                        int addedItems = 0;
                        for (int i = 0; i < allPowers.Count; i++)
                        {
                            if (
                                allPowers[i]
                                    .Name.ToLower()
                                    .StartsWith(PowersTB.Text.Trim().ToLower())
                            )
                            {
                                AdventureAugment augment = new AdventureAugment(
                                    new AugmentObject
                                    {
                                        AugmentImage = GetImageSource(
                                            $"./assets/files/adventure-pl_pl/pl_pl/img/powers/{allPowers[i].PowerCode}.png",
                                            out source
                                        )
                                            ? source
                                            : null,
                                        AugmentImagePath =
                                            $"./assets/files/adventure-pl_pl/pl_pl/img/powers/{allPowers[i].PowerCode}.png",
                                        AugmentWidth = 50,
                                        AugmentTextWidth = "0",
                                        AugmentText = ""
                                    }
                                );
                                augment.MouseLeftButtonDown += PowerOnMouseLeftButtonDown;
                                AllPowersGrid.Children.Add(augment);
                                Grid.SetRow(augment, addedItems / 6);
                                Grid.SetColumn(augment, addedItems % 6);
                                addedItems++;
                            }
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(PowersTB.Text))
                {
                    AllPowersGrid.Children.Clear();

                    AddPowersToGrid();
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private void DeckSV_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                var mouseWheelEventArgs = new MouseWheelEventArgs(
                    e.MouseDevice,
                    e.Timestamp,
                    e.Delta
                );
                mouseWheelEventArgs.RoutedEvent = MouseWheelEvent;
                mouseWheelEventArgs.Source = sender;
                DeckSV.RaiseEvent(mouseWheelEventArgs);
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                logger?.LogMessage(ex.Message, MessageType.Warning, messageType: ex.GetType().Name);
            }
        }

        private async void ItemsCommonBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allItems == null)
                {
                    throw new Exception();
                }

                await FilterItemsByRarityAsync("Common");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void ItemsCommonBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ResetGridAsync(AllItemsGrid, ItemIcons);
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void ItemsRareBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allItems == null)
                {
                    throw new Exception();
                }

                await FilterItemsByRarityAsync("Rare");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void ItemsRareBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ResetGridAsync(AllItemsGrid, ItemIcons);
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void ItemsEpicBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allItems == null)
                {
                    throw new Exception();
                }

                await FilterItemsByRarityAsync("Epic");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void ItemsEpicBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ResetGridAsync(AllItemsGrid, ItemIcons);
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async Task FilterItemsByRarityAsync(string rarityRef)
        {
            try
            {
                AllItemsGrid.Children.Clear();
                int i = 0;

                foreach (Item item in allItems.Where((item) => item.RarityRef == rarityRef))
                {
                    AdventureAugment augment = new AdventureAugment(
                        ItemIcons?.FirstOrDefault((icon) => icon?.AugmentCode == item.ItemCode)
                            ?? new AugmentObject()
                    );

                    AllItemsGrid.Children.Add(augment);
                    Grid.SetRow(augment, i / AllItemsGrid.RowDefinitions.Count);
                    Grid.SetColumn(augment, i % AllItemsGrid.ColumnDefinitions.Count);
                    i++;
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async Task FilterRelicsByRarityAsync(string rarityRef)
        {
            try
            {
                AllRelicsGrid.Children.Clear();
                int i = 0;

                foreach (Relic relic in allRelics.Where((relic) => relic.RarityRef == rarityRef))
                {
                    AdventureAugment augment = new AdventureAugment(
                        RelicIcons?.FirstOrDefault((icon) => icon?.AugmentCode == relic.RelicCode)
                            ?? new AugmentObject()
                    );
                    AllRelicsGrid.Children.Add(augment);
                    Grid.SetRow(augment, i / AllRelicsGrid.RowDefinitions.Count);
                    Grid.SetColumn(augment, i % AllRelicsGrid.RowDefinitions.Count);
                    i++;
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void RelicsCommonBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allRelics == null)
                {
                    throw new Exception(
                        "Cannot show common relics. There has been a problem with the allRelics object"
                    );
                }

                await FilterRelicsByRarityAsync("Common");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void RelicsCommonBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            await ResetGridAsync(grid: AllRelicsGrid, items: RelicIcons);
        }

        private async void RelicsRareBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allRelics == null)
                {
                    throw new Exception(
                        "Cannot show rare relics. There has been a problem with the allRelics object"
                    );
                }

                await FilterRelicsByRarityAsync("Rare");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        public async void RelicsRareBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ResetGridAsync(grid: AllRelicsGrid, items: RelicIcons);
            }
            catch (System.Exception ex)
            {
                await logger?.LogMessage(ex.Message, MessageType.Error, ex.GetType().ToString());
            }
        }

        private async void RelicsEpicBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allRelics == null)
                {
                    throw new Exception(
                        "Cannot show epic relics. There has been a problem with the allRelics object"
                    );
                }

                await FilterRelicsByRarityAsync("Epic");
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.Show();
                await logger?.LogMessage(
                    ex.Message,
                    MessageType.Warning,
                    messageType: ex.GetType().Name
                );
                throw;
            }
        }

        private async void RelicsEpicBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                await ResetGridAsync(grid: AllRelicsGrid, items: RelicIcons);
            }
            catch (System.Exception ex)
            {
                await logger?.LogMessage(ex.Message, MessageType.Warning, ex.GetType().ToString());
                throw;
            }
        }

        private async Task ResetGridAsync(Grid grid, List<AugmentObject?>? items)
        {
            try
            {
                int i = 0;
                foreach (var item in items ?? new List<AugmentObject?>())
                {
                    AdventureAugment adventureAugment = new AdventureAugment(
                        item ?? new AugmentObject()
                    );
                    grid.Children.Add(adventureAugment);
                    Grid.SetRow(adventureAugment, i / grid.RowDefinitions.Count);
                    Grid.SetColumn(adventureAugment, i % grid.ColumnDefinitions.Count);
                    i++;
                }
            }
            catch (System.Exception ex)
            {
                if (logger != null)
                    await logger.LogMessage(ex.Message, MessageType.Error, ex.GetType().ToString());
            }
        }

        private async void ItemsFilterTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ItemsFilterTB.Text) && allItems != null)
                {
                    //
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private async void RelicsFilterTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
