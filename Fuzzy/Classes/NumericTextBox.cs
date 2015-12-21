using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Fuzzy.Classes
{
    public class NumericTextBox : TextBox
    {
        bool allowSpace = false;

        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;

            string keyInput = e.KeyChar.ToString();

            if (!Char.IsDigit(e.KeyChar) &&
                !keyInput.Equals(decimalSeparator) &&
                !keyInput.Equals(groupSeparator) &&
                e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            else
                if (keyInput.Equals(decimalSeparator) && this.Text.IndexOf(decimalSeparator[0]) > -1)
                    e.Handled = true;
        }

        public int IntValue
        {
            get { return Int32.Parse(this.Text); }
        }

        public decimal DecimalValue
        {
            get { return Decimal.Parse(this.Text); }
        }

        public bool AllowSpace
        {
            set { this.allowSpace = value; }
            get { return this.allowSpace; }
        }
    }
}
