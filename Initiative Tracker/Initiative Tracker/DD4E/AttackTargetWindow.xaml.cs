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
    /// Interaction logic for AttackTargetWindow.xaml
    /// </summary>
    public partial class AttackTargetWindow : Window
    {
        DD4ECombatant Target { get; set; }
        DD4EDamageType DamageType { get; set; }
        IList<DD4EStatusEffect> StatusEffects { get; set; }
        Regex integerRegex = new Regex(@"^-?[0-9]+$");

        public AttackTargetWindow(ref DD4ECombatant target, DD4EDamageType damageType, IList<DD4EStatusEffect> statusEffects)
        {
            InitializeComponent();

            Target = target;
            DamageType = damageType;
            StatusEffects = statusEffects;

            DefenderNameLabel.Content = Target.Name;
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            if (DefenseComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a defense to attack.", "Warning");
                return;
            }

            if (!((bool)CriticalHitCheckBox.IsChecked))
            {
                if (String.IsNullOrWhiteSpace(ToHitTextBox.Text))
                {
                    MessageBox.Show("Please select enter their to hit value.", "Warning");
                    return;
                }
                else if (!integerRegex.IsMatch(ToHitTextBox.Text))
                {
                    MessageBox.Show("Please select enter their to hit value as an integer using [0-9].", "Warning");
                    return;
                }
            }
            

            if (String.IsNullOrWhiteSpace(DamageTextBox.Text))
            {
                MessageBox.Show("Please select enter the damage value.", "Warning");
                return;
            }
            else if (!integerRegex.IsMatch(DamageTextBox.Text))
            {
                MessageBox.Show("Please select enter the damage as an integer using [0-9].", "Warning");
                return;
            }

            var defenseAdjustment = 0;
            if (!String.IsNullOrWhiteSpace(DefenseAdjustmentTextBox.Text))
            {
                if (!integerRegex.IsMatch(DefenseAdjustmentTextBox.Text))
                {
                    MessageBox.Show("Please select enter the defense adjustment as an integer using [0-9].", "Warning");
                    return;
                }
                else
                {
                    defenseAdjustment = Convert.ToInt32(DefenseAdjustmentTextBox.Text);
                }
            }

            var doesHit = (bool)CriticalHitCheckBox.IsChecked;

            if (!doesHit)
            {
                // Check if the attack hits normally
                var toHit = Convert.ToInt32(ToHitTextBox.Text);
                switch (DefenseComboBox.SelectedIndex)
                {
                    case 1:
                        doesHit = toHit >= (Target.Fortitude + defenseAdjustment);
                        break;
                    case 2:
                        doesHit = toHit >= (Target.Reflex + defenseAdjustment);
                        break;
                    case 3:
                        doesHit = toHit >= (Target.Will + defenseAdjustment);
                        break;
                    default:
                        doesHit = toHit >= (Target.ArmorClass + defenseAdjustment);
                        break;
                }
            }

            if (doesHit)
            {
                foreach (var status in StatusEffects)
                    Target.ApplyStatusEffect(status.Copy());

                Target.TakeDamage(DamageType, Convert.ToInt32(DamageTextBox.Text));
            }

            this.Close();
        }
    }
}
