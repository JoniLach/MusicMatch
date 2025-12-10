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
using Model;

namespace MusicMatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static User loggedInUser;

        public static User LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value; }
        }
        public MainWindow() : this(null)
        {
            MainFrame.Navigate(new Login());
        }
        public MainWindow(User user)
        {

            InitializeComponent();
            LoggedInUser = user;


            MainFrame.Navigate(new SearchPage());
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser != null)
                MainFrame.Navigate(new SearchPage());
            else
                MessageBox.Show("Must log in first or create and account.");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser != null)
                MainFrame.Navigate(new UserPage());
            else
                MessageBox.Show("Must log in first or create and account.");
        }

        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser != null)
                MainFrame.Navigate(new UserPage());
            else
                MessageBox.Show("Must log in first or create and account.");

        }

        private void MiniGame_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedInUser != null)
                MainFrame.Navigate(new UserPage());
            else
                MessageBox.Show("Must log in first or create and account.");
        }
    }
}

