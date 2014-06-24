using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Initiative_Tracker
{
    public static class MathHelper
    {
        #region Variables
        static Regex positiveIntegerRegex = new Regex(@"^[0-9]+$");
        static Regex integerRegex = new Regex(@"^-?[0-9]+$");
        #endregion

        #region Methods
        public static bool IsInteger(this string value)
        {
            return integerRegex.IsMatch(value);
        }

        public static bool IsPositiveInteger(this string value)
        {
            return positiveIntegerRegex.IsMatch(value);
        }
        #endregion
    }
}
