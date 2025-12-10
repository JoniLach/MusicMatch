using Model;
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

namespace MusicMatch
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            StartGradientAnimation();



            tbWelcomeMessage.Text = "s";

        }




        private void StartGradientAnimation()
        {
            // Animate the two GradientStop colors to shift over time
            var gs0 = (LinearGradientBrush)FindResource("AnimatedBackground");
            if (gs0 == null) return;


            var g0 = gs0.GradientStops[0];
            var g1 = gs0.GradientStops[1];


            ColorAnimation cAnim1 = new ColorAnimation()
            {
                From = (Color)ColorConverter.ConvertFromString("#FF0B486B"),
                To = (Color)ColorConverter.ConvertFromString("#FF2E8B57"),
                Duration = TimeSpan.FromSeconds(6),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };


            ColorAnimation cAnim2 = new ColorAnimation()
            {
                From = (Color)ColorConverter.ConvertFromString("#FF3B8D99"),
                To = (Color)ColorConverter.ConvertFromString("#FFFF6F61"),
                Duration = TimeSpan.FromSeconds(8),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };


            g0.BeginAnimation(GradientStop.ColorProperty, cAnim1);
            g1.BeginAnimation(GradientStop.ColorProperty, cAnim2);
        }
    }
}

