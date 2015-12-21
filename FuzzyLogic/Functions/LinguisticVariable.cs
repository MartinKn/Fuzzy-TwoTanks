using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Functions
{
    public class LinguisticVariable
    {
        private List<MembershipFunction> mfs;

        private string name;        
        private double rangeMin;        
        private double rangeMax;

        #region Get&Set
        /// <summary>
        /// Get or set name of linguistic variable.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Get minimum range of linguistic variable.
        /// </summary>
        public double RangeMin
        {
            get { return rangeMin; }
        }

        /// <summary>
        /// Get maximum range of linguistic variable.
        /// </summary>
        public double RangeMax
        {
            get { return rangeMax; }
        }

        public int MFCount
        {
            get { return mfs.Count; }
        }

        #endregion

        /// <summary>
        /// Searches Membership function of specified name in linguistic variable.
        /// </summary>
        /// <param name="name">Search name</param>
        /// <returns>Membership function, when no result return null</returns>
        public MembershipFunction this[string name]
        {
            get
            {
                foreach (MembershipFunction mf in mfs)
                {
                    if (mf.Name == name)
                        return mf;
                }
                return null;
            }
        }

        /// <summary>
        /// Membership function in linquistic variable
        /// </summary>
        /// <param name="index">Index of membership function</param>
        /// <returns>Membership function</returns>
        public MembershipFunction this[int index]
        {
            get { return mfs[index]; }
        }
        
        /// <summary>
        /// Create lingquistic variable with default values.
        /// </summary>
        public LinguisticVariable()
            : this("LinguisticVariable", 0, 10)
        {
        }

        /// <summary>
        /// Create lingquistic variable with name and default values.
        /// </summary>
        /// <param name="name">Name of lingquistic variable</param>
        public LinguisticVariable(string name)
            : this(name, 0, 10)
        {
        }

        /// <summary>
        /// Create lingquistic variable.
        /// </summary>
        /// <param name="name">Name of lingquistic variable</param>
        /// <param name="rangeMin">Set minimum range of lingquistic variable</param>
        /// <param name="rangeMax">Set maximum range of lingquistic variable</param>
        public LinguisticVariable(string name, double rangeMin, double rangeMax)
            : this(name, rangeMin, rangeMax, new List<MembershipFunction>())
        {
        }
        
        /// <summary>
        /// Create lingquistic variable.
        /// </summary>
        /// <param name="name">Name of lingquistic variable</param>
        /// <param name="rangeMin">Set minimum range of lingquistic variable</param>
        /// <param name="rangeMax">Set maximum range of lingquistic variable</param>
        /// <param name="membershipFunctions">List of membership functions</param>
        public LinguisticVariable(string name, double rangeMin, double rangeMax, List<MembershipFunction> membershipFunctions)
        {
            this.mfs = new List<MembershipFunction>();
            foreach (MembershipFunction mf in membershipFunctions)
                AddMembershipFunction(mf);
            this.name = name;
            this.rangeMin = rangeMin;
            this.rangeMax = rangeMax;
        }

        /// <summary>
        /// Add new membership function to list.
        /// </summary>
        /// <param name="function">Membership function</param>
        public void AddMembershipFunction(MembershipFunction function)
        {
            mfs.Add(function);
            mfs[mfs.Count - 1].Name = CheckName(mfs[mfs.Count - 1].Name);
        }

        private string CheckName(string newName)
        {
            int i = 0;
            for(int j=0;j<mfs.Count-1;j++)
                if (mfs[j].Name == newName )
                {

                    if (Char.IsDigit(newName[newName.Length - 1]) == true)
                        newName = newName.Substring(0, newName.Length - 1) + (++i).ToString();
                    else
                        newName = newName + (++i).ToString();
                    j = 0;
                }
            return newName;
        }
    }
}
