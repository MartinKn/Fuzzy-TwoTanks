using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Gaussmf : MembershipFunction
    {
        private double s, c;

        private Gaussmf()
        {
        }

        /// <summary>
        /// Gaussian curve membership function with defaul name.
        /// </summary>
        /// <param name="s">Variance</param>
        /// <param name="c">Midpoint</param>
        public Gaussmf(double s, double c)
            : this("gaussmf", s, c)
        {
        }

        /// <summary>
        /// Gaussian curve membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="s">Variance</param>
        /// <param name="c">Midpoint</param>
        public Gaussmf(string name, double s, double c)
        {
            this.s = s;
            this.c = c;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            return Math.Exp(0 - ((x - c) * (x - c)) / (2 * s * s));
        }

        public override MembershipFunction Clone()
        {
            Gaussmf mf = new Gaussmf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.s = s;
            mf.c = c;

            return mf;
        }
    }
}
