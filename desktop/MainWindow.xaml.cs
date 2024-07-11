using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
using LoRAPI.Controllers;
using LoRAPI.Models;

namespace desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private event EventHandler<string> update,
            loaded;
        ILoRApiHandler? loRAPI;
        HttpClient httpClient;

        public MainWindow()
        {
            httpClient = new HttpClient();
            loRAPI = new LoRApiController(httpClient);
            InitializeComponent();
            Main.Content = new WelcomePage(loRAPI, OnUpdateRequired);
            Background = WelcomePage.GetBackground();
            update += updateUI;
            loaded += loadUI;
        }

        private void loadUI(object? sender, string e)
        {
            if (e == "Loaded")
            {
                OnUpdateRequired("Profile");
            }
        }

        private void updateUI(object? sender, string e)
        {
            switch (e)
            {
                case "Profile":
                    Main.Content = new ProfilePage(loRAPI, OnUpdateRequired);
                    Background = ProfilePage.GetBackground();
                    break;
                case "Loading":
                    Main.Content = new LoadingPage(OnUpdateRequired);
                    Background = LoadingPage.GetBackground();
                    break;
                case "InGame":
                    Main.Content = new LoadingPage(OnUpdateRequired);
                    InGamePage page = new InGamePage(new LoRApiTest(), OnUpdateRequired);
                    page.LoadCards().Wait();
                    Main.Content = page;
                    Background = InGamePage.GetBackground();
                    break;
                default:
                    System.Console.WriteLine("Nothing to update");
                    break;
            }
        }

        protected virtual void OnUpdateRequired(string value)
        {
            update?.Invoke(this, value);
        }

        protected virtual void OnLoaded(string value)
        {
            update?.Invoke(this, value);
        }

        private void toolbarGrid_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }

        private void btnMiniApp_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            OnLoaded("Loaded");
        }

        private void btnCloseApp_Click (object sender, EventArgs e)
        {
            Close();
        }
    }

    public class LoRApiTest : ILoRApiHandler
    {
        public Task<CardPositions> GetCardPositionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Deck> GetDeckAsync()
        {
            return Task.FromResult(new Deck
            {
                DeckCode = "CUBQCAIBD4BAQCQGCQBQMCQPCEKQOAIFAIKACBYKBEAQQAYDAEEAKKABBECQ4AQFBIAROAQJBIARIBABAQBQOAIFBIYQCCAADEAQQDAO",
                CardsInDeck = new Dictionary<string, int> { { "01IO012", 2}, { "01IO015T1", 2 },{ "01IO041",3},{ "04PZ016", 2 },{ "04PZ007", 2 },{ "05SI014", 2 },{ "05SI009", 1 } }
            });
        }

        public Task<GameResult> GetGameResultAsync()
        {
            throw new NotImplementedException();
        }
    }

    /*public class AsyncCountdownEvent
    {
        private readonly AsyncManualResetEvent m_amre = new AsyncManualResetEvent();
        private int m_count;

        public AsyncCountdownEvent(int initialCount)
        {
            if (initialCount <= 0) throw new ArgumentOutOfRangeException("initialCount");
            m_count = initialCount;
        }

        public Task WaitAsync() { return m_amre.WaitAsync(); }

        public void Signal()
        {
            if (m_count <= 0)
                throw new InvalidOperationException();

            int newCount = Interlocked.Decrement(ref m_count);
            if (newCount == 0)
                m_amre.Set();
            else if (newCount < 0)
                throw new InvalidOperationException();
        }

    }

    public sealed class DeferralManager
    {
        private readonly AsyncCountdownEvent _count = new AsyncCountdownEvent(1);
        public IDisposable GetDeferral()
        {
            return new Deferral(_count);
        }

        public Task SignalAndWaitAsync()
        {
            return Task.CompletedTask;
        }

        public sealed class Deferral : IDisposable
        {
            private readonly AsyncCountdownEvent _countdown;
            public Deferral(AsyncCountdownEvent countdownEvent)
            {
                _countdown = countdownEvent;
            }

            void IDisposable.Dispose()
            {
                //
            }
        }
    }*/

}
