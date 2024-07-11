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
using System.Windows.Shapes;

namespace desktop
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            InitializeComponent();
            messageTB.Text = message;
        }

        public static bool? Show(String message)
        {
            CustomMessageBox messageBox = new CustomMessageBox(message);          
            return messageBox.ShowDialog();
        }

        private void MessageBtn_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }
    }
}
