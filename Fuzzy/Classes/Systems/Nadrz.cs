using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy.Classes.Systems
{
    public class Nadrz
    {
        private string nazov;

        private double dlzkaM;         
        private double vyskaM;         
        private double sirkaM;         

        private double mnozstvoM3;     
        private double maxMnozstvoM3;  

        public double VyskaHladinyMeter      
        {
            get { return mnozstvoM3 / (dlzkaM * sirkaM); }
        }

        public Nadrz(double dlzka, double sirka, double vyska, double mnozstvoNaplnenia, string nazov)
        {
            this.nazov = nazov;

            dlzkaM = dlzka;
            sirkaM = sirka;
            vyskaM = vyska;

            mnozstvoM3 = mnozstvoNaplnenia;
            maxMnozstvoM3 = dlzka * sirka * vyska;
        }

        public void Pritok(double pritok)
        {
            mnozstvoM3 += pritok;            
        }

        public double Odtok(double odtok)
        {
            if (mnozstvoM3 >= odtok)
            {
                mnozstvoM3 -= odtok;
                return odtok;
            }
            else
            {
                odtok = mnozstvoM3;
                mnozstvoM3 = 0;
                return odtok;
            }
        }

        public void TestHladiny()
        {
            if (mnozstvoM3 > maxMnozstvoM3)
                throw new ApplicationException("Nadmerne množstvo v nadrži '" + nazov + "'!\nVýška hladiny (" + VyskaHladinyMeter.ToString("#0.0000") + " m) v nádrži je viac ako maximalna hodnota.");
        }
    }
}
