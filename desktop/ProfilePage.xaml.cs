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
using data.Hubs;
using LoRAPI.Controllers;
using LoRAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace desktop
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        List<Match> matches = new List<Match>();
        LoRApiController? loR;
        private readonly IHubContext<LoRHub>? hub;

        public ProfilePage(LoRAPI.Controllers.LoRApiController? loRAPI)
        {
            loR = loRAPI;
            InitializeComponent();
            LoadData().GetAwaiter().GetResult();
        }

        private async Task LoadData()
        {
            try
            {
                CardPositions? cardPositions = await loR?.GetCardPositionsAsync()! ?? null;
                Deck? deck = await loR?.GetDeckAsync()! ?? null;
                GameResult? gameResult = await loR?.GetGameResultAsync()! ?? null;

                matches.Add(
                    new Match
                    {
                        Id = gameResult?.GameID ?? -1,
                        IsWin = false,
                        DeckCode = deck?.DeckCode ?? "Błąd",
                        Opponent = cardPositions?.OpponentName ?? "Błąd",
                        GameType = GameType.PathOfChampions
                    }
                );

                matchesDB.DataContext = matches;
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Wystąpił błąd");
                throw;
            }
        }
    }
}
