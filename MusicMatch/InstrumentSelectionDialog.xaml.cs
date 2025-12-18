using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model;
using ViewModel;

namespace MusicMatch
{
    public partial class InstrumentSelectionDialog : Window
    {
        private User currentUser;
        private List<Instrument> selectedInstruments = new List<Instrument>();

        public InstrumentSelectionDialog(User user)
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
                InstrumentList allInstruments = db.SelectAll();
                InstrumentList userInstruments = db.GetUserInstruments(currentUser.Id);

                icInstruments.ItemsSource = allInstruments;

                // Pre-check instruments the user already has
                foreach (var item in icInstruments.Items)
                {
                    var instrument = item as Instrument;
                    if (instrument != null && userInstruments.Any(ui => ui.Id == instrument.Id))
                    {
                        selectedInstruments.Add(instrument);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading instruments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var instrument = checkBox?.DataContext as Instrument;
            if (instrument != null && !selectedInstruments.Contains(instrument))
            {
                selectedInstruments.Add(instrument);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            var instrument = checkBox?.DataContext as Instrument;
            if (instrument != null && selectedInstruments.Contains(instrument))
            {
                selectedInstruments.Remove(instrument);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstrumentDB db = new InstrumentDB();
                
                // Get current user instruments
                InstrumentList currentInstruments = db.GetUserInstruments(currentUser.Id);
                
                // Add new selections
                foreach (var inst in selectedInstruments)
                {
                    if (!currentInstruments.Any(ci => ci.Id == inst.Id))
                    {
                        db.AddUserInstrument(currentUser.Id, inst.Id);
                    }
                }
                
                // Remove deselected instruments
                foreach (var inst in currentInstruments)
                {
                    if (!selectedInstruments.Any(si => si.Id == inst.Id))
                    {
                        db.RemoveUserInstrument(currentUser.Id, inst.Id);
                    }
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving instruments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
