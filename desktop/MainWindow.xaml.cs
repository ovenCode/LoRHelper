using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using LoRAPI.Models;

namespace desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private event EventHandler<string> update, loaded;
        LoRApiController? loRAPI;
        HttpClient httpClient;

        public MainWindow()
        {
            httpClient = new HttpClient();
            loRAPI = new LoRApiController(httpClient);
            InitializeComponent();
            Main.Content = new WelcomePage(loRAPI, OnUpdateRequired);
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
                    Main.Content = new ProfilePage(loRAPI);
                    break;
                case "Loading":
                    Main.Content = new LoadingPage(OnUpdateRequired);
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
    }
}
