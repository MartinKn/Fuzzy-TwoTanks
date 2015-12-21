using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy.Classes
{
    public class Parametre
    {
        private double nadrz1Vyska;
        private double nadrz1Sirka;
        private double nadrz1Dlzka;

        private double nadrz2Vyska;
        private double nadrz2Sirka;
        private double nadrz2Dlzka;

        private double nadrz1Hladina;        
        private double nadrz2Hladina;
        
        private double rura12Priemer;
        private double rura20Priemer;

        private double rura01Pritok;

        private double pozadovana;

        #region Get
        public double Nadrz1Vyska
        {
            get { return nadrz1Vyska; }
        }        

        public double Nadrz1Sirka
        {
            get { return nadrz1Sirka; }
        }        

        public double Nadrz1Dlzka
        {
            get { return nadrz1Dlzka; }
        }        

        public double Nadrz2Vyska
        {
            get { return nadrz2Vyska; }
        }
        
        public double Nadrz2Sirka
        {
            get { return nadrz2Sirka; }
        }

        public double Nadrz2Dlzka
        {
            get { return nadrz2Dlzka; }
        }

        public double Nadrz1Hladina
        {
            get { return nadrz1Hladina; }
        }

        public double Nadrz2Hladina
        {
            get { return nadrz2Hladina; }
        }

        public double Rura12Priemer
        {
            get { return rura12Priemer; }
        }

        public double Rura20Priemer
        {
            get { return rura20Priemer; }
        }

        public double Rura01Pritok
        {
            get { return rura01Pritok; }
        }

        public double Pozadovana
        {
            get { return pozadovana; }
        }
        #endregion

        public Parametre(double[] parametre)
        {
            nadrz1Vyska = parametre[0];
            nadrz1Sirka = parametre[1];
            nadrz1Dlzka = parametre[2];

            nadrz2Vyska = parametre[3];
            nadrz2Sirka = parametre[4];
            nadrz2Dlzka = parametre[5];
            
            rura12Priemer = parametre[6];
            rura20Priemer = parametre[7];

            rura01Pritok = parametre[8];

            nadrz1Hladina = parametre[9];
            nadrz2Hladina = parametre[10];

            pozadovana = parametre[11];
        }
    }
}
