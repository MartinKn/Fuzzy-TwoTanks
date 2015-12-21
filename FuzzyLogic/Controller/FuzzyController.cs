using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using FuzzyLogic.Functions;
using FuzzyLogic.Functions.MF;

namespace FuzzyLogic.Controller
{
    public class FuzzyController
    {
        private List<LinguisticVariable> inputs;
        private List<LinguisticVariable> outputs;
        private List<FuzzyRule> rules;

        #region Get
        public List<LinguisticVariable> Inputs
        {
            get { return inputs; }
        }

        public List<LinguisticVariable> Outputs
        {
            get { return outputs; }
        }

        public List<FuzzyRule> Rules
        {
            get { return rules; }
        }

        public int NumberOfInputs
        {
            get { return inputs.Count; }
        }

        public int NumberOfOutputs
        {
            get { return outputs.Count; }
        }
        #endregion

        /// <summary>
        /// Create controller with inputs, outputs and rules
        /// </summary>
        /// <param name="inputs">All controller inputs</param>
        /// <param name="outputs">All controller outputs</param>
        /// <param name="rules">All controller rules</param>
        public FuzzyController(List<LinguisticVariable> inputs, List<LinguisticVariable> outputs, List<FuzzyRule> rules)
        {
            this.inputs = inputs;
            this.outputs = outputs;
            this.rules = rules;
        }

        /// <summary>
        /// Calculete all needed variable from sensors input and return output level
        /// </summary>
        /// <param name="inputValue">All inputs from sensors</param>
        /// <returns>Output level</returns>
        public List<double> Calculate(List<double> inputValue)
        {
            double[] inputD=new double[inputValue.Count];
            for (int i = 0; i < inputValue.Count; i++)
                inputD[i] = inputValue[i];
            return Calculate(inputD);
        }

        /// <summary>
        /// Calculete all needed variable from sensors input and return output level
        /// </summary>
        /// <param name="inputValue">All inputs from sensors</param>
        /// <returns>Output level</returns>
        public List<double> Calculate(double[] inputValue)
        {
            try
            {
                if (inputValue.Length != NumberOfInputs)
                    throw new ArgumentException("Inputs values dont match with inputs in fuzzy controller");
                for (int i = 0; i < inputValue.Length; i++)
                {
                    if (inputValue[i] > inputs[i].RangeMax)
                        inputValue[i] = inputs[i].RangeMax;
                    if (inputValue[i] < inputs[i].RangeMin)
                        inputValue[i] = inputs[i].RangeMin;
                }

                List<double> outNumbers = new List<double>();
                List<List<MembershipFunction>> mfs = new List<List<MembershipFunction>>();
                List<MembershipFunction> namedMf = new List<MembershipFunction>();
                List<MembershipFunction> acuOut = new List<MembershipFunction>();
                int numOut = 0;

                foreach (FuzzyRule r in rules)
                    mfs.Add(r.ChceckRule(inputValue));
                foreach (List<MembershipFunction> mf in mfs)
                    if (mf != null)
                    {
                        numOut = mf.Count;
                        break;
                    }
                for (int i = 0; i < numOut; i++)
                {
                    for (int k = 0; k < outputs[i].MFCount; k++)
                    {
                        for (int j = 0; j < mfs.Count; j++)
                            if (mfs[j] != null && mfs[j][i].Name == outputs[i][k].Name)
                                namedMf.Add(mfs[j][i]);
                        if (namedMf.Count > 0)
                            acuOut.Add(Acumulate(namedMf));
                        namedMf.Clear();
                    }
                    if (acuOut.Count > 0)
                        outNumbers.Add(Defuzzificate(acuOut, i));
                    acuOut.Clear();
                }
                return outNumbers;
            }
            catch (ThreadAbortException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Fuzzy Controller Error (Message) - " + ex.Message);
            }            
        }

        private MembershipFunction Acumulate(List<MembershipFunction> mfs)
        {
            MembershipFunction max = mfs[0];
            for (int i = 1; i < mfs.Count; i++)
                if (max.MaxHeight < mfs[i].MaxHeight)
                    max = mfs[i];
            return max;
        }


        private double Defuzzificate(List<MembershipFunction> mfs, int i)
        {
            double num = 0;
            double den = 0;

            double maxInPoint = 0;

            for (double u = outputs[i].RangeMin; u < outputs[i].RangeMax; u += (outputs[i].RangeMax - outputs[i].RangeMin) / 500)
            {
                maxInPoint = 0;
                foreach (MembershipFunction mf in mfs)
                    if (maxInPoint < mf.GetValue(u))
                        maxInPoint = mf.GetValue(u);
                num += u * maxInPoint;
                den += maxInPoint;
            }
            if (den != 0)
                return num / den;
            else
                return 0;
        }

        
    }
}
