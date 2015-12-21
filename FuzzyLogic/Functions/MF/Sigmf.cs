using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Sigmf : MembershipFunction
    {
        private double a, c;

        private Sigmf()
        {
        }

        /// <summary>
        /// Sigmoidal membership function with defaul name.
        /// </summary>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="c">Midpoint</param>
        public Sigmf(double a, double c)
            : this("sigmf", a, c)
        {
        }

        /// <summary>
        /// Sigmoidal membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a">Parameter 'a'</param>
        /// <param name="c">Midpoint</param>
        public Sigmf(string name, double a, double c)
        {
            this.a = a;
            this.c = c;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            return 1 / (1 + Math.Exp(0 - a * (x - c)));
        }

        public override MembershipFunction Clone()
        {
            Sigmf mf = new Sigmf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a = a;
            mf.c = c;

            return mf;
        }
    }
}
