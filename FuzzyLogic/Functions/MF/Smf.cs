using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Smf : MembershipFunction
    {
        private double a, b;

        private Smf()
        {
        }

        /// <summary>
        /// S-shaped membership function with defaul name.
        /// </summary>
        /// <param name="a">Right extreme of the sloped portion of the curve</param>
        /// <param name="b">Left extreme of the sloped portion of the curve</param>
        public Smf(double a, double b)
            : this("smf", a, b)
        { 
        }
        
        /// <summary>
        /// S-shaped membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Right extreme of the sloped portion of the curve</param>
        /// <param name="b">Left extreme of the sloped portion of the curve</param>
        public Smf(string name, double a, double b)
        {
            this.a = a;
            this.b = b;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            if (x <= a)
                return 0;
            if (a < x && x <= (a + b) / 2)
                return 2 * Math.Pow((x - a) / (b - a), 2);
            if ((a + b) / 2 < x && x <= b)
                return 1 - 2 * Math.Pow((b - x) / (b - a), 2);
            return 1;
        }

        public override MembershipFunction Clone()
        {
            Smf mf = new Smf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.b = b;

            return mf;
        }
    }
}
