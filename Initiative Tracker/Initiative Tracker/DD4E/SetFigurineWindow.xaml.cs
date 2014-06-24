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
    /// Interaction logic for SetFigurineWindow.xaml
    /// </summary>
    public partial class SetFigurineWindow : Window
    {
        DD4ECombatant Combatant { get; set; }

        public SetFigurineWindow(DD4ECombatant combatant)
        {
            InitializeComponent();

            Combatant = combatant;

            PromptLabel.Content = String.Format("Which figure is {0} using?", Combatant.CombatName);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkayButton_Click(object sender, RoutedEventArgs e)
        {
            Combatant.Figurine = FigurineTextBox.Text;
            this.Close();
        }
    }
}
