using System.Threading.Tasks;
using LoRAPI.Controllers;
using Moq;
using Xunit;

namespace LoR_Helper_UnitTests;

public class LoRApiTests
{
    LoRAPI.Models.Deck expectedDeck;
    Mock<ILoRApiHandler> mock;

    public LoRApiTests()
    {
        mock = new Mock<ILoRApiHandler>();
        expectedDeck = new LoRAPI.Models.Deck();
        mock.Setup(p => p.GetDeckAsync()).ReturnsAsync(expectedDeck);
    }

    [Fact]
    public async Task GetDeckAsync_SuccessfullyReturns()
    {
        ILoRApiHandler handler = mock.Object;
        LoRAPI.Models.Deck deck = await handler.GetDeckAsync();

        Assert.Equal(deck, expectedDeck);
    }
}

// public class LoRApiTest : ILoRApiHandler
// {
//     const string setsPath = "./assets/files/sets/data/setsDummy.json";

//     public bool IsAdventure
//     {
//         get => throw new NotImplementedException();
//         set => throw new NotImplementedException();
//     }

//     public async Task<IEnumerable<Card>> GetAllCardsAsync()
//     {
//         try
//         {
//             List<Card>? allCards;

//             using (StreamReader reader = new StreamReader(setsPath))
//             {
//                 var json = await reader.ReadToEndAsync();
//                 allCards = JsonConvert.DeserializeObject<List<Card>>(json);

//                 if (allCards != null)
//                 {
//                     return allCards;
//                 }
//                 else
//                 {
//                     throw new NullReferenceException("Cards are null.");
//                 }
//             }
//         }
//         catch (Exception)
//         {
//             throw;
//         }
//     }

//     public Task<CardPositions> GetCardPositionsAsync()
//     {
//         return Task.FromResult(
//             new CardPositions
//             {
//                 PlayerName = "Test",
//                 OpponentName = "Test",
//                 GameState = "Testing",
//                 Screen = new Dictionary<string, int>
//                 {
//                     { "ScreenWidth", 1513 },
//                     { "ScreenHeight", 954 }
//                 },
//                 Rectangles = new List<LoRAPI.Models.Rectangle>
//                 {
//                     new LoRAPI.Models.Rectangle
//                     {
//                         CardID = 1636657207,
//                         CardCode = "face",
//                         TopLeftX = 67,
//                         TopLeftY = 425,
//                         Height = 103,
//                         Width = 103,
//                         LocalPlayer = true
//                     },
//                     new LoRAPI.Models.Rectangle
//                     {
//                         CardID = 669259926,
//                         CardCode = "02PZ010",
//                         TopLeftX = 446,
//                         TopLeftY = 73,
//                         Height = 217,
//                         Width = 156,
//                         LocalPlayer = true
//                     },
//                     new LoRAPI.Models.Rectangle
//                     {
//                         CardID = 103145528,
//                         CardCode = "01DE027",
//                         TopLeftX = 678,
//                         TopLeftY = 75,
//                         Height = 217,
//                         Width = 156,
//                         LocalPlayer = true
//                     },
//                     new LoRAPI.Models.Rectangle
//                     {
//                         CardID = 607876141,
//                         CardCode = "09DE034",
//                         TopLeftX = 795,
//                         TopLeftY = 229,
//                         Height = 219,
//                         Width = 156,
//                         LocalPlayer = true
//                     },
//                     new LoRAPI.Models.Rectangle
//                     {
//                         CardID = 1624873019,
//                         CardCode = "01DE011",
//                         TopLeftX = 913,
//                         TopLeftY = 229,
//                         Height = 219,
//                         Width = 156,
//                         LocalPlayer = true
//                     }
//                 }
//             }
//         );
//     }

//     public Task<Deck> GetDeckAsync()
//     {
//         return Task.FromResult(
//             new Deck
//             {
//                 DeckCode =
//                     "CUBQCAIBD4BAQCQGCQBQMCQPCEKQOAIFAIKACBYKBEAQQAYDAEEAKKABBECQ4AQFBIAROAQJBIARIBABAQBQOAIFBIYQCCAADEAQQDAO",
//                 CardsInDeck = new Dictionary<string, int>
//                 {
//                     { "01IO012", 2 },
//                     { "01IO015T1", 2 },
//                     { "01IO041", 3 },
//                     { "04PZ016", 2 },
//                     { "04PZ007", 2 },
//                     { "05SI014", 2 },
//                     { "05SI009", 1 }
//                 }
//             }
//         );
//     }

//     public Task<GameResult> GetGameResultAsync()
//     {
//         throw new NotImplementedException();
//     }
// }
