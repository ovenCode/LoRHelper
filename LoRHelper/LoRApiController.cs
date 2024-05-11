using System.Net.Http.Json;
using Discord;

class LoRApiController
{
    HttpClient client;
    int? port;

    public LoRApiController(HttpClient httpClient)
    {
        this.client = httpClient;
        port = 21337;
    }

    // path static-decklist
    public async Task<Deck> GetDeckAsync()
    {
        Deck? deck = null;
        if (port == null)
            throw new NullReferenceException("Deck is null");
        HttpResponseMessage responseMessage = await client.GetAsync(
            $"http://localhost:{port}/static-decklist"
        );
        if (responseMessage.IsSuccessStatusCode)
        {
            deck = await responseMessage.Content.ReadFromJsonAsync<Deck>();
        }
        return deck ?? new Deck();
    }

    public async Task<CardPositions> GetCardPositionsAsync()
    {
        CardPositions? cardPositions = null;
        if (port == null)
            throw new NullReferenceException("Deck is null");
        HttpResponseMessage responseMessage = await client.GetAsync(
            $"http://localhost:{port}/positional-rectangles"
        );
        if (responseMessage.IsSuccessStatusCode)
        {
            cardPositions = await responseMessage.Content.ReadFromJsonAsync<CardPositions>();
        }
        return cardPositions ?? new CardPositions();
    }

    public async Task<GameResult> GetGameResultAsync()
    {
        GameResult? gameResult = null;
        if (port == null)
            throw new NullReferenceException("Deck is null");
        HttpResponseMessage responseMessage = await client.GetAsync(
            $"http://localhost:{port}/positional-rectangles"
        );
        if (responseMessage.IsSuccessStatusCode)
        {
            gameResult = await responseMessage.Content.ReadFromJsonAsync<GameResult>();
        }
        return gameResult ?? new GameResult();
    }
}
