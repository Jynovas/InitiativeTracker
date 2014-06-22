using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Initiative_Tracker
{
    public static class DependacnyObjectExtension
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            T properParent = parent as T;
            if (properParent != null)
            {
                return properParent;
            }
            else
            {
                return parent.FindParent<T>();
            }
        }
    }
}
