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
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
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
            this.Close();
        }
        #endregion
        #endregion
        #endregion

        #endregion
    }
}
