using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Pimf : MembershipFunction
    {
        private double a, b, c, d;

        private Pimf()
        {
        }

        /// <summary>
        /// Π-shaped membership function with defaul name. Product functions smf and zmf. 
        /// </summary>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        /// <param name="d">Parameter 'd'</param>
        public Pimf(double a, double b, double c, double d)
            : this("pimf", a, b, c, d)
        {
        }

        /// <summary>
        /// Π-shaped membership function. Product functions smf and zmf. 
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        /// <param name="d">Parameter 'd'</param>
        public Pimf(string name, double a, double b, double c, double d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            if (x <= a)
                return 0;
            if (a < x && x <= (a + b) / 2)
                return 2 * Math.Pow((x - a) / (b - a), 2);
            if ((a + b) / 2 < x && x <= b)
                return 1 - 2 * Math.Pow((x - b) / (b - a), 2);
            if (b < x && x <= c)
                return 1;
            if (c < x && x <= (c + d) / 2)
                return 1 - 2 * Math.Pow((x - c) / (d - c), 2);
            if ((c + d) / 2 < x && x <= d)
                return 2 * Math.Pow((x - d) / (d - c), 2);
            return 0;
        }

        public override MembershipFunction Clone()
        {
            Pimf mf = new Pimf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.b = b;
            mf.c = c;
            mf.d = d;

            return mf;
        }
    }
}
