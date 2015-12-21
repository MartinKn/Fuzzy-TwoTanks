using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Zmf : MembershipFunction
    {
        private double a, b;

        private Zmf()
        {
        }

        /// <summary>
        /// Z-shaped membership function with defaul name.
        /// </summary>
        /// <param name="a">Right extreme of the sloped portion of the curve</param>
        /// <param name="b">Left extreme of the sloped portion of the curve</param>
        public Zmf(double a, double b)
            : this("zmf", a, b)
        { 
        }

        /// <summary>
        /// Z-shaped membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Right extreme of the sloped portion of the curve</param>
        /// <param name="b">Left extreme of the sloped portion of the curve</param>
        public Zmf(string name, double a, double b)
        {
            this.a = a;
            this.b = b;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            if (x <= a)
                return 1;
            if (a < x && x <= (a + b) / 2)
                return 1 - 2 * Math.Pow((x - a) / (b - a), 2);
            if ((a + b) / 2 < x && x <= b)
                return 2 * Math.Pow((x - b) / (b - a), 2);
            return 0;
        }

        public override MembershipFunction Clone()
        {
            Zmf mf = new Zmf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.b = b;

            return mf;
        }
    }
}
