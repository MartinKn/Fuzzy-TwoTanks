using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Gbellmf : MembershipFunction
    {
        private double a, b, c;

        private Gbellmf()
        {
        }

        /// <summary>
        /// Generalized bell-shaped membership with defaul name.
        /// </summary>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Midpoint</param>
        public Gbellmf(double a, double b, double c)
            : this("gbellmf", a, b, c)
        {
        }

        /// <summary>
        /// Generalized bell-shaped membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="b">Parameter 'b'</param>
        /// <param name="c">Midpoint</param>
        public Gbellmf(string name, double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            return 1 / (1 + Math.Pow(Math.Abs((x - c) / a), 2 * b));
        }

        public override MembershipFunction Clone()
        {
            Gbellmf mf = new Gbellmf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.b = b;
            mf.c = c;

            return mf;
        }
    }
}
