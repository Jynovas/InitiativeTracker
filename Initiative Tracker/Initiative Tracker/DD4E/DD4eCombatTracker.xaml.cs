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
using System.ComponentModel;

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

            // This is for testing
            for (int i = 0; i < 5; i++)
            {
                DD4ECombatant combatant = new DD4ECombatant("Goblin " + Combatants.Count, 25, 15, 15, 16, 15, 3);
                var fireRes = new DD4EDamageModifier(DD4EStatusEffectType.Resistance, combatant.Name, DD4EDamageType.Fire, 5, DD4EStatusEffectDuration.EndOfEncounter);
                combatant.ApplyStatusEffect(fireRes);
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
        private void StartCombat_Click(object sender, RoutedEventArgs e)
        {
            foreach (Combatant c in Combatants)
                if (!c.IsPlayer)
                    c.RollInitiative();

            SortByInitiative();
        }
        private void AddCombatant_Click(object sender, RoutedEventArgs e)
        {
            DD4ECombatant combatant = new DD4ECombatant("Goblin " + Combatants.Count, 25, 15, 15, 16, 15, 3);
            combatant.RollInitiative();
            Combatants.Add(combatant);
        }
        #endregion
        #region Combatant Context Menu Items
        private void SortByInitiative_Click(object sender, RoutedEventArgs e)
        {
            SortByInitiative();
        }
        private void TakeDamage_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            //(CombatantList.SelectedItem as DD4ECombatant).TakeDamage(DD4EDamageType.Unaspected, 5);

            var damageWindow = new TakeDamageWindow();
            damageWindow.Show();
        }
        private void TakeFireDamage_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            (CombatantList.SelectedItem as DD4ECombatant).TakeDamage(DD4EDamageType.Fire, 5);
        }
        private void TakeFireRadiantDamage_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            (CombatantList.SelectedItem as DD4ECombatant).TakeDamage(DD4EDamageType.Fire | DD4EDamageType.Radiant, 5);
        }
        private void Heal_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            (CombatantList.SelectedItem as DD4ECombatant).Heal(5);
        }
        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (Combatants.Count < 2)
                return;

            if (CombatantList.SelectedIndex < 1)
                return;

            Combatants = (ObservableCollection<DD4ECombatant>)Combatants.MoveUp(CombatantList.SelectedIndex);
        }
        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (Combatants.Count < 2)
                return;

            if (CombatantList.SelectedIndex >= Combatants.Count - 1)
                return;

            Combatants = (ObservableCollection<DD4ECombatant>)Combatants.MoveDown(CombatantList.SelectedIndex);
        }
        #endregion
        #endregion
        #endregion
        #region Helper Methods
        private void SortByInitiative()
        {
            for (int i = 0; i < Combatants.Count - 1; i++)
            {
                var highestIndex = i;
                for (int j = i + 1; j < Combatants.Count; j++)
                {
                    if (Combatants[j].Initiative > Combatants[highestIndex].Initiative)
                    {
                        highestIndex = j;
                    }
                    else if (Combatants[j].Initiative == Combatants[highestIndex].Initiative)
                    {
                        if (Combatants[j].InitiativeBonus > Combatants[highestIndex].InitiativeBonus)
                        {
                            highestIndex = j;
                        }
                    }
                }

                if (i != highestIndex)
                    Combatants = (ObservableCollection<DD4ECombatant>)Combatants.Swap(i, highestIndex);
            }
        }
        #endregion
        #endregion
    }
}
