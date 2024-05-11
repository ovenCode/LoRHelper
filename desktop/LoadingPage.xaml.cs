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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace desktop
{
    /// <summary>
    /// Interaction logic for LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : Page
    {
        Storyboard? storyboard;
        Action<string> onUpdate;

        public LoadingPage(Action<string> onUpdateRequired)
        {
            onUpdate = onUpdateRequired;
            InitializeComponent();            
        }

        private void StartAnimation()
        {
            storyboard = new Storyboard();

            storyboard.Duration = Duration.Forever;
            storyboard.RepeatBehavior = RepeatBehavior.Forever;
            /*storyboard.SpeedRatio = 4;*/
            DoubleAnimation rotateAnim = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(rotateAnim, loadingEllipse);
            Storyboard.SetTargetProperty(
                rotateAnim,
                new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)")
            );

            storyboard.Children.Add(rotateAnim);
            Resources.Add("Storyboard", storyboard);
            storyboard.Begin();
            storyboard.Completed += Storyboard_Completed; 
        }

        private void Storyboard_Completed(object? sender, EventArgs e)
        {
            onUpdate("Profile");
        }

        public void StopAnimation()
        {
            if (storyboard != null)
            {
                storyboard.Stop();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartAnimation();
        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {
            onUpdate("Profile");
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            onUpdate("Profile");
        }
    }
}
