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
using ViewModel;

namespace MusicMatch
{
    public partial class InstrumentSelectionPage : Page
    {
        private User currentUser;
        private List<Instrument> selectedInstruments = new List<Instrument>();

        public InstrumentSelectionPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadInstruments();
        }

        private void LoadInstruments()
        {
            try
            {
                InstrumentDB db = new InstrumentDB();
                InstrumentList instruments = db.SelectAll();
                icInstruments.ItemsSource = instruments;
                
                // If the list is empty, add some mock data if DB is empty so UI isn't blank (optional fallback)
                if (instruments.Count == 0)
                {
                    // For now, let's assume DB has data. If not, the user sees nothing.
                    // We could hardcode insert if we wanted to seed the DB.
                    // db.InsertHardcodedInstruments(); // hypothetical method
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load instruments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_AttachedToVisualTree(object sender, EventArgs e)
        {
            // Optional: can use this to state management if needed
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var instrument = checkBox.DataContext as Instrument;
            if (instrument != null && !selectedInstruments.Contains(instrument))
            {
                selectedInstruments.Add(instrument);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var instrument = checkBox.DataContext as Instrument;
            if (instrument != null && selectedInstruments.Contains(instrument))
            {
                selectedInstruments.Remove(instrument);
            }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
            {
                 MessageBox.Show("User context lost. Please login again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                 NavigationService?.Navigate(new Login());
                 return;
            }

            try
            {
                InstrumentDB db = new InstrumentDB();
                foreach (var inst in selectedInstruments)
                {
                    db.AddUserInstrument(currentUser.Id, inst.Id);
                }

                // Navigate to next page (SearchPage or StudentHomePage depending on type)
                MainWindow.LoggedInUser = currentUser; // Ensure global static is set
                
                if (currentUser is Student)
                    NavigationService?.Navigate(new StudentHomePage());
                else
                    NavigationService?.Navigate(new SearchPage());
            }
            catch (Exception ex)
            {
                 MessageBox.Show($"Error saving selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
