using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FuzzyLogic.Functions;

namespace FuzzyLogic.Controller
{
    public class FuzzyRule
    {
        private List<MembershipFunction> inputs;
        private List<MembershipFunction> outputs;        
        private List<double> ruleField;

        private RuleMethod rMethod;
        private AggregationMethod aMethod;  

        #region Get
        public List<MembershipFunction> Inputs
        {
            get { return inputs; }
        }
        public List<MembershipFunction> Outputs
        {
            get { return outputs; }
        }
        public RuleMethod RMethod
        {
            get { return rMethod; }
        }
        #endregion
              
        /// <summary>
        /// Create new Rule for fuzzy controller.
        /// </summary>
        /// <param name="inputs">All input membership functions</param>
        /// <param name="outputs">All output membership functions</param>
        public FuzzyRule(MembershipFunction[] inputs, MembershipFunction[] outputs)
            :this(new List<MembershipFunction>(), new List<MembershipFunction>())
        {
            for (int i = 0; i < inputs.Length; i++)
                this.inputs.Add(inputs[i]);
            for (int i = 0; i < outputs.Length; i++)
                this.outputs.Add(outputs[i]);
        }

        /// <summary>
        /// Create new Rule for fuzzy controller.
        /// </summary>
        /// <param name="inputs">All input membership functions</param>
        /// <param name="outputs">All output membership functions<></param>
        public FuzzyRule(List<MembershipFunction> inputs, List<MembershipFunction> outputs)
            :this(inputs,outputs,RuleMethod.And)
        {
            
        }

        /// <summary>
        /// Create new Rule for fuzzy controller with rule method.
        /// </summary>
        /// <param name="inputs">All input membership functions</param>
        /// <param name="outputs">All output membership functions<></param>
        /// <param name="ruleMethod">Calculation methods rules</param>
        public FuzzyRule(List<MembershipFunction> inputs, List<MembershipFunction> outputs, RuleMethod ruleMethod)
        {
            this.inputs = inputs;
            this.outputs = outputs;
            this.rMethod = ruleMethod;

            this.aMethod=AggregationMethod.Min;

            ruleField = new List<double>();
        }


        private bool IsRuleFired(double[] inputValues)
        {
                double min = ruleField[0];
                for (int i = 1; i < inputValues.Length; i++)
                    min = Math.Min(min, ruleField[i]);
                if (min > 0)
                    return true;
                else
                    return false;
        }

        private double AggregationValue(double[] inputValues)
        {
                switch (aMethod)
                {
                    case AggregationMethod.Min:
                        {
                            double min = ruleField[0];
                            for (int i = 1; i < inputValues.Length; i++)
                                min = Math.Min(min, ruleField[i]);
                            return min;
                        }
                    case AggregationMethod.Max:
                        throw new NotImplementedException();
                    default: return 1;
                }
        }

        /// <summary>
        /// Chekck rule.
        /// </summary>
        /// <param name="inputValues">Input value from senzors</param>
        /// <returns>List of Membership function with changed height, if rule is not fired return null</returns>
        public List<MembershipFunction> ChceckRule(double[] inputValues)
        {
            if (inputValues.Length == inputs.Count)
            {
                List<MembershipFunction> outMfs=new List<MembershipFunction>();
                ruleField.Clear();
                for (int i = 0; i < inputs.Count; i++)
                    ruleField.Add(inputs[i].GetValue(inputValues[i]));

                double aggrValue = AggregationValue(inputValues);

                if (aggrValue > 0)
                {
                    
                    foreach (MembershipFunction output in outputs)
                    {
                        MembershipFunction mf = output.Clone();
                        mf.MaxHeight = aggrValue;
                        outMfs.Add(mf);
                    }
                    return outMfs;
                }
                return null;
            }
            throw new ArgumentException("Inputs values dont match with inputs in rule");
        }

        /// <summary>
        /// Chekck rule.
        /// </summary>
        /// <param name="inputValues">Input value from senzors</param>
        /// <returns>List of Membership function with changed height, if rule is not fired return null</returns>
        public List<MembershipFunction> ChceckRule(List<double> inputValues)
        {
            double[] d = new double[inputValues.Count];
            for (int i = 0; i < inputValues.Count; i++)
                d[i] = inputValues[i];
            return ChceckRule(d);
        }
    }
}
