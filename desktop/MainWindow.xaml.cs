using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.InteropServices;
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
using data.db;
using LoRAPI.Controllers;
using LoRAPI.Models;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using static desktop.InGamePage;

namespace desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private event EventHandler<string>? Update,
            LoadedUI;
        ILoRApiHandler? loRAPI;
        HttpClient httpClient;
        ErrorLogger errorLogger;
        JoinableTaskFactory taskFactory = new JoinableTaskFactory(new JoinableTaskContext());
        ICommand getDataCommand,
            changePageCommand;
        private double originalLeft = 0,
            originalTop = 0,
            originalWidth = 0,
            originalHeight = 0;

        private LoRDbContext? loRDbContext;

        // USER SETTINGS
        private enum WindowLocation
        {
            Left,
            Right
        }

        private WindowLocation windowLocation = WindowLocation.Right;

        /// <summary>
        /// Handle to the LoR Client
        /// </summary>
        private IntPtr GameClientHandle;

        public MainWindow()
        {
            httpClient = new HttpClient();
            loRDbContext = new LoRDbContext();
            loRAPI = new LoRApiController(httpClient);
            errorLogger = new ErrorLogger();
            getDataCommand = new AsyncRelayCommand(LoadContentAsync, (ex) => ShowException(ex));
            changePageCommand = new AsyncRelayCommand(ChangePageAsync, (ex) => ShowException(ex));
            InitializeComponent();
            originalLeft = Left;
            originalTop = Top;
            originalHeight = Height;
            originalWidth = Width;
            Main.NavigationService.Navigate(new WelcomePage(loRAPI, getDataCommand, errorLogger));
            Background = WelcomePage.GetBackground();
            Update += UpdateUI;
            LoadedUI += LoadUI;
        }

        private void LoadUI(object? sender, string e)
        {
            if (e == "Loaded")
            {
                changePageCommand.Execute("Profile");
            }
        }

        private async Task LoadContentAsync(object? parameter)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            ProfilePage profilePage = new ProfilePage(loRAPI, changePageCommand, errorLogger);
            Background = ProfilePage.GetBackground();
            SpinnerGrid.Visibility = Visibility.Visible;
            LoadData();
            await profilePage.LoadDataAsync(loRDbContext);
            Main.NavigationService.Navigate(profilePage);
            profilePage.AddInitialData();
            this.Width = profilePage.Width;
            this.Height = profilePage.Height + 30;
            SpinnerGrid.Visibility = Visibility.Collapsed;
            stopwatch.Stop();
            System.Console.WriteLine("Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed);
            Trace.WriteLine("Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed);
        }

        /// <summary>
        /// Function loads the user data and game data that has been stored
        /// </summary>
        /// <returns></returns>
        private void LoadData()
        {
            if (loRDbContext == null)
            {
                loRDbContext = new LoRDbContext();
                loRDbContext.Database.EnsureCreated();
            }
        }

        private void UpdateUI(object? sender, string e)
        {
            InGamePage? page = null;
            ProfilePage? profile = null;
            try
            {
                switch (e)
                {
                    case "Profile":
                        if (profile != null)
                        {
                            Main.Content = profile;
                            Background = ProfilePage.GetBackground();
                        }
                        break;
                    case "Profile Load":
                        profile = new ProfilePage(loRAPI, changePageCommand, errorLogger);
                        Background = ProfilePage.GetBackground();
                        SpinnerGrid.Visibility = Visibility.Visible;
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        taskFactory.Run(async () =>
                        {
                            await profile.LoadDataAsync();
                            await taskFactory.SwitchToMainThreadAsync();
                            Main.NavigationService.Navigate(profile);
                            profile.AddInitialData();
                            Width = profile.Width;
                            Height = profile.Height + 30;
                            SpinnerGrid.Visibility = Visibility.Collapsed;
                        });
                        stopwatch.Stop();
                        System.Console.WriteLine(
                            "Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed
                        );
                        Trace.WriteLine("Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed);
                        break;
                    case "Loading":
                        Main.NavigationService.Navigate(
                            new LoadingPage(Task.CompletedTask, changePageCommand, errorLogger)
                        );
                        Background = LoadingPage.GetBackground();
                        break;
                    case "InGame Load":
                        page = new InGamePage(loRAPI, changePageCommand, loRDbContext, errorLogger);
                        page.LoRDbContext = loRDbContext;
                        SpinnerGrid.Visibility = Visibility.Visible;
                        Background = GetBackground();

                        try
                        {
                            GameClientHandle = FindWindowA(null, "Legends of Runeterra");

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                            LRect clientPosition = new LRect(0, 0, 0, 0);
                            bool didGetWindowRect = false;
                            if (GameClientHandle != null)
                            {
                                didGetWindowRect = GetWindowRect(
                                    GameClientHandle,
                                    out clientPosition
                                );
                            }
                            Stopwatch stopwatchInGameScreen = Stopwatch.StartNew();
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                            taskFactory.Run(async () =>
                            {
                                await page.LoadDataAsync();
                                await page.StartAsync();
                                await taskFactory.SwitchToMainThreadAsync();
                                Main.NavigationService.Navigate(page);
                                page.SetHeight(
                                    (clientPosition.left != 0 && clientPosition.right != 0)
                                        ? clientPosition.bottom - clientPosition.top
                                        : page.Height + 30
                                );
                                SpinnerGrid.Visibility = Visibility.Collapsed;

                                Height =
                                    (clientPosition.left != 0 && clientPosition.right != 0)
                                        ? clientPosition.bottom - clientPosition.top
                                        : page.Height + 30;
                                if (didGetWindowRect)
                                {
                                    if (windowLocation == WindowLocation.Right)
                                    {
                                        // Set window to the right of the game client

                                        bool isMoved = MoveWindow(
                                            FindWindowA(null, "MainWindow"),
                                            clientPosition.right,
                                            clientPosition.top,
                                            (int)Width,
                                            clientPosition.bottom - clientPosition.top,
                                            true
                                        );
                                        //Left = clientPosition.right;
                                        //Top = clientPosition.top;
                                        Console.WriteLine(isMoved);
                                    }
                                    else
                                    {
                                        // Set window to the left of the game client

                                        bool isMoved = MoveWindow(
                                            FindWindowA(null, "LoRHelper"),
                                            (int)(clientPosition.left - this.Width),
                                            (int)clientPosition.top,
                                            (int)Width,
                                            (int)Height,
                                            true
                                        );

                                        Console.WriteLine(isMoved);
                                    }
                                }
                            });
                            stopwatchInGameScreen.Stop();
                            System.Console.WriteLine(
                                "Time elapsed ${stopwatchInGameScreen.Elapsed} "
                                    + stopwatchInGameScreen.Elapsed
                            );
                            Trace.WriteLine(
                                "Time elapsed ${stopwatchInGameScreen.Elapsed} "
                                    + stopwatchInGameScreen.Elapsed
                            );
                        }
                        catch (System.Exception error)
                        {
                            Trace.WriteLine(error.Message);
                            throw;
                        }
                        break;
                    case "InGame":
                        if (page != null)
                        {
                            Main.Content = page;
                            Background = GetBackground();
                        }
                        break;
                    default:
                        System.Console.WriteLine("Nothing to update");
                        break;
                }
            }
            catch (InvalidOperationException error)
            {
                Trace.WriteLine(error.Message);
                CustomMessageBox messageBox = new CustomMessageBox(error.Message);
                messageBox.ShowDialog();
            }
            catch (Exception ex)
            {
                taskFactory.Run(
                    async () => await errorLogger.LogMessage(ex.ToString(), MessageType.Error)
                );
                throw;
            }
        }

        private async Task ChangePageAsync(object? param)
        {
            InGamePage? page = null;
            ProfilePage? profile = null;

            try
            {
                try
                {
                    switch (param)
                    {
                        case "Profile":
                            if (profile != null)
                            {
                                Main.NavigationService.Navigate(profile);
                                Background = ProfilePage.GetBackground();
                            }
                            break;
                        case "Profile Load":
                            profile = new ProfilePage(loRAPI, changePageCommand, errorLogger);
                            Background = ProfilePage.GetBackground();
                            SpinnerGrid.Visibility = Visibility.Visible;
                            Stopwatch stopwatch = Stopwatch.StartNew();
                            await taskFactory.RunAsync(async () =>
                            {
                                await profile.LoadDataAsync();
                                await taskFactory.SwitchToMainThreadAsync();
                                Main.NavigationService.Navigate(profile);
                                profile.AddInitialData();
                                this.Width = profile.Width;
                                this.Height = profile.Height + 30;
                                SpinnerGrid.Visibility = Visibility.Collapsed;
                            });
                            stopwatch.Stop();
                            System.Console.WriteLine(
                                "Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed
                            );
                            Trace.WriteLine(
                                "Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed
                            );
                            break;
                        case "Loading":
                            Main.NavigationService.Navigate(
                                new LoadingPage(Task.CompletedTask, changePageCommand, errorLogger)
                            );
                            Background = LoadingPage.GetBackground();
                            break;
                        case "InGame Load":
                            Main.Width = 300;
                            page = new InGamePage(loRAPI, changePageCommand, null, errorLogger);
                            page.Width = 300;
                            page.LoRDbContext = loRDbContext;
                            Background = GetBackground();
                            SpinnerGrid.Visibility = Visibility.Visible;

                            try
                            {
                                GameClientHandle = FindWindowA(null, "Legends of Runeterra");

#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                LRect clientPosition = new LRect(0, 0, 0, 0);
                                bool didGetWindowRect = false;
                                if (GameClientHandle != null)
                                {
                                    didGetWindowRect = GetWindowRect(
                                        GameClientHandle,
                                        out clientPosition
                                    );
                                }
                                Stopwatch stopwatchInGameScreen = Stopwatch.StartNew();
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                                await taskFactory.RunAsync(async () =>
                                {
                                    await page.LoadDataAsync();
                                    await page.StartAsync();
                                    await taskFactory.SwitchToMainThreadAsync();
                                    Main.NavigationService.Navigate(page);
                                    //Width = page.Width;
                                    page.SetHeight(
                                        (clientPosition.left != 0 && clientPosition.right != 0)
                                            ? clientPosition.bottom - clientPosition.top
                                            : page.Height + 30
                                    );
                                    SpinnerGrid.Visibility = Visibility.Collapsed;

                                    Height =
                                        (clientPosition.left != 0 && clientPosition.right != 0)
                                            ? clientPosition.bottom - clientPosition.top
                                            : page.Height + 30;
                                    if (didGetWindowRect)
                                    {
                                        if (windowLocation == WindowLocation.Right)
                                        {
                                            // Set window to the right of the game client

                                            bool isMoved = MoveWindow(
                                                FindWindowA(null, "MainWindow"),
                                                clientPosition.right,
                                                clientPosition.top,
                                                (int)Width,
                                                clientPosition.bottom - clientPosition.top,
                                                true
                                            );
                                            //Left = clientPosition.right;
                                            //Top = clientPosition.top;
                                            Console.WriteLine(isMoved);
                                        }
                                        else
                                        {
                                            // Set window to the left of the game client

                                            bool isMoved = MoveWindow(
                                                FindWindowA(null, "LoRHelper"),
                                                (int)(clientPosition.left - this.Width),
                                                (int)clientPosition.top,
                                                (int)Width,
                                                (int)Height,
                                                true
                                            );

                                            Console.WriteLine(isMoved);
                                        }
                                    }
                                });
                                stopwatchInGameScreen.Stop();
                                System.Console.WriteLine(
                                    "Time elapsed ${stopwatchInGameScreen.Elapsed} "
                                        + stopwatchInGameScreen.Elapsed
                                );
                                Trace.WriteLine(
                                    "Time elapsed ${stopwatchInGameScreen.Elapsed} "
                                        + stopwatchInGameScreen.Elapsed
                                );
                            }
                            catch (System.Exception error)
                            {
                                Trace.WriteLine(error.Message);
                                throw;
                            }
                            break;
                        case "InGame":
                            if (page != null)
                            {
                                Main.NavigationService.Navigate(page);
                                Background = GetBackground();
                            }
                            break;
                        default:
                            System.Console.WriteLine("Nothing to update");
                            break;
                    }
                }
                catch (InvalidOperationException error)
                {
                    Trace.WriteLine(error.Message);
                    CustomMessageBox messageBox = new CustomMessageBox(error.Message);
                    messageBox.ShowDialog();
                }
                catch (Exception ex)
                {
                    await taskFactory.RunAsync(
                        async () => await errorLogger.LogMessage(ex.ToString(), MessageType.Error)
                    );
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                ShowException(ex);
                // if (profile == null)
                // {
                //     profile = new ProfilePage(loRAPI, changePageCommand, errorLogger);
                // }
                await ChangePageAsync("Profile Load");
                // await taskFactory.RunAsync(async () =>
                // {
                //     await profile!.LoadDataAsync();
                //     profile.AddInitialData();
                //     await taskFactory.SwitchToMainThreadAsync();
                //     Main.NavigationService.Navigate(page);
                //     SpinnerGrid.Visibility = Visibility.Collapsed;
                // });
            }
        }

        public void SetWindowLocation(int location)
        {
            switch (location)
            {
                case 1:
                    windowLocation = WindowLocation.Left;
                    break;
                case 2:
                    windowLocation = WindowLocation.Right;
                    break;
                default:
                    windowLocation = WindowLocation.Right;
                    break;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(
            IntPtr hWnd,
            int x,
            int y,
            int nWidth,
            int nHeight,
            bool repaint
        );

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindowA(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out LRect lpRect);

        internal struct LRect
        {
            internal LRect(int l, int t, int r, int b)
            {
                left = l;
                top = t;
                right = r;
                bottom = b;
            }

            internal int left { get; }
            internal int top { get; }
            internal int right { get; }
            internal int bottom { get; }
        }

        protected virtual void OnUpdateRequired(string value)
        {
            try
            {
                Update?.Invoke(this, value);
            }
            catch (System.Exception ex)
            {
                ShowException(ex);
            }
        }

        protected virtual void OnLoaded(string value)
        {
            changePageCommand.Execute(value);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetVisualChild(0)
                .SetValue(StyleProperty, Application.Current.Resources["WindowBorder"]);
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task LoadCardsAsync()
        {
            try
            {
                if (loRAPI == null)
                {
                    throw new Exception("Cannot proceed, because the API is not reachable");
                }
                await loRAPI.GetAllCardsAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void ShowException(Exception ex)
        {
            CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
            messageBox.ShowDialog();
            System.Console.WriteLine(ex.Message);
            Trace.WriteLine(ex.Message);
        }
    }

    public class LoRApiTest : ILoRApiHandler
    {
        const string setsPath = "./assets/files/sets/data/setsDummy.json";

        public bool IsAdventure
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            try
            {
                List<Card>? allCards;

                using (StreamReader reader = new StreamReader(setsPath))
                {
                    var json = await reader.ReadToEndAsync();
                    allCards = JsonConvert.DeserializeObject<List<Card>>(json);

                    if (allCards != null)
                    {
                        return allCards;
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

        public Task<CardPositions> GetCardPositionsAsync()
        {
            return Task.FromResult(
                new CardPositions
                {
                    PlayerName = "Test",
                    OpponentName = "Test",
                    GameState = "Testing",
                    Screen = new Dictionary<string, int>
                    {
                        { "ScreenWidth", 1513 },
                        { "ScreenHeight", 954 }
                    },
                    Rectangles = new List<LoRAPI.Models.Rectangle>
                    {
                        new LoRAPI.Models.Rectangle
                        {
                            CardID = 1636657207,
                            CardCode = "face",
                            TopLeftX = 67,
                            TopLeftY = 425,
                            Height = 103,
                            Width = 103,
                            LocalPlayer = true
                        },
                        new LoRAPI.Models.Rectangle
                        {
                            CardID = 669259926,
                            CardCode = "02PZ010",
                            TopLeftX = 446,
                            TopLeftY = 73,
                            Height = 217,
                            Width = 156,
                            LocalPlayer = true
                        },
                        new LoRAPI.Models.Rectangle
                        {
                            CardID = 103145528,
                            CardCode = "01DE027",
                            TopLeftX = 678,
                            TopLeftY = 75,
                            Height = 217,
                            Width = 156,
                            LocalPlayer = true
                        },
                        new LoRAPI.Models.Rectangle
                        {
                            CardID = 607876141,
                            CardCode = "09DE034",
                            TopLeftX = 795,
                            TopLeftY = 229,
                            Height = 219,
                            Width = 156,
                            LocalPlayer = true
                        },
                        new LoRAPI.Models.Rectangle
                        {
                            CardID = 1624873019,
                            CardCode = "01DE011",
                            TopLeftX = 913,
                            TopLeftY = 229,
                            Height = 219,
                            Width = 156,
                            LocalPlayer = true
                        }
                    }
                }
            );
        }

        public Task<Deck> GetDeckAsync()
        {
            return Task.FromResult(
                new Deck
                {
                    DeckCode =
                        "CUBQCAIBD4BAQCQGCQBQMCQPCEKQOAIFAIKACBYKBEAQQAYDAEEAKKABBECQ4AQFBIAROAQJBIARIBABAQBQOAIFBIYQCCAADEAQQDAO",
                    CardsInDeck = new Dictionary<string, int>
                    {
                        { "01IO012", 2 },
                        { "01IO015T1", 2 },
                        { "01IO041", 3 },
                        { "04PZ016", 2 },
                        { "04PZ007", 2 },
                        { "05SI014", 2 },
                        { "05SI009", 1 }
                    }
                }
            );
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
