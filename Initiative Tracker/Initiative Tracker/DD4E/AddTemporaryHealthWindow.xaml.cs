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
using InitiativeTrackerLibrary;

namespace Initiative_Tracker.DD4E
{
    /// <summary>
    /// Interaction logic for AddTemporaryHealthWindow.xaml
    /// </summary>
    public partial class AddTemporaryHealthWindow : Window
    {
        DD4ECombatant Combatant { get; set; }

        public AddTemporaryHealthWindow(DD4ECombatant combatant)
        {
            InitializeComponent();

            Combatant = combatant;

            PromptLabel.Content = String.Format("How much temp HP will {0} add?\nCurrent: {1}", Combatant.CombatName, Combatant.TemporaryHP);
        }

        private void AddTempButton_Click(object sender, RoutedEventArgs e)
        {
            if (HealthTextBox.Text.IsPositiveInteger())
            {
                var tempHP = Convert.ToInt32(HealthTextBox.Text);
                Combatant.TemporaryHP += tempHP;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please input a positive integer", "Warning");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
