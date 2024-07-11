using desktop.data.Models;
using Discord;
using LoRAPI.Controllers;
using LoRAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace desktop
{
    public class LoRApiPoller
    {
        /**
         * Game states:
         * - Menu (player is neither searching for a pvp game, nor in any PoC adventure) (positional rectangles status Menus + static decklist null)
         * - In Adventure (player started or is continuing an adventure and is not in game) (positional rectangles status Menus + static decklist deck)
         * - In Game PVP (player is in a pvp game) (positional rectangles status InProgess + OpponentName doesn't start with card)
         * - In Game PVE (player is in a pve game) (positional rectangles status InProgess + OpponentName starts with decks)
         * - In Game PoC (player is in a poc game) (positional rectangles status InProgess + OpponentName doesn't start with card + can only be set after In Adventure was set)
         * - Game Ended (player won or lost a game) (positional rectangles status Menus + game result new entry)
         */
        private GameState GameState { get; set; }
        private List<Match> MatchList { get; set; } = new List<Match>();
        public ILoRApiHandler? LoRApi { get; set; }
        private Stopwatch MatchTimer { get; set; } = new Stopwatch();

        /// <summary>
        /// Initialize the data to start processing
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="cardPositions"></param>
        /// <param name="gameResult"></param>
        public void InitialData(Deck deck, CardPositions cardPositions, GameResult gameResult, ILoRApiHandler loR)
        {
            LoRApi = loR;
            if (cardPositions.GameState == "Menus")
            {
                if (deck.DeckCode == null && deck.CardsInDeck == null)
                {
                    GameState = GameState.Menu;
                }
                else if (deck.DeckCode != null)
                {
                    GameState = GameState.InAdventure;
                }
                else if ((GameState == GameState.InGamePVP || GameState == GameState.InGamePVE || GameState == GameState.InGamePOC) && gameResult.GameID != -1)
                {
                    GameState = GameState.GameEnded;
                }
            }
            else if (cardPositions.GameState == "InProgress")
            {
                MatchTimer.Start();
                if (cardPositions?.OpponentName?.StartsWith("decks_") ?? false)
                {
                    GameState = GameState.InGamePVE;
                }
                else if (cardPositions?.OpponentName?.StartsWith("card_") ?? false)
                {
                    GameState = GameState.InGamePOC;
                }
                else
                {
                    GameState = GameState.InGamePVP;
                }
            }
            else GameState = GameState.Unknown;
        }

        public async Task<string> LoRApiProcess()
        {
            if (LoRApi == null)
            {
                throw new NullReferenceException("Error. Can't connect to API");
            }

            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1500));
            // based on game state the task should poll different endpoints and inform of different changes

            while (await timer.WaitForNextTickAsync())
            {
                switch (GameState)
                {
                    case GameState.Menu:
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        break;
                    case GameState.InAdventure:
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        break;
                    case GameState.InGamePVP:
                        MatchTimer.Start();
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        break;
                    case GameState.InGamePVE:
                        MatchTimer.Start();
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        break;
                    case GameState.InGamePOC:
                        MatchTimer.Start();
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        break;
                    case GameState.GameEnded:
                        MatchTimer.Stop();
                        MatchList.Add(new Match
                        {
                            
                        });
                        await LoRApi.GetDeckAsync();
                        await LoRApi.GetCardPositionsAsync();
                        await LoRApi.GetGameResultAsync();
                        break;
                    default:
                        break;
                }
            }
            return "Success";
        }

        public GameState GetGameState()
        {
            return GameState;
        }

        
    }

    /// <summary>
    ///
    /// Game states:
    /// <list type="bullet">Menu (player is neither searching for a pvp game, nor in any PoC adventure) (positional rectangles status Menus + static decklist null)</list>
    /// <list type="bullet">In Adventure (player started or is continuing an adventure and is not in game) (positional rectangles status Menus + static decklist deck)</list>
    /// <list type="bullet">In Game PVP (player is in a pvp game) (positional rectangles status InProgess + OpponentName doesn't start with card)</list>
    /// <list type="bullet">In Game PVE (player is in a pve game) (positional rectangles status InProgess + OpponentName starts with card)</list>
    /// <list type="bullet">In Game PoC (player is in a poc game) (positional rectangles status InProgess + OpponentName doesn't start with card + can only be set after In Adventure was set)</list>
    /// <list type="bullet">Game Ended (player won or lost a game) (positional rectangles status Menus + game result new entry)</list>
    /// </summary>
    public enum GameState
    {
        Menu,
        InAdventure,
        InGamePVP,
        InGamePVE,
        InGamePOC,
        GameEnded,
        Unknown
    }
}
