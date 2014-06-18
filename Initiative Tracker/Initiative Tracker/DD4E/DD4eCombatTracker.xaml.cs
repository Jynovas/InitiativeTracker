using InitiativeTrackerLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Initiative_Tracker.DD4E
{
    /// <summary>
    /// Interaction logic for DD4eCombatTracker.xaml
    /// </summary>
    public partial class DD4eCombatTracker : UserControl
    {
        #region Variables
        ObservableCollection<DD4ECombatant> combatants;
        #endregion

        #region Properties
        public ObservableCollection<DD4ECombatant> Combatants
        {
            set { combatants = value; }
            get
            {
                if (combatants == null)
                    combatants = new ObservableCollection<DD4ECombatant>();

                return combatants;
            }
        }
        #endregion

        #region Construcors
        public DD4eCombatTracker()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                DD4ECombatant combatant = new DD4ECombatant("Goblin " + Combatants.Count, 25, 15, 15, 16, 15, 3);
                Combatants.Add(combatant);
            }

            this.CombatantList.ItemsSource = Combatants;
        }
        #endregion

        #region Methods
        #region Event Handler Methods
        private void TestMenuItem_Click(object sender, RoutedEventArgs args)
        {
            if (sender is MenuItem)
                MessageBox.Show("You clicked MenuItem " + (sender as MenuItem).Header + "...");
            else
                MessageBox.Show("You attached this event to something that is not a MenuItem...");
        }
        #region Menu Items
        #region File Menu Items
        private void Exit_Click(object sender, RoutedEventArgs args)
        {
            //this.Close();
        }
        #endregion
        #region Combat Menu Items
        private void AddCombatant_Click(object sender, RoutedEventArgs e)
        {
            DD4ECombatant combatant = new DD4ECombatant("Goblin " + Combatants.Count, 25, 15, 15, 16, 15, 3);
            Combatants.Add(combatant);
            //CombatantList.InvalidateVisual();
            //MessageBox.Show("Added " + (Combatants[Combatants.Count - 1] as DD4ECombatant).Name + "\nTotal: " + Combatants.Count);
        }
        #endregion
        #region Combatant Context Menu Items
        private void TakeDamage_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            (CombatantList.SelectedItem as DD4ECombatant).TakeDamage(DD4EDamageType.Unaspected, 5);
        }
        #endregion
        #endregion
        #endregion

        #endregion
    }
}
