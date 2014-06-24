using System;
using System.Collections.Generic;
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
    /// Interaction logic for SetInitiativeWindow.xaml
    /// </summary>
    public partial class SetInitiativeWindow : Window
    {
        Regex integerRegex = new Regex(@"^[0-9]+$");
        public Combatant Combatant { get; set; }

        public SetInitiativeWindow(Combatant combatant)
        {
            InitializeComponent();

            Combatant = combatant;

            PromptLabel.Content = String.Format("What is the initiative for {0}?", Combatant.CombatName);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            if (integerRegex.IsMatch(InitiativeTextBox.Text))
            {
                var initiative = Convert.ToInt32(InitiativeTextBox.Text);
                Combatant.Initiative = initiative;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please put in a positive integer", "Warning");
            }
        }
    }
}
