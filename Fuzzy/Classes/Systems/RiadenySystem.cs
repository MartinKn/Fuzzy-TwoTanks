using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fuzzy.Classes.Saving;

namespace Fuzzy.Classes.Systems
{
    public class RiadenySystem
    {
        private List<Nadrz> vsetkyNadrze;        
        private List<Rura> vsetkyPotrubia;

        private double[] hladina;
        private double[] prietok;

        #region Get
        public List<Nadrz> VsetkyNadrze
        {
            get { return vsetkyNadrze; }
        }

        public List<Rura> VsetkyPotrubia
        {
            get { return vsetkyPotrubia; }
        }

        public double[] Hladina
        {
            get { return hladina; }
        }

        public double[] Prietok
        {
            get { return prietok; }
        }
        #endregion

        public RiadenySystem(Data data)
        {
            vsetkyNadrze = new List<Nadrz>();
            vsetkyPotrubia = new List<Rura>();

            vsetkyNadrze.Add(new Nadrz(data.Parametre.Nadrz1Dlzka, data.Parametre.Nadrz1Sirka, data.Parametre.Nadrz1Vyska, data.Hladiny[0][data.Hladiny[0].Count - 1] * data.Parametre.Nadrz1Sirka * data.Parametre.Nadrz1Dlzka, "Modrá"));
            vsetkyNadrze.Add(new Nadrz(data.Parametre.Nadrz2Dlzka, data.Parametre.Nadrz2Sirka, data.Parametre.Nadrz2Vyska, data.Hladiny[1][data.Hladiny[1].Count - 1] * data.Parametre.Nadrz2Dlzka * data.Parametre.Nadrz2Sirka, "Zelená"));

            vsetkyPotrubia.Add(new Rura(0, data.Parametre.Rura01Pritok, vsetkyNadrze[0]));
            vsetkyPotrubia.Add(new Rura(data.Parametre.Rura12Priemer, vsetkyNadrze[0], vsetkyNadrze[1]));
            vsetkyPotrubia.Add(new Rura(data.Parametre.Rura20Priemer, vsetkyNadrze[1]));            

            hladina = new double[vsetkyNadrze.Count];
            prietok = new double[vsetkyPotrubia.Count];
        }

        public void CyklusSystemu(double xSec)
        {
            foreach (Rura r in vsetkyPotrubia)
                r.CyklusPrietoku(xSec);
            for (int i = 0; i < hladina.Length; i++)
                hladina[i] = vsetkyNadrze[i].VyskaHladinyMeter;
            AktualizujPrietok();

            for (int i = 0; i < hladina.Length; i++)
                vsetkyNadrze[i].TestHladiny();            
        }

        public void AktualizujPrietok()
        {
            for (int i = 0; i < prietok.Length; i++)
                prietok[i] = vsetkyPotrubia[i].PrietokM3;
        }        
    }
}
