using System.Windows.Controls;

namespace desktop.pages
{
    public partial class InGamePage : UserControl
    {
        public InGamePage()
        {
            InitializeComponent();
        }
    }

    /*
    Cards adding logic from old InGamePage

    for (int i = 0; i < cards.Count; i++)
    {
        ListBoxItemCard newItem = new ListBoxItemCard();
        ListBoxItemCard item = new ListBoxItemCard();
        newItem.DataContext = cards[i];
        newItem.Style =
            Application.Current.FindResource("CardItem") as Style;
        newItem.Background = (LinearGradientBrush)
            mergedDict[cards[i].Region + "Region"];
        newItem.Height = cardListedHeight;
        newItem.Foreground = new SolidColorBrush(Colors.White);
        newItem.FontWeight = FontWeights.Bold;
        newItem.PreviewMouseLeftButtonDown += CardItem_MouseLeftButtonDown;
        newItem.SetIndex(i);
        cardsLB.Items.Add(newItem);
        item.Content = new TextBox
        {
            AcceptsReturn = true,
            Text =
                $"{cards[i].CardId} {cards[i].CardCode} {cards[i].Name}  {cards[i].CopiesInDeck - cards[i].CopiesRemaining}"
        };
        item.DataContext = cards[i];
        item.Height = 40;
        item.Foreground = new SolidColorBrush(Colors.White);
        item.FontWeight = FontWeights.Bold;
        item.SetIndex(i);
        allCardsLB.Items.Add(item);
    }
    */
}
