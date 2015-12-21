using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions.MF
{
    public class Gauss2mf : MembershipFunction
    {
        private double s1, c1, s2, c2;

        private Gauss2mf()
        {
        }

        /// <summary>
        /// Gaussian combination membership function with defaul name.
        /// </summary>
        /// <param name="s1">First variance</param>
        /// <param name="c1">First midpoint</param>
        /// <param name="s2">Second variance</param>
        /// <param name="c2">Second midpoint</param>
        public Gauss2mf(double s1, double c1, double s2, double c2)
            : this("gauss2mf", s1, c1, s2, c2)
        {
        }

        /// <summary>
        /// Gaussian combination membership function.
        /// </summary>
        /// <param name="name">Name of function</param>
        /// <param name="s1">First variance</param>
        /// <param name="c1">First midpoint</param>
        /// <param name="s2">Second variance</param>
        /// <param name="c2">Second midpoint</param>
        public Gauss2mf(string name, double s1, double c1, double s2, double c2)
        {
            this.s1 = s1;
            this.c1 = c1;
            this.s2 = s2;
            this.c2 = c2;
            this.Name = name;
        }

        override protected double GetY(double x)
        {
            if(x<=c1)
                return Math.Exp(0 - ((x - c1) * (x - c1)) / (2 * s1 * s1));
            if(x>=c2)
                return Math.Exp(0 - ((x - c2) * (x - c2)) / (2 * s2 * s2));            
            return 1;
        }

        public override MembershipFunction Clone()
        {
            Gauss2mf mf = new Gauss2mf();
            mf.Name = Name;
            mf.MaxHeight = MaxHeight;

            mf.s1 = s1;
            mf.c1 = c1;
            mf.s2 = s2;
            mf.c2 = c2;
            
            return mf;
        }
    }
}
