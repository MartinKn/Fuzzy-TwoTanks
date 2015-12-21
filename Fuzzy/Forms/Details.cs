using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fuzzy.Classes;
using Fuzzy.Classes.Saving;

namespace Fuzzy.Forms
{
    public partial class Details : Form
    {
        private MainForm main;
        private Data data;

        private bool notSet = true;
        private int jednotkaPrietok = 1;
        private bool lastTime = true;

        private string format = "#,##0.0000";

        public Details(MainForm main, ref Data data)
        {
            this.main = main;
            this.data = data;
            InitializeComponent();
        }

        #region Eventy
        //Form - Nacitanie 
        private void Details_Load(object sender, EventArgs e)
        {
            rbMeter.Checked = true;
            rbTlast.Checked = true;
            if (data != null)
            {
                lDSV1.Text = data.Parametre.Nadrz1Dlzka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz1Sirka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz1Vyska.ToString("#,#0.####");
                lDSV2.Text = data.Parametre.Nadrz2Dlzka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz2Sirka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz2Vyska.ToString("#,#0.####");
                lP0Prietok.Text = (data.Parametre.Rura01Pritok * jednotkaPrietok).ToString("#,#0.####");
                lP1Priemer.Text = data.Parametre.Rura12Priemer.ToString("#,#0.####");
                lP2Priemer.Text = data.Parametre.Rura20Priemer.ToString("#,#0.####");                

                notSet = false;
                Aktualizacia();
            }
        }

        //RadioTl - Zmena ozancenia
        private void rbChange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLitre.Checked == true)
            {
                jednotkaPrietok = 1000;
                lJprietok1.Text = "l";
                lJprietok.Text = "l";
                lJprietok2s.Text = "l/s";
                lJprietok3s.Text = "l/s";
                lJprietok4s.Text = "l/s";
                format = "#,#0.00";
            }
            if (rbMeter.Checked == true)
            {
                jednotkaPrietok = 1;
                lJprietok1.Text = "m^3";
                lJprietok.Text = "m^3";
                lJprietok2s.Text = "m^3/s";
                lJprietok3s.Text = "m^3/s";
                lJprietok4s.Text = "m^3/s";
                format = "#,#0.0000";
            }

            notSet = true;
            if (data != null)
            {
                lP0Prietok.Text = (data.Parametre.Rura01Pritok * jednotkaPrietok).ToString("#,#0.00");
                Aktualizacia();
            }
            else
                lP0Prietok.Text = 0.ToString("#,#0.00");
        }

        //RadioTl - Zmena casu
        private void rbTlast_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTlast.Checked == true)
            {
                lastTime = true;
                tbTime.Enabled = false;
                bChangeTime.Enabled = false;
                if (data != null)
                    Aktualizacia(data.Hladiny[0].Count - 1);
            }
            if (rbTset.Checked == true)
            {
                if (data != null)
                {
                    lastTime = false;
                    tbTime.Enabled = true;
                    bChangeTime.Enabled = true;
                }
                else
                    rbTlast.Checked = true;
            }
        }

        //Tlacidlo - Zobraz konkretny cas
        private void bChangeTime_Click(object sender, EventArgs e)
        {
            try
            {
                Aktualizacia(Convert.ToDouble(tbTime.Text));
            }
            catch (Exception)
            {
                tbTime.Text = "";
            }

        }

        //Numeric - Stlacenie klavesy
        private void tbTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                bChangeTime.PerformClick();
        }

        //Form - Zavretie okna
        private void Details_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.ClosingOtherForm(this);
        }
        #endregion

        #region Ostatne
        //Aktualizacia udajov
        public void Aktualizacia()
        {
            if (lastTime == true)
                this.Invoke((MethodInvoker)delegate
                {
                    AktualizaciaPoslendy();
                    if (notSet)
                    {
                        lDSV1.Text = data.Parametre.Nadrz1Dlzka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz1Sirka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz1Vyska.ToString("#,#0.####") + " m";
                        lDSV2.Text = data.Parametre.Nadrz2Dlzka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz2Sirka.ToString("#,#0.####") + " x " + data.Parametre.Nadrz2Vyska.ToString("#,#0.####") + " m";
                        lP0Prietok.Text = (data.Parametre.Rura01Pritok * jednotkaPrietok).ToString("#,#0.00");
                        lP1Priemer.Text = data.Parametre.Rura12Priemer.ToString("#,#0.####");
                        lP2Priemer.Text = data.Parametre.Rura20Priemer.ToString("#,#0.####");
                        notSet = false;
                    }
                });
        }

        //Aktualizacia v poslednom case
        private void AktualizaciaPoslendy()
        {
            if (data.Hladiny[0].Count > 0)
            {
                lHladina1.Text = data.Hladiny[0][data.Hladiny[0].Count-1].ToString("#,#0.0000");
                lMnozstvo1.Text = ((data.Hladiny[0][data.Hladiny[0].Count - 1] * data.Parametre.Nadrz1Dlzka * data.Parametre.Nadrz1Sirka) * jednotkaPrietok).ToString("#,#0.00");
            }
            if (data.Hladiny[1].Count > 0)
            {
                lHladina2.Text = data.Hladiny[1][data.Hladiny[1].Count - 1].ToString("#,#0.0000");
                lMnozstvo2.Text = ((data.Hladiny[1][data.Hladiny[1].Count - 1] * data.Parametre.Nadrz2Dlzka * data.Parametre.Nadrz2Sirka) * jednotkaPrietok).ToString("#,#0.00");
            }
            if (lastTime == true)
            {
                lP1Ventil.Text = (data.OtvorenieVentilu[0] * 100).ToString("#0.##");
                lP2Ventil.Text = (data.OtvorenieVentilu[1] * 100).ToString("#0.##");
            }
            else
            {
                lP1Ventil.Text = "-";
                lP2Ventil.Text = "-";
            }
            if (data.Prietoky[0].Count > 0)
                lP1Prietok.Text = (data.Prietoky[0][data.Prietoky[0].Count - 1] * jednotkaPrietok * main.Engine.TickCount).ToString(format);
            if (data.Prietoky[1].Count > 0)
                lP2Prietok.Text = (data.Prietoky[1][data.Prietoky[1].Count - 1] * jednotkaPrietok * main.Engine.TickCount).ToString(format);

            lTime.Text = ((main.Engine.CasSimulacie / 60 / 60) % 60).ToString("00") + ":" + ((main.Engine.CasSimulacie / 60) % 60).ToString("00") + "." + (main.Engine.CasSimulacie % 60).ToString("00");
        }

        //Aktualizacia v konkretnom case
        private void Aktualizacia(double cas)
        {
            int poradie = NajdiCas(cas);
            if (data.Hladiny[0].Count > 0)
            {                
                lHladina1.Text = data.Hladiny[0][poradie].ToString("#,#0.0000");
                lMnozstvo1.Text = ((data.Hladiny[0][poradie] * data.Parametre.Nadrz1Dlzka * data.Parametre.Nadrz1Sirka) * jednotkaPrietok).ToString("#,#0.00");
            }
            if (data.Hladiny[1].Count > 0)
            {
                lHladina2.Text = data.Hladiny[1][poradie].ToString("#,#0.0000");
                lMnozstvo2.Text = ((data.Hladiny[1][poradie] * data.Parametre.Nadrz2Dlzka * data.Parametre.Nadrz2Sirka) * jednotkaPrietok).ToString("#,#0.00");
            }
            if (lastTime == true)
            {
                lP1Ventil.Text = (data.OtvorenieVentilu[0] * 100).ToString("#0.##");
                lP2Ventil.Text = (data.OtvorenieVentilu[1] * 100).ToString("#0.##");
            }
            else
            {
                lP1Ventil.Text = "-";
                lP2Ventil.Text = "-";
            }
            if (data.Prietoky[0].Count > 0)
                lP1Prietok.Text = (data.Prietoky[0][poradie] * jednotkaPrietok).ToString(format);
            if (data.Prietoky[1].Count > 0)
                lP2Prietok.Text = (data.Prietoky[1][poradie] * jednotkaPrietok).ToString(format);

        }

        private int NajdiCas(double cas)
        {
            int konkrete = 0;
            for (int i = 0; i < data.Hladiny[0].Count; i++)
                if (Math.Abs(data.Cas[konkrete] - cas) > Math.Abs(data.Cas[i] - cas))
                    konkrete = i;
            tbTime.Text = data.Cas[konkrete].ToString("0.##");
            lTime.Text = ((data.Cas[konkrete] / 60 / 60) % 60).ToString("00") + ":" + ((data.Cas[konkrete] / 60) % 60).ToString("00") + "." + (data.Cas[konkrete] % 60).ToString("00");
            return konkrete;
        }

        //Resetovanie na nulu
        public void AktualizaciaZero()
        {
            lDSV1.Text = string.Empty;
            lDSV2.Text = string.Empty;
            lP0Prietok.Text = "0";
            lP1Priemer.Text = "0";
            lP2Priemer.Text = "0";
            lHladina1.Text = "0";
            lMnozstvo1.Text = "0";
            lHladina2.Text = "0";
            lMnozstvo2.Text = "0";
            lP1Prietok.Text = "0";
            lP1Ventil.Text = "0";
            lP2Prietok.Text = "0";
            lP2Ventil.Text = "0";
            notSet = true;
        }

        //Nastavi Data #main
        public void NastavData(ref Data data)
        {
            this.data = data;
        }
        #endregion
    }
}