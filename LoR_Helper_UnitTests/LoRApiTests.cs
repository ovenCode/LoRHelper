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
