using System.Diagnostics;
using System.Net.Http.Json;
using Discord;
using LoRAPI.Models;

namespace LoRAPI.Controllers
{
    public class LoRApiController : ILoRApiHandler
    {
        HttpClient client;
        HttpResponseMessage? responseMessage;
        int? port;

        public LoRApiController(HttpClient httpClient)
        {
            this.client = httpClient;
            port = 21337;
        }

        // path static-decklist
        public async Task<Deck> GetDeckAsync()
        {            
            try
            {
                Deck? deck = null;
                if (port == null)
                    throw new NullReferenceException("Deck is null");
                responseMessage = await client.GetAsync(
                    $"http://localhost:{port}/static-decklist"
                );
                if (responseMessage.IsSuccessStatusCode)
                {
                    deck = await responseMessage.Content.ReadFromJsonAsync<Deck>();
                }
                return deck ?? new Deck();
            }
            catch (HttpRequestException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetDeckAsync.");
                Trace.WriteLine(error.Message.ToString());
                throw error;
            }
            catch (System.Exception)
            {
                Trace.WriteLine("Wystąpił błąd w GetDeckAsync.");
                throw;
            }
            finally
            {
                if (responseMessage != null)
                {
                    responseMessage.Dispose();                    
                }
            }
        }

        public async Task<CardPositions> GetCardPositionsAsync()
        {
            try
            {
                CardPositions? cardPositions = null;
                if (port == null)
                    throw new NullReferenceException("Deck is null");
                responseMessage = await client.GetAsync(
                    $"http://localhost:{port}/positional-rectangles"
                );
                if (responseMessage.IsSuccessStatusCode)
                {
                    cardPositions =
                        await responseMessage.Content.ReadFromJsonAsync<CardPositions>();
                }
                return cardPositions ?? new CardPositions();
            } 
            catch (HttpRequestException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                //return new CardPositions();
                throw error;
            }
            catch (System.Exception)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                throw;
            }
            finally
            {
                if (responseMessage != null)
                {
                    responseMessage.Dispose();
                }
            }
        }

        public async Task<GameResult> GetGameResultAsync()
        {
            try
            {
                GameResult? gameResult = null;
                if (port == null)
                    throw new NullReferenceException("Deck is null");
                responseMessage = await client.GetAsync($"http://localhost:{port}/positional-rectangles");
                if (responseMessage.IsSuccessStatusCode)
                {
                    gameResult = await responseMessage.Content.ReadFromJsonAsync<GameResult>();
                }
                return gameResult ?? new GameResult();
            }
            catch (HttpRequestException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetGameResultAsync.");
                Trace.WriteLine(error.Message.ToString());
                //return new GameResult();
                throw error;
            }
            catch (System.Exception)
            {
                Trace.WriteLine("Wystąpił błąd w GetGameResultAsync.");
                throw;
            }
        }

        public void SetPort(int portNumber)
        {
            this.port = portNumber;
        }

        public bool IsReady()
        {
            if (port == null)
                return false;
            return true;
        }
    }
}
