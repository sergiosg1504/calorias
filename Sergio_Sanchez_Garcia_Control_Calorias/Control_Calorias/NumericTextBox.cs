using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;

namespace Control_Calorias
{
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            this.PreviewTextInput += new TextCompositionEventHandler(NumericTextBox_PreviewTextInput);
        }
        public int IntValue
        {
            get { return Int32.Parse(this.Text); }
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;
            string caracter = e.Text;
            if (Char.IsDigit(caracter[0])) { }
            else if (caracter.Equals(decimalSeparator) || caracter.Equals(negativeSign)) { }
            else if (caracter == "\b") { }
            else
            {
                e.Handled = true;
            }
        }
    }
}
