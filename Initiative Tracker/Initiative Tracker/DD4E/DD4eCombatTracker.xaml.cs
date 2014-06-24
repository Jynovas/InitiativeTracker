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
        DD4ECombatant activeCombatant;
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
        bool CombatActive { get; set; }
        public DD4ECombatant ActiveCombatant
        {
            get { return activeCombatant; }
            set
            {
                if (activeCombatant == null || value == null || !activeCombatant.Equals(value))
                {
                    activeCombatant = value;

                    var index = CombatantList.Items.IndexOf(activeCombatant);
                    if (index != -1)
                    {
                        for (int i = 0; i < CombatantList.Items.Count; i++)
                        {
                            var row = CombatantList.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                            if (i == index)
                            {
                                row.Background = Brushes.CornflowerBlue;
                            }
                            else
                            {
                                row.Background = Brushes.Transparent;
                            }

                        }
                    }
                }
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
                DD4ECombatant combatant = new DD4ECombatant("Goblin", 25, 15, 15, 16, 15, 3);
                var fireRes = new DD4EDamageModifier(DD4EStatusEffectType.Resistance, combatant.Name, DD4EDamageType.Fire, 5, DD4EStatusEffectDuration.EndOfEncounter);
                combatant.ApplyStatusEffect(fireRes);
                AddCombatant(combatant);
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
            if (Combatants.Count > 1)
            {
                for (int i = 0; i < Combatants.Count; i++)
                {
                    if (!Combatants[i].IsPlayer)
                    {
                        Combatants[i].RollInitiative();
                    }
                    else
                    {
                        SetCombatantInitiative(i);
                    }
                }

                SortByInitiative();

                ActiveCombatant = CombatantList.Items[0] as DD4ECombatant;

                CombatActive = true;
            }
        }
        private void EndCombat_Click(object sender, RoutedEventArgs e)
        {
            CombatActive = false;

            ActiveCombatant = null;
        }
        private void NextTurn_Click(object sender, RoutedEventArgs e)
        {
            if (CombatActive)
            {
                var index = CombatantList.Items.IndexOf(ActiveCombatant);
                if (index != -1)
                {
                    for (int i = 0; i < Combatants.Count; i++)
                    {
                        if (i == index)
                            Combatants[i].EndTurn();
                        else
                            Combatants[i].OtherEndTurn(Combatants[index].CombatName);
                    }

                    if (index >= CombatantList.Items.Count - 1)
                        ActiveCombatant = CombatantList.Items[0] as DD4ECombatant;
                    else
                        ActiveCombatant = CombatantList.Items[index + 1] as DD4ECombatant;

                    var startIndex = CombatantList.Items.IndexOf(activeCombatant);

                    for (int i = 0; i < Combatants.Count; i++)
                    {
                        if (i == startIndex)
                            Combatants[i].StartTurn();
                        else
                            Combatants[i].OtherStartTurn(Combatants[startIndex].CombatName);
                    }
                }
            }
        }
        private void AddCombatant_Click(object sender, RoutedEventArgs e)
        {
            var addCombatantWindow = new AddCombatantWindow(Combatants);
            addCombatantWindow.Owner = this.FindParent<Window>();
            addCombatantWindow.ShowDialog();
        }
        private void RemoveCombatant_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedItem != null)
            {
                if (ActiveCombatant != null)
                {
                    if (ActiveCombatant.Name.Equals((CombatantList.SelectedItem as DD4ECombatant).Name))
                    {
                        var index = CombatantList.Items.IndexOf(ActiveCombatant);
                        if (index >= CombatantList.Items.Count - 1)
                            ActiveCombatant = CombatantList.Items[0] as DD4ECombatant;
                        else
                            ActiveCombatant = CombatantList.Items[index + 1] as DD4ECombatant;
                    }
                }

                var removeIndex = CombatantList.SelectedIndex;

                CombatantList.SelectedIndex = -1;

                Combatants.RemoveAt(removeIndex);
            }
        }
        #endregion
        #region Combatant Context Menu Items
        private void SortByInitiative_Click(object sender, RoutedEventArgs e)
        {
            SortByInitiative();
        }
        private void SetInitiative_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            SetCombatantInitiative(CombatantList.SelectedIndex);
        }
        private void SetFigurine_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            var figurineWindow = new SetFigurineWindow(Combatants[CombatantList.SelectedIndex]);
            figurineWindow.Owner = this.FindParent<Window>();
            figurineWindow.ShowDialog();
        }
        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            var damageWindow = new SelectTargetsWindow(Combatants, CombatantList.SelectedIndex);
            damageWindow.Owner = this.FindParent<Window>(); 
            damageWindow.ShowDialog();
        }
        private void Heal_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            var healWindow = new HealCombatantWindow(Combatants[CombatantList.SelectedIndex]);
            healWindow.Owner = this.FindParent<Window>();
            healWindow.ShowDialog();
        }
        private void AddTempHP_Click(object sender, RoutedEventArgs e)
        {
            if (CombatantList.SelectedIndex == -1)
                return;

            var addTempWindow = new AddTemporaryHealthWindow(Combatants[CombatantList.SelectedIndex]);
            addTempWindow.Owner = this.FindParent<Window>();
            addTempWindow.ShowDialog();
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
        private void SetCombatantInitiative(int index)
        {
            var setInitiativeWindow = new SetInitiativeWindow(Combatants[index]);
            setInitiativeWindow.Owner = this.FindParent<Window>();
            setInitiativeWindow.ShowDialog();
        }
        private void AddCombatant(DD4ECombatant combatant)
        {
            if (Combatants.Any(c => c.Name.Equals(combatant.Name) && c.IsPlayer == false))
            {
                int id = Combatants.Count(c => c.Name.Equals(combatant.Name) && c.IsPlayer == false);
                combatant.ID = id;
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
        }
        #endregion
        #endregion
    }
}
