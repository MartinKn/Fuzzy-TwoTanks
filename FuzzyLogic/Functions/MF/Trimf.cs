using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Trimf : MembershipFunction
    {
        private double a, b, c;

        private Trimf()
        {
        }

        /// <summary>
        /// Triangular-shaped membership function with defaul name.
        /// </summary>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        public Trimf(double a, double b, double c)
            :this("trimf", a, b, c)
        {
        }

        /// <summary>
        /// Triangular-shaped membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Parameter 'c'</param>
        public Trimf(string name, double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            if (x <= a)
                return 0;
            if (a < x && x <= b)
                return (x - a) / (b - a);
            if (b < x && x <= c)
                return (c - x) / (c - b);
            return 0;
        }

        public override MembershipFunction Clone()
        {
            Trimf mf = new Trimf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.b = b;
            mf.c = c;

            return mf;
        }
    }
}
