using InitiativeTrackerLibrary;
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
using System.Windows.Shapes;

namespace Initiative_Tracker.DD4E
{
    /// <summary>
    /// Interaction logic for HealCombatantWindow.xaml
    /// </summary>
    public partial class HealCombatantWindow : Window
    {
        public DD4ECombatant Combatant { get; set; }

        public HealCombatantWindow(DD4ECombatant combatant)
        {
            InitializeComponent();

            Combatant = combatant;

            PromptLabel.Content = String.Format("How much is {0} healing?\nCurrent: {1}/{2}", Combatant.CombatName, Combatant.CurrentHP, Combatant.MaxHP);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HealButton_Click(object sender, RoutedEventArgs e)
        {
            if (HealthTextBox.Text.IsPositiveInteger())
            {
                var health = Convert.ToInt32(HealthTextBox.Text);
                Combatant.Heal(health);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please put in a positive integer for healing.", "Warning");
            }
        }
    }
}
