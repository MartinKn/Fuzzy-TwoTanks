using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy.Classes.Saving
{
    public class Data
    {
        private List<double> cas;
        
        private List<List<double>> hladiny;
        private List<List<double>> prietoky;

        private Parametre parametre;

        private double[] otvorenieVentilu;

        #region Get&Set
        public List<double> Cas
        {
            get { return cas; }
            set { cas = value; }
        }

        public List<List<double>> Hladiny
        {
            get { return hladiny; }
            set { hladiny = value; }
        }

        public List<List<double>> Prietoky
        {
            get { return prietoky; }
            set { prietoky = value; }
        }

        public Parametre Parametre
        {
            get { return parametre; }
        }

        public double[] OtvorenieVentilu
        {
            get { return otvorenieVentilu; }
            set { otvorenieVentilu = value; }
        }
        #endregion

        public Data(Parametre parametre)
        {
            this.parametre =parametre;

            cas = new List<double>();

            hladiny = new List<List<double>>();
            hladiny.Add(new List<double>());
            hladiny.Add(new List<double>());

            prietoky = new List<List<double>>();
            prietoky.Add(new List<double>());
            prietoky.Add(new List<double>());

            otvorenieVentilu = new double[2];
            otvorenieVentilu[0] = 0;
            otvorenieVentilu[1] = 0;
        }

        public Data(Parametre parametre, List<double> cas, List<List<List<double>>> hodnoty, double[] otvory)
        {
            this.parametre = parametre;
            this.cas = cas;

            hladiny = hodnoty[0];
            prietoky = hodnoty[1];            

            otvorenieVentilu = new double[2];
            otvorenieVentilu[0] = otvory[0];
            otvorenieVentilu[1] = otvory[1];
        }

        public void InitialtData()
        {
            cas.Add(0);

            hladiny[0].Add(parametre.Nadrz1Hladina);
            hladiny[1].Add(parametre.Nadrz2Hladina);

            prietoky[0].Add(0);
            prietoky[1].Add(0);
        }
    }
}
