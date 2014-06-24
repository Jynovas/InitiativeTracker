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
            if (String.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please input a name for the character", "Warning");
                return;
            }

            if (!MaxHPTextBox.Text.IsPositiveInteger())
            {
                MaxHPTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for health.", "Warning");
                return;
            }

            if (!ArmorClassTextBox.Text.IsPositiveInteger())
            {
                ArmorClassTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for armor class.", "Warning");
                return;
            }

            if (!FortitudeTextBox.Text.IsPositiveInteger())
            {
                FortitudeTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for fortitude.", "Warning");
                return;
            }

            if (!ReflexTextBox.Text.IsPositiveInteger())
            {
                ReflexTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for relfex.", "Warning");
                return;
            }

            if (!WillTextBox.Text.IsPositiveInteger())
            {
                WillTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for will.", "Warning");
                return;
            }

            if (!InitiativeBonusTextBox.Text.IsPositiveInteger())
            {
                InitiativeBonusTextBox.Text = "";
                MessageBox.Show("Please put in a positive integer for intiative bonus.", "Warning");
                return;
            }

            var combatant = new DD4ECombatant(NameTextBox.Text, 
                Convert.ToInt32(MaxHPTextBox.Text),
                Convert.ToInt32(ArmorClassTextBox.Text),
                Convert.ToInt32(FortitudeTextBox.Text),
                Convert.ToInt32(ReflexTextBox.Text),
                Convert.ToInt32(WillTextBox.Text),
                Convert.ToInt32(InitiativeBonusTextBox.Text));

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
