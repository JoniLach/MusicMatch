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
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null)
            {
                if (int.TryParse(rb.Tag.ToString(), out int rating))
                {
                    SelectedRating = rating;
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
