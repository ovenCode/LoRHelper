using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using LoRAPI.Controllers;

namespace desktop
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page, ILoRHelperWindow
    {
        ILoRApiHandler loRAPI;
        ICommand requireUpdate;

        public WelcomePage(
            ILoRApiHandler apiController,
            ICommand onUpdateRequired,
            ErrorLogger errorLogger
        )
        {
            loRAPI = apiController;
            requireUpdate = onUpdateRequired;
            InitializeComponent();
        }

        public static Brush GetBackground()
        {
            return new SolidColorBrush(Color.FromRgb(94, 94, 94));
        }

        private async void BtnPort_Click(object sender, RoutedEventArgs e)
        {
            // TODO: improve handling
            try
            {
                if (loRAPI == null)
                {
                    throw new ApiConnectionException(
                        "Nie udało się utworzyć połączenia. Skontaktuj się z administratorem"
                    );
                }
                int port = 0;
                if (loRAPI.GetType() == typeof(LoRApiController))
                {
                    ((LoRApiController)loRAPI)?.SetPort(
                        int.TryParse(portNumber.Text, out port) ? port : 0
                    );
                }
                System.Console.WriteLine("Preparing to load the user profile");
                Trace.WriteLine("Preparing to load the user profile");
                requireUpdate.Execute("Profile Load");
            }
            catch (InvalidOperationException error)
            {
                CustomMessageBox messageBox = new CustomMessageBox(error.Message);
                messageBox.ShowDialog();
            }
            catch (System.Exception ex)
            {
                CustomMessageBox messageBox = new CustomMessageBox(ex.Message);
                messageBox.ShowDialog();
                throw;
            }
        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void portNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnPort_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
