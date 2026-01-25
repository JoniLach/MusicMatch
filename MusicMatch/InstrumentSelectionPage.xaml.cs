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
    public class SelectableInstrument
    {
        public Instrument Instrument { get; set; }
        public bool IsSelected { get; set; }
    }

    public partial class InstrumentSelectionPage : Page
    {
        private User currentUser;
        private bool isEditMode;
        private List<SelectableInstrument> instrumentsList = new List<SelectableInstrument>();

        public InstrumentSelectionPage(User user, bool isEditMode = false)
        {
            InitializeComponent();
            currentUser = user;
            this.isEditMode = isEditMode;
            
            if (isEditMode)
            {
                btnContinue.Content = "Save";
            }
            
            LoadInstruments();
        }

        private void LoadInstruments()
        {
            try
            {
                InstrumentDB db = new InstrumentDB();
                InstrumentList allInstruments = db.SelectAll();

                // Get user's existing instruments to pre-select
                InstrumentList userInstruments = db.GetUserInstruments(currentUser.Id);

                instrumentsList.Clear();
                foreach (Instrument inst in allInstruments)
                {
                    instrumentsList.Add(new SelectableInstrument
                    {
                        Instrument = inst,
                        IsSelected = userInstruments.Any(ui => ui.Id == inst.Id)
                    });
                }

                icInstruments.ItemsSource = instrumentsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load instruments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                
                // Get current validated selections
                var selectedInstruments = instrumentsList.Where(i => i.IsSelected).Select(i => i.Instrument).ToList();

                if (isEditMode)
                {
                    // In edit mode, we need to sync additions and removals
                    InstrumentList currentDbInstruments = db.GetUserInstruments(currentUser.Id);

                    // Add new ones
                    foreach (var inst in selectedInstruments)
                    {
                        if (!currentDbInstruments.Any(ci => ci.Id == inst.Id))
                        {
                            db.AddUserInstrument(currentUser.Id, inst.Id);
                        }
                    }

                    // Remove deselected ones
                    foreach (var inst in currentDbInstruments)
                    {
                        if (!selectedInstruments.Any(si => si.Id == inst.Id))
                        {
                            db.RemoveUserInstrument(currentUser.Id, inst.Id);
                        }
                    }
                    
                    // Go back
                    if (NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                    else
                    {
                        // Fallback if no history
                        if (currentUser is Student)
                            NavigationService?.Navigate(new StudentHomePage());
                        else
                            NavigationService?.Navigate(new TeacherHomePage());
                    }
                }
                else
                {
                    // Onboarding mode: Just add everything selected (assuming fresh start or cumulative add)
                    // Just to be safe, we can use the same logic as sync, or just add.
                    // The original code just added. Let's stick to safe adding.
                     InstrumentList currentDbInstruments = db.GetUserInstruments(currentUser.Id);
                     
                     foreach (var inst in selectedInstruments)
                     {
                        if (!currentDbInstruments.Any(ci => ci.Id == inst.Id))
                        {
                            db.AddUserInstrument(currentUser.Id, inst.Id);
                        }
                     }
                     
                    // Navigate to next page
                    MainWindow.LoggedInUser = currentUser;
                    
                    if (currentUser is Student)
                        NavigationService?.Navigate(new StudentHomePage());
                    else
                        NavigationService?.Navigate(new TeacherHomePage());
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show($"Error saving selection: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
