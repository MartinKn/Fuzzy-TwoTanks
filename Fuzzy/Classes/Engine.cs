using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;

using Fuzzy.Classes.Systems;
using Fuzzy.Classes.Saving;

using FuzzyLogic.Functions;
using FuzzyLogic.Controller;
using FuzzyLogic.Functions.MF;

namespace Fuzzy.Classes
{
    public class Engine
    {
        private MainForm main;
        private RiadenySystem fyzSystem;
        private FuzzyController fuzzyReg = null;
        private Data data;

        private double cas;
        private int tickSpeed;
        private int tickCount;
        
        private bool run;

        private Thread thr;
        private ManualResetEvent spustene;

        private double[] vstupy;
        private List<double> vystup;


        private double pozadovana;

        private Stopwatch stopky;

        #region Get&Set
        public double CasSimulacie
        {
            get { return cas; }
            set { cas = value; }
        }

        public int Tick
        {
            get { return tickSpeed; }
            set { tickSpeed = value; }
        }

        public int TickCount
        {
            get { return tickCount; }
            set { tickCount = value; }
        }

        public double Pozadovana
        {
            get { return pozadovana; }
            set { pozadovana = value; }
        }

        public FuzzyController FuzzyReg
        {
            get { return fuzzyReg; }
            set { fuzzyReg = value; }
        }

        public ManualResetEvent Spustene
        {
            get { return spustene; }
            set { spustene = value; }
        }
        #endregion

        public Engine(MainForm mf)
        {
            main = mf;
            InicializovatRegulator();
        }

        public void Inicializovat(ref Data data)
        {
            stopky = new Stopwatch();
            this.data = data;

            fyzSystem = new RiadenySystem(data);

            if (fuzzyReg == null)
                InicializovatRegulator();

            cas = 0;
            tickSpeed = 1000;
            tickCount = 1;
            run = true;

            pozadovana = data.Parametre.Pozadovana;

            vstupy = new double[3];
            vystup = new List<double>();

            thr = new Thread(new ThreadStart(Runing));
            spustene = new ManualResetEvent(false);
            thr.Start();
        }

        public void Reset()
        {
            UkoncitEngine();

            fyzSystem = null;
            if (main.RegulPath != string.Empty)
                InicializovatRegulator();

            cas = 0;
            tickSpeed = 1000;
            tickCount = 1;
            run = false;

            spustene = null;
        }

        public void Runing()
        {
            while (run)
            {
                stopky.Reset();
                stopky.Start();
                try
                {
                    spustene.WaitOne();
                    cas += 1 / (double)tickCount;

                    if (fuzzyReg == null)
                        InicializovatRegulator();
                    Regulator();
                    fyzSystem.CyklusSystemu(tickCount);
                    main.PauseAktual();
                }
                catch (ThreadAbortException)
                { }
                catch (Exception ex)
                {
                    main.VypisChybu(ex);
                }
                
                Akutalizovat();
                stopky.Stop();
                main.AktualizaciaTrvanie(stopky.ElapsedMilliseconds);
                if ((tickSpeed / tickCount) - stopky.ElapsedMilliseconds > 0)
                    Thread.Sleep((int)((tickSpeed / tickCount) - stopky.ElapsedMilliseconds));
            }
        }

        public void UkoncitEngine()
        {
            if (spustene != null && thr != null)
            {
                spustene.Set();
                thr.Abort();
            }
        }

        public void nastavVentil(int percento, int poradie)
        {
            fyzSystem.VsetkyPotrubia[poradie].OtvorenieVentilu(percento);
            fyzSystem.AktualizujPrietok();
            main.AktualizovatPremenne(fyzSystem.Prietok);
        }

        private void Akutalizovat()
        {
            AktualizujData();
            main.AktualizovatPremenne();
        }

        private void AktualizujData()
        {
            data.Cas.Add(cas);
            for (int i = 0; i < data.Hladiny.Count; i++)
                data.Hladiny[i].Add(fyzSystem.Hladina[i]);
            for (int i = 0; i < data.Prietoky.Count; i++)
                data.Prietoky[i].Add(fyzSystem.Prietok[i + 1]);
            for (int i = 0; i < data.OtvorenieVentilu.Length; i++)
                data.OtvorenieVentilu[i] = fyzSystem.VsetkyPotrubia[i + 1].OtvorPercent;
        }

        private void Regulator()
        {
            if (main.Manual == false)
            {
                if (KontrolaRegulatora() == true)
                {
                    //vstupy[0] = -1*(data.Parametre.Nadrz1Vyska / 2 - data.Hladiny[0][data.Hladiny[0].Count - 1]) / data.Parametre.Nadrz1Vyska * 100*2;
                    //vstupy[1] = -1 * (pozadovana - data.Hladiny[1][data.Hladiny[1].Count - 1]) / data.Parametre.Nadrz2Vyska * 100;

                    vstupy[0] = -1 * (data.Parametre.Nadrz1Vyska / 2 - data.Hladiny[0][data.Hladiny[0].Count - 1]) / data.Parametre.Nadrz1Vyska * 100 * 2;

                    double p11 = -1 * ((data.Hladiny[1][data.Hladiny[1].Count - 1] * data.Parametre.Nadrz2Dlzka * data.Parametre.Nadrz2Sirka) - (pozadovana * data.Parametre.Nadrz2Dlzka * data.Parametre.Nadrz2Sirka));
                    double p12 = Math.PI * Math.Pow(data.Parametre.Rura20Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Hladiny[1][data.Hladiny[1].Count - 1]);

                    if (Math.Abs(p11) > p12)
                        if (p11 > 0)
                        {
                            vstupy[0] = 100;
                            vstupy[1] = -100;
                        }
                        else
                        {
                            if (-1 * ((data.Hladiny[0][data.Hladiny[0].Count - 1] * data.Parametre.Nadrz1Dlzka * data.Parametre.Nadrz1Sirka) - (data.Parametre.Nadrz1Vyska / 2 * data.Parametre.Nadrz1Dlzka * data.Parametre.Nadrz1Sirka)) > Math.PI * Math.Pow(data.Parametre.Rura12Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Hladiny[0][data.Hladiny[0].Count - 1]))
                                vstupy[0] = -100;
                            vstupy[1] = 100;
                        }
                    else
                        vstupy[1] = -1 * (pozadovana - data.Hladiny[1][data.Hladiny[1].Count - 1]) / data.Parametre.Nadrz2Vyska * 100;


                    if (data.Hladiny[0].Count > 1)
                        vstupy[2] = data.Hladiny[1][data.Hladiny[1].Count - 2] - data.Hladiny[1][data.Hladiny[1].Count - 1];
                    else
                        vstupy[2] = 0;

                    vystup = fuzzyReg.Calculate(vstupy);

                    if (vystup.Count > 1)
                    {
                        fyzSystem.VsetkyPotrubia[1].VypocetPrietoku(vystup[0], data.Parametre.Rura01Pritok);
                        fyzSystem.VsetkyPotrubia[2].VypocetPrietoku(vystup[1], data.Parametre.Rura01Pritok);
                    }
                }
                else
                    throw new ArgumentException("Nedostatok vstupov alebo výstupov regulátora!");
            }
        }

        private bool KontrolaRegulatora()
        {
            if (fuzzyReg.NumberOfInputs >= 3 && fuzzyReg.NumberOfOutputs >= 2)
                return true;
            return false;
        }

        private void InicializovatRegulator()
        {
            List<LinguisticVariable> inp = new List<LinguisticVariable>();
            List<LinguisticVariable> outp = new List<LinguisticVariable>();
            List<FuzzyRule> rules = new List<FuzzyRule>();

            inp.Add(new LinguisticVariable("vstup1", -100, 100));
            inp.Add(new LinguisticVariable("vstup2", -100, 100));
            inp.Add(new LinguisticVariable("vstup3", -100, 100));
            outp.Add(new LinguisticVariable("vystup1", -50, 50));
            outp.Add(new LinguisticVariable("vystup2", -50, 50));


            inp[0].AddMembershipFunction(new Trapmf(-150, -125, -75, -50));
            inp[0].AddMembershipFunction(new Trimf(-95, -50, -5));
            inp[0].AddMembershipFunction(new Trimf(-20, -10, 0));
            inp[0].AddMembershipFunction(new Trimf(-5, 0, 5));
            inp[0].AddMembershipFunction(new Trimf(0, 10, 20));
            inp[0].AddMembershipFunction(new Trimf(5, 50, 95));
            inp[0].AddMembershipFunction(new Trapmf(50, 75, 125, 150));

            inp[1].AddMembershipFunction(new Trapmf(-150, -125, -75, -50));
            inp[1].AddMembershipFunction(new Trimf(-95, -50, -5));
            inp[1].AddMembershipFunction(new Trimf(-20, -10, 0));
            inp[1].AddMembershipFunction(new Trimf(-5, 0, 5));
            inp[1].AddMembershipFunction(new Trimf(0, 10, 20));
            inp[1].AddMembershipFunction(new Trimf(5, 50, 95));
            inp[1].AddMembershipFunction(new Trapmf(50, 75, 125, 150));

            inp[2].AddMembershipFunction(new Trimf(-5.8, -3, -0.2));
            inp[2].AddMembershipFunction(new Trimf(-0.4, 0, 0.4));
            inp[2].AddMembershipFunction(new Trimf(0.2, 3, 5.8));


            outp[0].AddMembershipFunction(new Trimf(-75, -50, -30));
            outp[0].AddMembershipFunction(new Trimf(-50, -30, -15));
            outp[0].AddMembershipFunction(new Trimf(-30, -15, 0));
            outp[0].AddMembershipFunction(new Trimf(-15, 0, 15));
            outp[0].AddMembershipFunction(new Trimf(0, 15, 30));
            outp[0].AddMembershipFunction(new Trimf(15, 30, 50));
            outp[0].AddMembershipFunction(new Trimf(30, 50, 75));

            outp[1].AddMembershipFunction(new Trimf(-75, -50, -30));
            outp[1].AddMembershipFunction(new Trimf(-50, -30, -15));
            outp[1].AddMembershipFunction(new Trimf(-30, -15, 0));
            outp[1].AddMembershipFunction(new Trimf(-15, 0, 15));
            outp[1].AddMembershipFunction(new Trimf(0, 15, 30));
            outp[1].AddMembershipFunction(new Trimf(15, 30, 50));
            outp[1].AddMembershipFunction(new Trimf(30, 50, 75));


            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][0], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][1], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][2], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][2] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][4], outp[1][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[1][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][3], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[1][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][4], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][5], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][5], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][6], inp[2][0] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));


            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][0], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][1], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][2], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][2] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][4], outp[1][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[1][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][3], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[1][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][4], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][5], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][5], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][6], inp[2][1] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));


            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][0], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][1], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][1] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][2], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][2] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[1][0] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][4], outp[1][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[1][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][3], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[1][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][2] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][3] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][4], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][4] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][5], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][5], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));

            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][0], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][1], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][2], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][3], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][0], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][4], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][5], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][5] }));
            rules.Add(new FuzzyRule(new MembershipFunction[] { inp[0][6], inp[1][6], inp[2][2] }, new MembershipFunction[] { outp[0][6], outp[0][6] }));


            fuzzyReg = new FuzzyLogic.Controller.FuzzyController(inp, outp, rules);
        }
    }
}
