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
    /// Interaction logic for TakeDamageWindow.xaml
    /// </summary>
    public partial class TakeDamageWindow : Window
    {
        protected IList<DD4ECombatant> Combatants { get; set; }

        List<DD4EDamageType> damageTypes;
        protected List<DD4EDamageType> DamageTypes
        {
            set { damageTypes = value; }
            get
            {
                if (damageTypes == null)
                {
                    damageTypes = new List<DD4EDamageType>();

                    foreach (var value in Enum.GetValues(typeof(DD4EDamageType)).Cast<DD4EDamageType>())
                    {
                        if (value != DD4EDamageType.None && value != DD4EDamageType.Unaspected && value != DD4EDamageType.All)
                            damageTypes.Add(value);
                    }
                }

                return damageTypes;
            }
        }

        public TakeDamageWindow(IList<DD4ECombatant> combatants)
        {
            InitializeComponent();
            this.Combatants = combatants;

            AttackerBox.ItemsSource = Combatants;
            DefenderList.ItemsSource = Combatants;
            DamageTypeList.ItemsSource = DamageTypes;
        }

        public TakeDamageWindow(IList<DD4ECombatant> combatants, int selectedIndex)
        {
            InitializeComponent();
            this.Combatants = combatants;

            AttackerBox.ItemsSource = Combatants;
            AttackerBox.SelectedIndex = selectedIndex;
            DefenderList.ItemsSource = Combatants;
            DamageTypeList.ItemsSource = DamageTypes;
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            DamageTypeList.SelectAll();
        }

        private void None_Click(object sender, RoutedEventArgs e)
        {
            DamageTypeList.UnselectAll();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox_IntegerLimit(object sender, TextCompositionEventArgs e)
        {
            e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]*$");
        }

        private void ToHitText_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = System.Text.RegularExpressions.Regex.IsMatch(ToHitText.Text, "^[0-9]*$");
        }
    }
}
