using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy.Classes.Systems
{
    public class Rura
    {
        private double priemerM;
        private double prietokM3;

        private double prietokAktual;

        private double otvorPercent;

        private Nadrz vstup;
        private Nadrz vystup;

        private const double G_ZRYCH = 9.8065;

        #region Get
        public double PrietokM3
        {
            get { return prietokAktual; }
        }

        public double OtvorPercent
        {
            get { return otvorPercent; }
        }
        #endregion

        #region Konstruktory
        private Rura(double priemer)
        {
            priemerM = priemer;
            otvorPercent = 0;

            vstup = null;
            vystup = null;
        }

        public Rura(double priemer, double vstup, Nadrz vystup)
        {
            priemerM = priemer;
            prietokM3 = vstup;

            otvorPercent = 100;

            this.vstup = null;
            this.vystup = vystup;
        }

        public Rura(double priemer, Nadrz vstup, Nadrz vystup)
        {
            priemerM = priemer;
            prietokM3 = 0;

            this.vstup = vstup;
            this.vystup = vystup;
        }

        public Rura(double priemer, Nadrz vstup)
        {
            priemerM = priemer;
            prietokM3 = 0;

            this.vstup = vstup;
            this.vystup = null;
        }
        #endregion

        public void VypocetPrietoku(double vystup, double pritok)
        {
            if (vystup <= -43.3)
                OtvorenieVentilu(0);
            else
                OtvorenieVentilu(((Math.Pow(1.096478, vystup) * pritok) / ((VypocetPrietok > 0 ? VypocetPrietok : 0.01))) * 100);
            
            /*
            if (vystup < 5)
                OtvorenieVentilu(0);
            else
                if (vystup > 95)
                    OtvorenieVentilu(100);
                else
                    OtvorenieVentilu((vystup / 50 * pritok) / (VypocetPrietok > 0.01 ? VypocetPrietok : 0.01) * 100);
                    //OtvorenieVentilu(Math.Pow(1.096478, vystup - 50) * pritok / (VypocetPrietok > 0 ? VypocetPrietok : 0.01) * 100);
            */
        }

        public void OtvorenieVentilu(double percenta)
        {
            if (percenta > 100)
                percenta = 100;
            if (percenta < 0)
                percenta = 0;

            if (priemerM != 0)
            {
                otvorPercent = Math.Round(percenta / 100, 4);
                prietokM3 = VypocetPrietoku();
            }
        }
        
        public void CyklusPrietoku(double xSec)
        {
            prietokAktual = prietokM3;
            if (priemerM != 0)
                prietokAktual = VypocetPrietoku();
            prietokAktual/=xSec;
            double odtok = prietokAktual;
            
            if (vstup != null)
                odtok = vstup.Odtok(prietokAktual);
            if (vystup != null)
                if (odtok != prietokM3)
                    vystup.Pritok(odtok);
                else
                    vystup.Pritok(prietokAktual);
        }

        private double VypocetPrietoku()
        {
            return otvorPercent * VypocetPrietok; //Math.PI * Math.Pow(priemerM / 2, 2) * Math.Sqrt(2 * G_ZRYCH * vstup.VyskaHladinyMeter);
        }

        private double VypocetPrietok
        {
            get { return Math.PI * Math.Pow(priemerM / 2, 2) * Math.Sqrt(2 * G_ZRYCH * vstup.VyskaHladinyMeter); }
        }
    }
}
