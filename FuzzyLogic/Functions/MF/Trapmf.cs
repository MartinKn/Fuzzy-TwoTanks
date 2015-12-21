using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Trapmf : MembershipFunction
    {
        private double a, b, c, d;

        private Trapmf()
        { 
        }

        /// <summary>
        /// Trapezoidal-shaped membership function with defaul name.
        /// </summary>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        /// <param name="d">Parameter 'd'</param>
        public Trapmf(double a, double b, double c, double d)
            :this("trapmf", a, b, c, d)
        {
        }

        /// <summary>
        /// Trapezoidal-shaped membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        /// <param name="d">Parameter 'd'</param>
        public Trapmf(string name, double a, double b, double c, double d)
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
            if (a < x && x <= b)
                return (x - a) / (b - a);
            if (b < x && x <= c)
                return 1;
            if (c < x && x <= d)
                return (d - x) / (d - c);
            return 0;
        }

        public override MembershipFunction Clone()
        {
            Trapmf mf = new Trapmf();
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
