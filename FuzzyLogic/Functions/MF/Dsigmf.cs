using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Dsigmf : MembershipFunction
    {
        private double a1, c1, a2, c2;

        private Dsigmf()
        {
        }

        /// <summary>
        /// Difference between two sigmoidal functions membership function with defaul name.
        /// </summary>
        /// <param name="a1">First 'a'</param>
        /// <param name="c1">First midpoint</param>
        /// <param name="a2">Second 'a'</param>
        /// <param name="c2">Second midpoint </param>
        public Dsigmf(double a1, double c1, double a2, double c2)
            : this("dsigmf", a1, c1, a2, c2)
        {
        }

        /// <summary>
        /// Difference between two sigmoidal functions membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="a1">First 'a'</param>
        /// <param name="c1">First midpoint</param>
        /// <param name="a2">Second 'a'</param>
        /// <param name="c2">Second midpoint </param>
        public Dsigmf(string name, double a1, double c1, double a2, double c2)
        {
            this.a1 = a1;            
            this.c1 = c1;
            this.a2 = a2;
            this.c2 = c2;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            return 1 / (1 + Math.Exp(0 - a1 * (x - c1))) - 1 / (1 + Math.Exp(0 - a2 * (x - c2)));
        }

        public override MembershipFunction Clone()
        {
            Dsigmf mf = new Dsigmf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.a1 = a1;
            mf.c1 = c1;
            mf.a2 = a2;
            mf.c2 = c2;
            
            return mf;
        }
    }
}
