using LoRAPI.Controllers;
using System;
using System.Collections.Generic;
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

namespace desktop
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        LoRApiController loRAPI;
        Action<string> requireUpdate;
        public WelcomePage(LoRApiController apiController, Action<string> onUpdateRequired)
        {
            loRAPI = apiController;
            requireUpdate = onUpdateRequired;
            InitializeComponent();
        }

        private void BtnPort_Click(object sender, RoutedEventArgs e)
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
                loRAPI?.SetPort(int.TryParse(portNumber.Text, out port) ? port : 0);
                requireUpdate("Profile");
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }    
}
