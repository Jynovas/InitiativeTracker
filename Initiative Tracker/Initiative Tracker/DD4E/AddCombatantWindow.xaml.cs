using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitiativeTrackerLibrary;

namespace Initiative_Tracker.DD4E
{
    /// <summary>
    /// Interaction logic for AddCombatantToCombatWindow.xaml
    /// </summary>
    public partial class AddCombatantWindow : Window
    {
        Regex integerRegex = new Regex(@"^[0-9]+$");
        ObservableCollection<DD4ECombatant> Combatants { get; set; }

        public AddCombatantWindow(ObservableCollection<DD4ECombatant> combatants)
        {
            InitializeComponent();

            Combatants = combatants;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var combatant = new DD4ECombatant("Cloud Strife", 100, 23, 25, 20, 17, 5);
            combatant.IsPlayer = (bool)IsPlayerCheckBox.IsChecked;

            if (!combatant.IsPlayer)
            {
                if (Combatants.Any(c => c.Name.Equals(combatant.Name) && c.IsPlayer == false))
                {
                    int id = Combatants.Count(c => c.Name.Equals(combatant.Name) && c.IsPlayer == false);
                    combatant.ID = id;
                }
            }
            else
            {
                if (Combatants.Any(c => c.Name.Equals(combatant.Name)))
                {
                    MessageBox.Show("You cannot add the same player character twice.", "Warning");
                    return;
                }
            }

            Combatants.Add(combatant);
            this.Close();
            
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
