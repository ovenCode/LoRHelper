using System.Diagnostics;
using System.Net.Http.Json;
using Discord;
using LoRAPI.Models;
using Newtonsoft.Json;

namespace LoRAPI.Controllers
{
    public class LoRApiController : ILoRApiHandler
    {
        HttpClient client;
        HttpResponseMessage? responseMessage;
        int? port;
        const string setsPath = "./assets/files/sets/data/setsDummy.json";

        public LoRApiController(HttpClient httpClient)
        {
            this.client = httpClient;
            //client.Timeout = TimeSpan.FromSeconds(30);
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
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.IsSuccessStatusCode)
                {
                    deck = await responseMessage.Content.ReadFromJsonAsync<Deck>();
                }
                return deck ?? new Deck();
            }
            catch (ObjectDisposedException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetDeckAsync.");
                Trace.WriteLine(error.Message.ToString());
                throw error;
            }
            catch (HttpRequestException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetDeckAsync.");
                Trace.WriteLine(error.Message.ToString());
                throw error;
            }
            catch (Exception error)
            {
                Trace.WriteLine("Wystąpił błąd w GetDeckAsync.");
                Trace.WriteLine(error.Message.ToString());
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

        /// <summary>
        /// Async function requesting with http GetAsync localhost endpoint
        /// </summary>
        /// <returns>Task with CardPositions</returns>
        /// <exception cref="HttpRequestException" />
        /// <exception cref="NullReferenceException" />
        /// <exception cref="InvalidOperationException" />
        /// <exception cref="UriFormatException" />
        /// <exception cref="TaskCanceledException" />
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
            catch (InvalidOperationException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                //return new CardPositions();
                throw error;
            }
            catch (TaskCanceledException error)
            {
                Trace.WriteLine("Wystąpił błąd w GetCardPositionsAsync.");
                Trace.WriteLine(error.Message.ToString());
                //return new CardPositions();
                throw error;
            }
            catch (UriFormatException error)
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
                responseMessage = await client.GetAsync($"http://localhost:{port}/game-result");
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
            if(portNumber != 0)
            {
                port = portNumber;
            }
            
        }

        public bool IsReady()
        {
            if (port == null)
                return false;
            return true;
        }

        public Task<IEnumerable<Card>> GetAllCards()
        {
            try
            {
                List<Card>? allCards;

                using (StreamReader reader = new StreamReader(setsPath))
                {
                    var json = reader.ReadToEnd();
                    allCards = JsonConvert.DeserializeObject<List<Card>>(json);

                    if (allCards != null)
                    {
                        return Task.FromResult((IEnumerable<Card>)allCards);
                    }
                    else
                    {
                        throw new NullReferenceException("Cards are null.");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
