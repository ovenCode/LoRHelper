using System;
using System.Collections.Generic;
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

namespace desktop
{
    /// <summary>
    /// Interaction logic for CardPreview.xaml
    /// </summary>
    public partial class CardPreview : UserControl
    {
        /// <summary>
        /// CardPreview constructor
        /// </summary>
        /// <param name="card">Card data from the card pool</param>
        /// <param name="currentCard">Card data from current game</param>
        /// <param name="cardHeight">Height of card to be displayed</param>
        public CardPreview(ICard card, ICard currentCard, int cardHeight = 0)
        {
            InitializeComponent();
            DataContext = card;
            if (cardHeight != 0)
            {
                CardImage.Height = cardHeight;
            }
            AddCardItems(currentCard);
        }

        private bool AddCardItems(ICard card)
        {
            try
            {
                if (card.GetType() == typeof(POCCard) && (card as POCCard)!.Attachments.Count > 0)
                {
                    POCCard pocCard = (POCCard)card;
                    List<Relic?> relics = pocCard
                        .Attachments.Where(att => att is Relic)
                        .Select(el => el as Relic)
                        .ToList();
                    List<Item?> items = pocCard
                        .Attachments.Where(att => att is Item)
                        .Select(el => el as Item)
                        .ToList();
                    List<AugmentObject> objects = new List<AugmentObject>();
                    AugmentObject? item;

                    for (int i = 0; i < relics.Count; i++)
                    {
                        if (relics[i] != null)
                            CardRelics.Children.Add(
                                new AdventureAugment(
                                    AugmentObject.TryParse(
                                        relics[i]!,
                                        relics[i]!.RelicCode,
                                        out item
                                    )
                                        ? item!
                                        : new AugmentObject()
                                )
                            );
                    }
                    CardRelics.Visibility = Visibility.Visible;

                    for (int i = 0; i < items.Count; i++)
                    {
                        if (items[i] != null)
                            CardItems.Children.Add(
                                new AdventureAugment(
                                    AugmentObject.TryParse(items[i]!, items[i]!.ItemCode, out item)
                                        ? item!
                                        : new AugmentObject()
                                )
                            );
                    }
                    CardItems.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
    }
}
