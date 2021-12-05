using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskSCV
{
    public class Car : Point
    {
        public DateTime date { get; set; }
        //public int instantSpeed { get; set; } //если вдруг мгновенная скорость понадобится
        public Car(string str)
        {
            date = DateTime.Parse(str.Substring(0, 19));
            str = str.Substring(20);
            //instantSpeed = Int32.Parse(str.Substring(0, str.IndexOf(";")));
            str = str.Substring(str.IndexOf(";") + 1);
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            lon = Double.Parse(str.Substring(0, str.IndexOf(";")), formatter);
            str = str.Substring(str.IndexOf(";") + 1);
            lat = Double.Parse(str, formatter);
        }
        public bool IsBetween(DateTime start, DateTime end) => date >= start && date <= end;
    }
}
