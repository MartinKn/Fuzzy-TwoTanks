using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Fuzzy.Classes.Saving
{
    public static class SaveLoad
    {
        public static void SaveData(string path, Data data)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path, false, Encoding.UTF8);
                sw.WriteLine("#Parametre#");
                sw.WriteLine(data.Parametre.Nadrz1Vyska);
                sw.WriteLine(data.Parametre.Nadrz1Sirka);
                sw.WriteLine(data.Parametre.Nadrz1Dlzka);

                sw.WriteLine(data.Parametre.Nadrz2Vyska);
                sw.WriteLine(data.Parametre.Nadrz2Sirka);
                sw.WriteLine(data.Parametre.Nadrz2Dlzka);

                sw.WriteLine(data.Parametre.Rura12Priemer);
                sw.WriteLine(data.Parametre.Rura20Priemer);

                sw.WriteLine(data.Parametre.Rura01Pritok);

                sw.WriteLine(data.Parametre.Pozadovana);

                sw.WriteLine("#Ventil#");
                sw.WriteLine(data.OtvorenieVentilu[0] + ";" + data.OtvorenieVentilu[1]);

                sw.WriteLine("#Cas#");
                sw.Write(data.Cas[0]);
                for (int i = 1; i < data.Cas.Count; i++)
                    sw.Write(";" + data.Cas[i].ToString("0.##"));
                sw.WriteLine();

                sw.WriteLine("#Hladina1#");
                sw.Write(data.Hladiny[0][0]);
                for (int i = 1; i < data.Hladiny[0].Count; i++)
                    sw.Write(";" + data.Hladiny[0][i]);
                sw.WriteLine();

                sw.WriteLine("#Hladina2#");
                sw.Write(data.Hladiny[1][0]);
                for (int i = 1; i < data.Hladiny[1].Count; i++)
                    sw.Write(";" + data.Hladiny[1][i]);
                sw.WriteLine();

                sw.WriteLine("#Prietok1#");
                sw.Write(data.Prietoky[0][0]);
                for (int i = 1; i < data.Hladiny[0].Count; i++)
                    sw.Write(";" + data.Prietoky[0][i]);
                sw.WriteLine();

                sw.WriteLine("#Prietok2#");
                sw.Write(data.Prietoky[1][0]);
                for (int i = 1; i < data.Hladiny[1].Count; i++)
                    sw.Write(";" + data.Prietoky[1][i]);
                sw.WriteLine();
                sw.Close();
                sw = null;
            }
            catch (Exception)
            {
                if (sw != null)
                    sw.Close();
                throw new ArgumentException("Chyba pri ukladaní súboru!");            
            }
            
        }

        public static void LoadData(string path, MainForm main)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(path, Encoding.UTF8);
                string allString = sr.ReadToEnd();
                string[] parse = allString.Split('#');

                double[] ventily = new double[2];
                double[] d = new double[12];

                List<double> cas = new List<double>();

                List<double> hladina1 = new List<double>();
                List<double> hladina2 = new List<double>();

                List<double> prietok1 = new List<double>();
                List<double> prietok2 = new List<double>();

                string[] smallParse;

                for (int i = 2; i < parse.Length; i += 2)
                {
                    switch (i)
                    {
                        case 2:
                            {
                                smallParse = parse[i].Split('\n');
                                for (int j = 1; j < smallParse.Length - 1; j++)
                                    d[j - 1] = Convert.ToDouble(smallParse[j].Substring(0, smallParse[j].Length));
                                d[11] = d[9];
                                d[9] = 0;
                                break;
                            }
                        case 4:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                ventily[0] = Convert.ToDouble(smallParse[0]);
                                ventily[1] = Convert.ToDouble(smallParse[1]);
                                break;
                            }
                        case 6:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                for (int j = 0; j < smallParse.Length; j++)
                                    cas.Add(Convert.ToDouble(smallParse[j]));
                                break;
                            }
                        case 8:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                for (int j = 0; j < smallParse.Length; j++)
                                    hladina1.Add(Convert.ToDouble(smallParse[j]));
                                break;
                            }
                        case 10:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                for (int j = 0; j < smallParse.Length; j++)
                                    hladina2.Add(Convert.ToDouble(smallParse[j]));
                                break;
                            }
                        case 12:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                for (int j = 0; j < smallParse.Length; j++)
                                    prietok1.Add(Convert.ToDouble(smallParse[j]));
                                break;
                            }
                        case 14:
                            {
                                smallParse = parse[i].Substring(1, parse[i].Length - 1).Split(';');
                                for (int j = 0; j < smallParse.Length; j++)
                                    prietok2.Add(Convert.ToDouble(smallParse[j]));
                                break;
                            }
                    }
                }
                Parametre p = new Parametre(d);

                List<List<double>> listd = new List<List<double>>();
                listd.Add(hladina1);
                listd.Add(hladina2);

                List<List<double>> listc = new List<List<double>>();
                listc.Add(prietok1);
                listc.Add(prietok2);

                List<List<List<double>>> hodnoty = new List<List<List<double>>>();
                hodnoty.Add(listd);
                hodnoty.Add(listc);

                main.CreateDataFromFile(p, cas, hodnoty, ventily);
            }
            catch (Exception)            
            {
                if (sr != null)
                    sr.Close();
                throw new ArgumentException("Nesprávny formát súboru!");
            }
            
        }
    }
}
