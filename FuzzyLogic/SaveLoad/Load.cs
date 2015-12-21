using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Globalization;

using FuzzyLogic.Functions;
using FuzzyLogic.Functions.MF;
using FuzzyLogic.Controller;

namespace FuzzyLogic.SaveLoad
{
    public static class Load
    {
        private static List<LinguisticVariable> inputs;
        private static List<LinguisticVariable> outputs;
        
        /// <summary>
        /// Load fuzzy controller from file
        /// </summary>
        /// <param name="path">Path of file</param>
        /// <returns>Generated Fuzzy controller</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static FuzzyController ControllerLoad(string path)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path);
                string allRead = sr.ReadToEnd();

                allRead = allRead.Replace('.', ',');

                string[] parseString = allRead.Split('#');
                if (parseString.Length % 2 != 1)
                    throw new ArgumentException("File is in incorrect format!");

                inputs = new List<LinguisticVariable>();
                outputs = new List<LinguisticVariable>();

                List<FuzzyRule> rules = new List<FuzzyRule>();

                for (int i = 1; i < parseString.Length - 1; i += 2)
                {
                    switch (parseString[i])
                    {
                        case "Input": inputs.Add(ParseInputs(parseString[i+1])); break;
                        case "Output": outputs.Add(ParseInputs(parseString[i + 1])); break;
                        case "Rule": rules = ParseRules(parseString[i + 1]); break;
                        case "Parameters": break;
                        default: break;
                    }
                }
                sr.Close();
                return new FuzzyController(inputs, outputs, rules);
            }
            catch (Exception ex)
            {
                if (sr != null)
                    sr.Close();
                throw new ArgumentException("Controller loaded with errors - " + ex.Message);
            }
        }

        private static LinguisticVariable ParseInputs(string text)
        {
            try
            {
                string[] parseString = text.Split('\n');
                List<string> dataStr = DataString(text);
                List<MembershipFunction> mfs = new List<MembershipFunction>();
                bool first = true;

                string name = "input", rangeMinS = "0", rangeMaxS = "1", nameMf = string.Empty, typeMf = string.Empty, check;
                string[] ss;
                List<string> paraMf = new List<string>();
                foreach (string s in dataStr)
                {
                    ss = s.Split('=');
                    check = ss[0].Replace(' ', '\r').Trim();
                    

                    switch (check)
                    {
                        case "name": name = ss[1].Replace(" ", "").Trim(); break;
                        case "rangemin": rangeMinS = ss[1].Replace(' ', '\r').Trim(); break;
                        case "rangemax": rangeMaxS = ss[1].Replace(' ', '\r').Trim(); break;
                        case "namemf": nameMf = ss[1].Replace(' ', '\r').Trim(); break;
                        case "params": paraMf = ParseParam(ss[1].Split(' ')); break;
                        case "type": typeMf = ss[1].Replace(' ', '\r').Trim(); break;
                        default:
                            {
                                if (ss[0][0] == '<')
                                    if (first)
                                        first = false;
                                    else
                                    {
                                        switch (typeMf)
                                        {
                                            case "trimf": mfs.Add(new Trimf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]))); break;
                                            case "dsigmf": mfs.Add(new Dsigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                                            case "gauss2mf": mfs.Add(new Gauss2mf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                                            case "gaussmf": mfs.Add(new Gaussmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                                            case "gbellmf": mfs.Add(new Gbellmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]))); break;
                                            case "pimf": mfs.Add(new Pimf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                                            case "psigmf": mfs.Add(new Psigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                                            case "sigmf": mfs.Add(new Sigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                                            case "smf": mfs.Add(new Smf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                                            case "trapmf": mfs.Add(new Trapmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                                            case "zmf": mfs.Add(new Zmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                                            default: throw new ArgumentException("Nesprávny názov funckie príslušnosti vo vstupe alebo výstutupe!\nPovolené hodnoty: 'trimf' 'dsigmf' 'gauss2mf' 'gaussmf' 'gbellmf' 'pimf' 'psigmf' 'sigmf' 'smf' 'trapmf' 'zmf'");
                                        }
                                        nameMf = string.Empty;
                                        typeMf = string.Empty;
                                        paraMf.Clear();
                                    }
                                break;
                            }
                    }
                }
                switch (typeMf)
                {
                    case "trimf": mfs.Add(new Trimf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]))); break;
                    case "dsigmf": mfs.Add(new Dsigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                    case "gauss2mf": mfs.Add(new Gauss2mf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                    case "gaussmf": mfs.Add(new Gaussmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                    case "gbellmf": mfs.Add(new Gbellmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]))); break;
                    case "pimf": mfs.Add(new Pimf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                    case "psigmf": mfs.Add(new Psigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                    case "sigmf": mfs.Add(new Sigmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                    case "smf": mfs.Add(new Smf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                    case "trapmf": mfs.Add(new Trapmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]), Convert.ToDouble(paraMf[2]), Convert.ToDouble(paraMf[3]))); break;
                    case "zmf": mfs.Add(new Zmf(nameMf, Convert.ToDouble(paraMf[0]), Convert.ToDouble(paraMf[1]))); break;
                    default: throw new ArgumentException("Incorrect type of membership function in input or output!\n Allowed value: 'trimf' 'dsigmf' 'gauss2mf' 'gaussmf' 'gbellmf' 'pimf' 'psigmf' 'sigmf' 'smf' 'trapmf' 'zmf'");
                }                                                                                                                             
                double min = Convert.ToDouble(rangeMinS);                                                                              
                double max = Convert.ToDouble(rangeMaxS);                                                                              
                                                                                                                                       
                return new LinguisticVariable(name, min, max, mfs);                                                                    
            }                                                                                                                          
            catch (ArgumentOutOfRangeException)                                                                                        
            {                                                                                                                          
                throw new ArgumentOutOfRangeException("Incorrect count of arguments to membership functions");                                                   
            }                                                                                                                          
            catch (Exception)
            {
                throw new ArgumentException("Incorrect loaded inputs and outputs");
            }
        }


        private static List<FuzzyRule> ParseRules(string text)
        {
            try
            {
                List<FuzzyRule> rules = new List<FuzzyRule>();
                MembershipFunction[] inputsMf;
                MembershipFunction[] outputsMf;
                List<string> dataStr = DataString(text);

                RuleMethod rm = new RuleMethod();
                rm = RuleMethod.And;


                string[] ioParse, inRule, outRule;

                int j = 0;

                foreach (string s in dataStr)
                    switch (s[0])
                    {
                        case 'm':
                            {
                                switch (s.Split('=')[1])
                                {
                                    case "and": rm = RuleMethod.And; break;
                                    case "or": rm = RuleMethod.Or; break;
                                    default: rm = RuleMethod.And; break;
                                }
                                break;
                            }
                        default:
                            {
                                ioParse = s.Split(',');
                                inRule = ioParse[0].Split(' ');
                                outRule = ioParse[1].Split(' ');

                                 j = 0;
                                 for (int i = 0; i < inRule.Length; i++)
                                     if (inRule[i].Length > 0)
                                         j++;
                                inputsMf = new MembershipFunction[j];
                                j=0;
                                for (int i = 0; i < outRule.Length; i++)
                                    if (outRule[i].Trim().Length > 0)
                                        j++;
                                outputsMf = new MembershipFunction[j];
                                
                                j = 0;
                                for (int i = 0; i < inRule.Length; i++)
                                    if (inRule[i].Length > 0)
                                    {
                                        inputsMf[j] = inputs[j][Convert.ToInt32(inRule[i])].Clone();
                                        j++;
                                    }
                                j = 0;
                                for (int i = 0; i < outRule.Length; i++)
                                    if (outRule[i].Trim().Length > 0)
                                    {
                                        outputsMf[j] = outputs[j][Convert.ToInt32(outRule[i])].Clone();
                                        j++;
                                    }
                                rules.Add(new FuzzyRule(new List<MembershipFunction>(inputsMf), new List<MembershipFunction>(outputsMf), rm));
                                break;
                            }
                    }
                return rules;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Incorrect loaded controller rules!\n" + ex.Message);
            }

        }

        private static List<string> DataString(string text)
        {
            string[] parseString = text.Split('\n');
            List<string> dataStr = new List<string>();

            for (int i = 0; i < parseString.Length; i++)
                if (parseString[i].Trim().Length > 0)
                    dataStr.Add(parseString[i].Trim());

            return dataStr;
        }

        private static List<string> ParseParam(string[] text)
        {
            List<string> dataStr = new List<string>();

            for (int i = 0; i < text.Length; i++)
                if (text[i].Trim().Length > 0)
                    dataStr.Add(text[i].Trim());

            return dataStr;
        }
    }
}
