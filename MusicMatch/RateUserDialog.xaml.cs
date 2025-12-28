using System;
using System.Windows;
using System.Windows.Controls;

namespace MusicMatch
{
    public partial class RateUserDialog : Window
    {
        public int SelectedRating { get; private set; } = 5;

        public RateUserDialog(string userName)
        {
            InitializeComponent();
            txtPrompt.Text = $"How was your experience with {userName}?";
            UpdateStars();
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                if (int.TryParse(btn.Tag.ToString(), out int rating))
                {
                    SelectedRating = rating;
                    UpdateStars();
                }
            }
        }

        private void UpdateStars()
        {
            if (StarsPanel == null) return;

            foreach (var child in StarsPanel.Children)
            {
                if (child is Button btn && int.TryParse(btn.Tag.ToString(), out int starValue))
                {
                    if (starValue <= SelectedRating)
                    {
                        btn.Content = "⭐"; // Filled star
                        btn.Opacity = 1.0;
                    }
                    else
                    {
                        btn.Content = "☆"; // Empty star
                        btn.Opacity = 0.5;
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
