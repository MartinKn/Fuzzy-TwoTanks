using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fuzzy.Forms
{
    public partial class Setting : Form
    {
        private MainForm main;
        private double[] parametre;

        private int jPrietok = 1;

        public double[] Parametre
        {
            get { return parametre; }
        }

        #region Konstanty
        private const double NADRZ1_VYSKA_MAX = 1000;
        private const double NADRZ1_SIRKA_MAX = 1000;
        private const double NADRZ1_DLZKA_MAX = 1000;

        private const double NADRZ1_VYSKA_MIN = 1;
        private const double NADRZ1_SIRKA_MIN = 1;
        private const double NADRZ1_DLZKA_MIN = 1;


        private const double NADRZ2_VYSKA_MAX = 1000;
        private const double NADRZ2_SIRKA_MAX = 1000;
        private const double NADRZ2_DLZKA_MAX = 1000;

        private const double NADRZ2_VYSKA_MIN = 1;
        private const double NADRZ2_SIRKA_MIN = 1;
        private const double NADRZ2_DLZKA_MIN = 1;


        private const double PRIEMER12_MAX = 5;
        private const double PRIEMER20_MAX = 5;

        private const double PRIEMER12_MIN = 0.05;
        private const double PRIEMER20_MIN = 0.05;


        private const double PRITOK01_MAX = 10;
        private const double PRITOK01_MIN = 0.001;
        #endregion

        public Setting(MainForm main)
        {
            this.main = main;
            InitializeComponent();
        }

        #region Eventy
        //Tlacidlo - Potvrdenie udajov
        private void bOk_Click(object sender, EventArgs e)
        {
            if (KontrolaParametrov())
            {
                parametre = new double[12];
                parametre[0] = PrevodZoStringu(tbVyska1.Text);
                parametre[1] = PrevodZoStringu(tbSirka1.Text);
                parametre[2] = PrevodZoStringu(tbDlzka1.Text);
                
                parametre[3] = PrevodZoStringu(tbVyska2.Text);
                parametre[4] = PrevodZoStringu(tbSirka2.Text);
                parametre[5] = PrevodZoStringu(tbDlzka2.Text);

                parametre[6] = PrevodZoStringu(tbPrierez1.Text) / 100;
                parametre[7] = PrevodZoStringu(tbPrierez2.Text) / 100;

                parametre[8] = PrevodZoStringu(tbPritok01.Text) / jPrietok;

                parametre[9] = PrevodZoStringu(tbHladina1.Text);
                parametre[10] = PrevodZoStringu(tbHladina2.Text);

                parametre[11] = parametre[3] / 2;

                DialogResult = DialogResult.OK;
                Close();
            }
            else
                MessageBox.Show("Niektorý parameter je nesprávne zadaný!\nJedna z možných príčin je nedodržanie zadaného intervalu.\nSkontrolujte prázdne textové polia.", "Informácia!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Tlacidlo - Zavretie okna
        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        //Nastavenie auto-kliku za pomoci kláves
        private void Setting_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                bOk_Click(sender, e);
            if (e.KeyChar == (char)Keys.Escape)
                bCancel_Click(sender, e);
        }

        //Zmena oznacienia radioTlacidla
        private void rbChange_CheckedChanged(object sender, EventArgs e)
        {
            if (rbM3.Checked == true)
            {
                labelX.Text = "Prítok:\n(m^3/s)";
                jPrietok = 1;
                tbPritok01.Text = (Convert.ToDouble(tbPritok01.Text) / 1000).ToString();
            }
            if (rbLitre.Checked == true)
            {
                labelX.Text = "Prítok:\n(l/s)";
                jPrietok = 1000;                
                tbPritok01.Text = (Convert.ToDouble(tbPritok01.Text) * 1000).ToString();
            }
        }
        #endregion

        #region Focus
        private void tbVyska1_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte výšku modrej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ1_VYSKA_MIN.ToString("# ##0") + " po " + NADRZ1_VYSKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbDlzka1_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte dĺžku modrej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ1_DLZKA_MIN.ToString("# ##0") + " po " + NADRZ1_DLZKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbSirka1_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte šírku modrej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ1_SIRKA_MIN.ToString("# ##0") + " po " + NADRZ1_SIRKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbVyska2_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte výšku zelenej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ2_VYSKA_MIN.ToString("# ##0") + " po " + NADRZ2_VYSKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbDlzka2_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte dĺžku zelenej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ2_DLZKA_MIN.ToString("# ##0") + " po " + NADRZ2_DLZKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbSirka2_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte šírku zelenej nádrže";
            lAbleChar.Text = "V intervale od " + NADRZ2_SIRKA_MIN.ToString("# ##0") + " po " + NADRZ2_SIRKA_MAX.ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbHladina1_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte počiatočnú hladinu v modrej nádrži.";
            lAbleChar.Text = "V intervale od " + 0.ToString("# ##0.##") + " po " + PrevodZoStringu(tbVyska1.Text).ToString("# ##0.##");
            OznacText(sender as TextBox);
        }

        private void tbHladina2_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte počiatočnú hladinu v zelenej nádrži.";
            lAbleChar.Text = "V intervale od " + 0.ToString("# ##0.##") + " po " + PrevodZoStringu(tbVyska2.Text).ToString("# ##0.##");
            OznacText(sender as TextBox);
        }

        private void tbPrierez1_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte priemer potrubia. Potrubie je spojené z modrej nádrže do zelenej nádrže";
            if (PRIEMER12_MAX < Math.Min(PrevodZoStringu(tbSirka1.Text), PrevodZoStringu(tbDlzka1.Text)))
                lAbleChar.Text = "V intervale od " + (PRIEMER12_MIN * 100).ToString("#0") + " po " + (PRIEMER12_MAX * 100).ToString("# ##0");
            else
                lAbleChar.Text = "V intervale od " + (PRIEMER12_MIN * 100).ToString("#0") + " po " + (Math.Min(PrevodZoStringu(tbSirka1.Text), PrevodZoStringu(tbDlzka1.Text)) * 100).ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbPrierez2_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte priemer odtokového potrubia zo zelenej nádrže";
            if (PRIEMER20_MAX < Math.Min(PrevodZoStringu(tbSirka2.Text), PrevodZoStringu(tbDlzka2.Text)))
                lAbleChar.Text = "V intervale od " + (PRIEMER20_MIN * 100).ToString("#0") + " po " + (PRIEMER20_MAX * 100).ToString("# ##0");
            else
                lAbleChar.Text = "V intervale od " + (PRIEMER12_MIN * 100).ToString("#0") + " po " + (Math.Min(PrevodZoStringu(tbSirka2.Text), PrevodZoStringu(tbDlzka2.Text)) * 100).ToString("# ##0");
            OznacText(sender as TextBox);
        }

        private void tbPotr3_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "Zadajte prítok potrubia do modrej nádrže";
            lAbleChar.Text = "V intervale od " + (PRITOK01_MIN * jPrietok).ToString("#0.####") + " po " + (PRITOK01_MAX * jPrietok).ToString("#0.##");
            OznacText(sender as TextBox);
        }        

        private void rbM3_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "-";
            lAbleChar.Text = "-";
        }

        private void bOk_Enter(object sender, EventArgs e)
        {
            lDescrip.Text = "-";
            lAbleChar.Text = "-";
        }
        #endregion

        #region Ostatne
        private bool KontrolaParametrov()
        {
            bool returning = true;
            if (PrevodZoStringu(tbDlzka1.Text) > NADRZ1_DLZKA_MAX || PrevodZoStringu(tbDlzka1.Text) < NADRZ1_DLZKA_MIN)
            {
                tbDlzka1.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbSirka1.Text) > NADRZ1_SIRKA_MAX || PrevodZoStringu(tbSirka1.Text) < NADRZ1_SIRKA_MIN)
            {
                tbSirka1.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbVyska1.Text) > NADRZ1_VYSKA_MAX || PrevodZoStringu(tbVyska1.Text) < NADRZ1_VYSKA_MIN)
            {
                tbVyska1.Text = string.Empty;
                returning = false;
            }

            if (PrevodZoStringu(tbDlzka2.Text) > NADRZ2_DLZKA_MAX || PrevodZoStringu(tbDlzka2.Text) < NADRZ2_DLZKA_MIN)
            {
                tbDlzka2.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbSirka2.Text) > NADRZ2_SIRKA_MAX || PrevodZoStringu(tbSirka2.Text) < NADRZ2_SIRKA_MIN)
            {
                tbSirka2.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbVyska2.Text) > NADRZ2_VYSKA_MAX || PrevodZoStringu(tbVyska2.Text) < NADRZ2_VYSKA_MIN)
            {
                tbVyska2.Text = string.Empty;
                returning = false;
            }

            if (PrevodZoStringu(tbHladina1.Text) > PrevodZoStringu(tbVyska1.Text) || PrevodZoStringu(tbHladina1.Text) < 0)
            {
                tbHladina1.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbHladina2.Text) > PrevodZoStringu(tbVyska2.Text) || PrevodZoStringu(tbHladina2.Text) < 0)
            {
                tbHladina2.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbPrierez1.Text) / 100 > Math.Min(Math.Min(PrevodZoStringu(tbSirka1.Text), PrevodZoStringu(tbDlzka1.Text)), PRIEMER12_MAX) || PrevodZoStringu(tbPrierez1.Text) / 100 < PRIEMER12_MIN)
            {
                tbPrierez1.Text = string.Empty;
                returning = false;
            }
            if (PrevodZoStringu(tbPrierez2.Text) / 100 > Math.Min(Math.Min(PrevodZoStringu(tbSirka2.Text), PrevodZoStringu(tbDlzka2.Text)), PRIEMER20_MAX) || PrevodZoStringu(tbPrierez2.Text) / 100 < PRIEMER20_MIN)
            {
                tbPrierez2.Text = string.Empty;
                returning = false;
            }

            if (PrevodZoStringu(tbPritok01.Text) / jPrietok > PRITOK01_MAX || PrevodZoStringu(tbPritok01.Text) / jPrietok < PRITOK01_MIN)
            {
                tbPritok01.Text = string.Empty;
                returning = false;
            }
            return returning;
        }

        private double PrevodZoStringu(string s)
        {
            if (s.Length > 0)
            {
                //s = s.Replace(',', '.');
                if (s[s.Length - 1] == '.')
                    s = s.Substring(0, s.Length - 2);
                return Convert.ToDouble(s);
            }
            return -1;
        }

        //Oznaci text v textboxe
        private void OznacText(TextBox tb)
        {
            tb.SelectAll();
            
        }
        #endregion
    }
}
