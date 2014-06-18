using Initiative_Tracker.DD4E;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Initiative_Tracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        DD4eCombatTracker dd4eCombatTracker;
        #endregion

        #region Properties
        DD4eCombatTracker DD4eCombatTracker
        {
            set { dd4eCombatTracker = value; }
            get
            {
                if (dd4eCombatTracker == null)
                    dd4eCombatTracker = new DD4eCombatTracker();

                return dd4eCombatTracker;
            }
        }
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            this.Content = DD4eCombatTracker;
        }
        #endregion

        #region Methods

        #region Event Handler Methods
        #endregion

        #endregion
    }
}
