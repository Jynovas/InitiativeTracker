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
    /// Interaction logic for AttackTargetWindow.xaml
    /// </summary>
    public partial class AttackTargetWindow : Window
    {
        DD4ECombatant Target { get; set; }
        DD4EDamageType DamageType { get; set; }
        IList<DD4EStatusEffect> StatusEffects { get; set; }

        public AttackTargetWindow(ref DD4ECombatant target, DD4EDamageType damageType, IList<DD4EStatusEffect> statusEffects)
        {
            InitializeComponent();

            Target = target;
            DamageType = damageType;
            StatusEffects = statusEffects;
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            if (DefenseComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a defense to attack.", "Warning");
                return;
            }

            if (String.IsNullOrWhiteSpace(ToHitTextBox.Text))
            {
                MessageBox.Show("Please select enter their to hit value.", "Warning");
                return;
            }

            if (String.IsNullOrWhiteSpace(DamageTextBox.Text))
            {
                MessageBox.Show("Please select enter the damage value.", "Warning");
                return;
            }



            var doesHit = (bool)CriticalHitCheckBox.IsChecked;

            if (!doesHit)
            {
                // Check if the attack hits normally
            }

            if (doesHit)
            {
                foreach (var status in StatusEffects)
                    Target.ApplyStatusEffect(status.Copy());

                Target.TakeDamage(DamageType, Convert.ToInt32(DamageTextBox.Text));
            }
        }
    }
}
