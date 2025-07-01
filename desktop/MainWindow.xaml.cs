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
using desktop.Commands;
using desktop.data.db;
using desktop.data.Models;
using desktop.Services;
using desktop.Stores;
using desktop.utils;
using desktop.ViewModels;
using LoRAPI.Controllers;
using LoRAPI.Models;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;

namespace desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private event EventHandler<string>? Update;

        // private double originalLeft = 0,
        //     originalTop = 0,
        //     originalWidth = 0,
        //     originalHeight = 0;

        // private ILoRDbContextFactory? loRDbContextFactory;

        // USER SETTINGS
        // private enum WindowLocation
        // {
        //     Left,
        //     Right
        // }

        // private WindowLocation windowLocation = WindowLocation.Right;

        /// <summary>
        /// Handle to the LoR Client
        /// </summary>
        // private IntPtr GameClientHandle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadUI(object? sender, string e)
        {
            if (e == "Loaded")
            {
                // changePageCommand?.Execute("Profile");
            }
        }

        // private async Task LoadContentAsync(object? parameter)
        // {
        //     await taskFactory.RunAsync(() =>
        //     {
        //         Console.WriteLine();
        //         return Task.CompletedTask;
        //     });
        // }

        // private async Task ChangePageAsync(object? param)
        // {
        //     await taskFactory.RunAsync(() =>
        //     {
        //         Console.WriteLine();
        //         return Task.CompletedTask;
        //     });
        //     //             InGamePage? page = null;
        //     //             ProfilePage? profile = null;

        //     //             try
        //     //             {
        //     //                 try
        //     //                 {
        //     //                     switch (param)
        //     //                     {
        //     //                         case "Profile":
        //     //                             if (profile != null)
        //     //                             {
        //     //                                 Main.NavigationService.Navigate(profile);
        //     //                                 Background = ProfilePage.GetBackground();
        //     //                             }
        //     //                             break;
        //     //                         case "Profile Load":
        //     //                             profile = new ProfilePage(loRAPI, changePageCommand, errorLogger);
        //     //                             Background = ProfilePage.GetBackground();
        //     //                             SpinnerGrid.Visibility = Visibility.Visible;
        //     //                             Stopwatch stopwatch = Stopwatch.StartNew();
        //     //                             await taskFactory.RunAsync(async () =>
        //     //                             {
        //     //                                 await profile.LoadDataAsync();
        //     //                                 await taskFactory.SwitchToMainThreadAsync();
        //     //                                 Main.NavigationService.Navigate(profile);
        //     //                                 profile.AddInitialData();
        //     //                                 this.Width = profile.Width;
        //     //                                 this.Height = profile.Height + 30;
        //     //                                 SpinnerGrid.Visibility = Visibility.Collapsed;
        //     //                             });
        //     //                             stopwatch.Stop();
        //     //                             System.Console.WriteLine(
        //     //                                 "Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed
        //     //                             );
        //     //                             Trace.WriteLine(
        //     //                                 "Time elapsed ${stopwatch.Elapsed} " + stopwatch.Elapsed
        //     //                             );
        //     //                             break;
        //     //                         case "Loading":
        //     //                             Main.NavigationService.Navigate(
        //     //                                 new LoadingPage(Task.CompletedTask, changePageCommand, errorLogger)
        //     //                             );
        //     //                             Background = LoadingPage.GetBackground();
        //     //                             break;
        //     //                         case "InGame Load":
        //     //                             Main.Width = 300;
        //     //                             page = new InGamePage(loRAPI, changePageCommand, null, errorLogger);
        //     //                             page.Width = 300;
        //     //                             page.LoRDbContext = loRDbContext;
        //     //                             Background = GetBackground();
        //     //                             SpinnerGrid.Visibility = Visibility.Visible;

        //     //                             try
        //     //                             {
        //     //                                 GameClientHandle = FindWindowA(null, "Legends of Runeterra");

        //     // #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        //     //                                 LRect clientPosition = new LRect(0, 0, 0, 0);
        //     //                                 bool didGetWindowRect = false;
        //     //                                 if (GameClientHandle != null)
        //     //                                 {
        //     //                                     didGetWindowRect = GetWindowRect(
        //     //                                         GameClientHandle,
        //     //                                         out clientPosition
        //     //                                     );
        //     //                                 }
        //     //                                 Stopwatch stopwatchInGameScreen = Stopwatch.StartNew();
        //     // #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
        //     //                                 await taskFactory.RunAsync(async () =>
        //     //                                 {
        //     //                                     await page.LoadDataAsync();
        //     //                                     await page.StartAsync();
        //     //                                     await taskFactory.SwitchToMainThreadAsync();
        //     //                                     Main.NavigationService.Navigate(page);
        //     //                                     //Width = page.Width;
        //     //                                     page.SetHeight(
        //     //                                         (clientPosition.left != 0 && clientPosition.right != 0)
        //     //                                             ? clientPosition.bottom - clientPosition.top
        //     //                                             : page.Height + 30
        //     //                                     );
        //     //                                     SpinnerGrid.Visibility = Visibility.Collapsed;

        //     //                                     Height =
        //     //                                         (clientPosition.left != 0 && clientPosition.right != 0)
        //     //                                             ? clientPosition.bottom - clientPosition.top
        //     //                                             : page.Height + 30;
        //     //                                     if (didGetWindowRect)
        //     //                                     {
        //     //                                         if (windowLocation == WindowLocation.Right)
        //     //                                         {
        //     //                                             // Set window to the right of the game client

        //     //                                             bool isMoved = MoveWindow(
        //     //                                                 FindWindowA(null, "MainWindow"),
        //     //                                                 clientPosition.right,
        //     //                                                 clientPosition.top,
        //     //                                                 (int)Width,
        //     //                                                 clientPosition.bottom - clientPosition.top,
        //     //                                                 true
        //     //                                             );
        //     //                                             //Left = clientPosition.right;
        //     //                                             //Top = clientPosition.top;
        //     //                                             Console.WriteLine(isMoved);
        //     //                                         }
        //     //                                         else
        //     //                                         {
        //     //                                             // Set window to the left of the game client

        //     //                                             bool isMoved = MoveWindow(
        //     //                                                 FindWindowA(null, "LoRHelper"),
        //     //                                                 (int)(clientPosition.left - this.Width),
        //     //                                                 (int)clientPosition.top,
        //     //                                                 (int)Width,
        //     //                                                 (int)Height,
        //     //                                                 true
        //     //                                             );

        //     //                                             Console.WriteLine(isMoved);
        //     //                                         }
        //     //                                     }
        //     //                                 });
        //     //                                 stopwatchInGameScreen.Stop();
        //     //                                 System.Console.WriteLine(
        //     //                                     "Time elapsed ${stopwatchInGameScreen.Elapsed} "
        //     //                                         + stopwatchInGameScreen.Elapsed
        //     //                                 );
        //     //                                 Trace.WriteLine(
        //     //                                     "Time elapsed ${stopwatchInGameScreen.Elapsed} "
        //     //                                         + stopwatchInGameScreen.Elapsed
        //     //                                 );
        //     //                             }
        //     //                             catch (System.Exception error)
        //     //                             {
        //     //                                 Trace.WriteLine(error.Message);
        //     //                                 throw;
        //     //                             }
        //     //                             break;
        //     //                         case "InGame":
        //     //                             if (page != null)
        //     //                             {
        //     //                                 Main.NavigationService.Navigate(page);
        //     //                                 Background = GetBackground();
        //     //                             }
        //     //                             break;
        //     //                         default:
        //     //                             System.Console.WriteLine("Nothing to update");
        //     //                             break;
        //     //                     }
        //     //                 }
        //     //                 catch (InvalidOperationException error)
        //     //                 {
        //     //                     Trace.WriteLine(error.Message);
        //     //                     await CustomMessageBox.ShowAsync(ex.Message);
        //     //                     messageBox.ShowDialog();
        //     //                 }
        //     //                 catch (Exception ex)
        //     //                 {
        //     //                     await taskFactory.RunAsync(
        //     //                         async () => await errorLogger.LogMessage(ex.ToString(), MessageType.Error)
        //     //                     );
        //     //                     throw;
        //     //                 }
        //     // }
        //     // catch (System.Exception ex)
        //     // {
        //     //     ShowException(ex);
        //     //     // if (profile == null)
        //     //     // {
        //     //     //     profile = new ProfilePage(loRAPI, changePageCommand, errorLogger);
        //     //     // }
        //     //     await ChangePageAsync("Profile Load");
        //     //     // await taskFactory.RunAsync(async () =>
        //     //     // {
        //     //     //     await profile!.LoadDataAsync();
        //     //     //     profile.AddInitialData();
        //     //     //     await taskFactory.SwitchToMainThreadAsync();
        //     //     //     Main.NavigationService.Navigate(page);
        //     //     //     SpinnerGrid.Visibility = Visibility.Collapsed;
        //     //     // });
        //     // }
        // }

        public void SetWindowLocation(int location)
        {
            // switch (location)
            // {
            //     case 1:
            //         windowLocation = WindowLocation.Left;
            //         break;
            //     case 2:
            //         windowLocation = WindowLocation.Right;
            //         break;
            //     default:
            //         windowLocation = WindowLocation.Right;
            //         break;
            // }
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
            // changePageCommand?.Execute(value);
        }

        private void toolbarGrid_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }

        private void btnMiniApp_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // GetVisualChild(0)
            //     .SetValue(StyleProperty, Application.Current.Resources["WindowBorder"]);
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowException(Exception ex)
        {
            //await CustomMessageBox.ShowAsync(ex.Message);
            System.Console.WriteLine(ex.Message);
            Trace.WriteLine(ex.Message);
        }
    }
}
