using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions
{
    public abstract class MembershipFunction
    {
        private string name;
        private double maxHeight = 1;

        /// <summary>
        /// Get name of membership function.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (value == string.Empty)
                    name = this.GetType().ToString().Split('.')[this.GetType().ToString().Split('.').Length - 1];
                else
                    name = value;
            }
        }

        /// <summary>
        /// Get or set maximum height of membership function
        /// </summary>
        public double MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        /// <summary>
        /// Geting µ(x). µ(x) is the grade of membership of an element x.
        /// </summary>
        /// <param name="x">X axis. In fuzzy U axis as univerzum.</param>
        /// <returns></returns>
        public double GetValue(double x)
        {
            double y = GetY(x);
            if (y > maxHeight)
                return maxHeight;
            else
                return y;
        }

        protected abstract double GetY(double x);

        public abstract MembershipFunction Clone();
    }
}
