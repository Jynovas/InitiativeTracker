﻿using InitiativeTrackerLibrary;
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
    /// Interaction logic for TakeDamageWindow.xaml
    /// </summary>
    public partial class TakeDamageWindow : Window
    {
        DD4ECombatant Combatant { get; set; }

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

        public TakeDamageWindow(DD4ECombatant combatant)
        {
            InitializeComponent();

            Combatant = combatant;
            DamageTypeList.ItemsSource = DamageTypes;

            PromptLabel.Content = String.Format("How much damage is {0} taking?", Combatant);
        }

        private void DamageButton_Click(object sender, RoutedEventArgs e)
        {
            if (DamageAmountTextBox.Text.IsPositiveInteger())
            {
                DD4EDamageType damageFlag = DD4EDamageType.None;

                if (DamageTypeList.SelectedItems.Count < 1)
                {
                    damageFlag = DD4EDamageType.Unaspected;
                }
                else
                {
                    foreach (var flag in DamageTypeList.SelectedItems)
                        damageFlag |= (DD4EDamageType)flag;
                }

                var damage = Convert.ToInt32(DamageAmountTextBox.Text);

                Combatant.TakeDamage(damageFlag, damage);

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
