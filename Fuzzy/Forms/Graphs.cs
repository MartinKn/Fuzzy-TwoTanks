using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

using Fuzzy.Classes.Saving;

namespace Fuzzy.Forms
{
    public partial class Graphs : Form
    {
        private MainForm main;
        private Data data;

        private int indexOfGraph = 0;
        private double maxRangeSelect = 0;

        private int posX;
        private int posY;
        private double zoomX;
        private double zoomY;
        private bool clickDown;
        private bool checking = false;


        private const double INTERVAL_X = 1;
        private const double INTERVAL_Y = 0.1;

        private const double POCET_X = 15;
        private const double POCET_Y = 10;

        public Graphs(MainForm mf, ref Data data)
        {
            InitializeComponent();
            graph.MouseWheel += new MouseEventHandler(graph_MouseWheel);

            main = mf;
            this.data = data;
        }

        #region Eventy
        //Form - Nacitanie
        private void Graphs_Load(object sender, EventArgs e)
        {
            zoomX = 1;
            zoomY = 1;

            graph.Series.Add("Hladina (Modrá nádrž)");
            graph.Series[0].ChartType = SeriesChartType.Line;
            graph.Series[0].Color = Color.FromArgb(0, 150, 255);
            graph.Series.Add("Hladina (Zelená nádrž)");
            graph.Series[1].ChartType = SeriesChartType.Line;
            graph.Series[1].Color = Color.FromArgb(0, 100, 0);
            graph.Series.Add("Prietok");
            graph.Series[2].ChartType = SeriesChartType.Line;
            graph.Series[2].Color = Color.Orange;
            graph.Series.Add("Odtok");
            graph.Series[3].ChartType = SeriesChartType.Line;
            graph.Series[3].Color = Color.Red;

            graph.ChartAreas.Add(new ChartArea("area"));
            graph.ChartAreas[0].AxisX.Title = "Čas (t)";
            graph.ChartAreas[0].AxisY.Title = "Hladina (m)";

            cbVariable.Items.Add("Hladina 1 (m)");
            cbVariable.Items.Add("Hladina 2 (m)");
            cbVariable.Items.Add("Prietok (m^3/s)");
            cbVariable.Items.Add("Odtok (m^3/s)");
            cbVariable.Items.Add("Hladina 1, 2 (m)");
            cbVariable.Items.Add("Prietok, Odtok (m^3/s)");
            indexOfGraph = main.GraphsForms.Count - 1;
            if (indexOfGraph < cbVariable.Items.Count)
                cbVariable.SelectedIndex = indexOfGraph;
            else
                cbVariable.SelectedIndex = 0;

            for (int i = 0; i < graph.Series.Count; i++)
                if (i != cbVariable.SelectedIndex)
                    graph.Series[i].Enabled = false;
                else
                    graph.Series[i].Enabled = true;

            graph.ChartAreas[0].AxisX.Interval = INTERVAL_X;
            graph.ChartAreas[0].AxisX.Maximum = POCET_X * graph.ChartAreas[0].AxisX.Interval;
            graph.ChartAreas[0].AxisX.Minimum = 0;
            graph.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            graph.ChartAreas[0].AxisY.Interval = INTERVAL_Y;
            graph.ChartAreas[0].AxisY.Maximum = POCET_Y * graph.ChartAreas[0].AxisY.Interval;
            graph.ChartAreas[0].AxisY.Minimum = 0;
            graph.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            graph.ChartAreas[0].AxisX.Crossing = 0;
            graph.ChartAreas[0].AxisY.Crossing = 0;

            //graph.ChartAreas[0].AxisX.MajorTickMark.Enabled = true;
            //graph.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
            //graph.ChartAreas[0].AxisX.MajorTickMark.LineDashStyle = ChartDashStyle.Solid;
            //graph.ChartAreas[0].AxisX.MajorTickMark.LineColor = Color.Red;

            //graph.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Solid;

            if (data == null)
            {
                graph.Series[0].Points.AddXY(0, 0);
                graph.Series[1].Points.AddXY(0, 0);
                graph.Series[2].Points.AddXY(0, 0);
                graph.Series[3].Points.AddXY(0, 0);
            }
            else
                PridadVsetkyBody();

            
            this.Text = "Graf " + main.GraphsForms.Count;
            ZmenaSledovania();   
        }

        //Tlacidlo TSB - Vytvori novu formu grafu
        private void tsbNewGraph_Click(object sender, EventArgs e)
        {
            Graphs g = new Graphs(main, ref data);
            main.PridatGrafForm(g);
            g.Show();            
        }

        //Tlacidlo TSB - Zobrazi vsetky grafy
        private void tsbFocus_Click(object sender, EventArgs e)
        {
            main.ZobrazVsetkyGrafForm();
            this.Focus();
        }

        //Tlacidlo TSB - Zoradi grafy podla velkosti
        private void tsbSetSizeAll_Click(object sender, EventArgs e)
        {
            foreach (Graphs g in main.GraphsForms)
            {
                g.Size = new Size(this.Width, this.Height);
                g.Refresh();
            }
            
            switch (main.GraphsForms.Count)
            {
                case 0: break;
                case 1:
                    {
                        main.GraphsForms[0].WindowState = FormWindowState.Normal;                        
                        main.GraphsForms[0].DesktopLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, 0);
                        main.GraphsForms[0].Focus();
                        break;
                    }
                default:
                    {
                        for (int i = 0; i < main.GraphsForms.Count; i++)
                        {
                            main.GraphsForms[i].WindowState = FormWindowState.Normal;
                            if ((main.GraphsForms.Count % 2 == 0 && main.GraphsForms.Count != 2) || main.GraphsForms.Count > 2)
                                if (i % 2 == 0)
                                    main.GraphsForms[i].DesktopLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width, (i / 2 * main.GraphsForms[i].Size.Height));
                                else
                                    main.GraphsForms[i].DesktopLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width - ((i % 2 + 1) * this.Size.Width), (i / 2 * main.GraphsForms[i].Size.Height));
                            else
                                main.GraphsForms[i].DesktopLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width, i * main.GraphsForms[i].Size.Height);
                            main.GraphsForms[i].Focus();
                        }
                        break;
                    }
            }
        }

        //TSB Tlacidlo - Zoradi grafy
        private void tsbSort_Click(object sender, EventArgs e)
        {
            switch (main.GraphsForms.Count)
            {
                case 0: break;
                case 1:
                    {
                        main.GraphsForms[0].WindowState = FormWindowState.Normal;
                        main.GraphsForms[0].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                        main.GraphsForms[0].DesktopLocation = new Point(0, 0);
                        main.GraphsForms[0].Focus();
                        break;
                    }
                default:
                    {
                        for (int i = 0; i < main.GraphsForms.Count; i++)
                        {
                            main.GraphsForms[i].WindowState = FormWindowState.Normal;
                            if ((main.GraphsForms.Count % 2 == 0 && main.GraphsForms.Count != 2) || main.GraphsForms.Count > 4)
                            {
                                main.GraphsForms[i].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / ((main.GraphsForms.Count + 1) / 2));
                                if (i % 2 == 0)
                                    main.GraphsForms[i].DesktopLocation = new Point((i % 2) * this.Size.Width, (i / 2 * main.GraphsForms[i].Size.Height));
                                else
                                    main.GraphsForms[i].DesktopLocation = new Point(Screen.PrimaryScreen.WorkingArea.Width/2, (i / 2 * main.GraphsForms[i].Size.Height));
                            }
                            else
                            {
                                main.GraphsForms[i].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height / main.GraphsForms.Count);
                                main.GraphsForms[i].DesktopLocation = new Point(0, i * main.GraphsForms[i].Size.Height);
                            }
                            main.GraphsForms[i].Focus();
                        }
                        break;
                    }
            }
        }

        //ComboBox - Zmena zobrazenia ciary grafu
        private void cbVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < graph.Series.Count; i++)
                switch (cbVariable.SelectedIndex)
                {
                    case 4: graph.Series[0].Enabled = true; graph.Series[1].Enabled = true; graph.Series[2].Enabled = false; graph.Series[3].Enabled = false; break;
                    case 5: graph.Series[0].Enabled = false; graph.Series[1].Enabled = false; graph.Series[2].Enabled = true; graph.Series[3].Enabled = true; break;
                    default:
                        {
                            if (i != cbVariable.SelectedIndex)
                                graph.Series[i].Enabled = false;
                            else
                                graph.Series[i].Enabled = true;
                            break;
                        }
                }
            ZmenaSledovania();
            graph.Focus();
        }

        //Koleso mysi - Event
        private void graph_MouseWheel(object sender, MouseEventArgs e)
        {
            graph.Series[1].Enabled = true;
            if (e.Delta > 0)
            {
                if (sliderX.Value > -4 && cbLockX.Checked == false)
                    sliderX.Value--;
                if (sliderY.Value > 0 && cbLockY.Checked == false)
                    sliderY.Value--;
            }
            else
            {
                if (sliderX.Value < 10 && cbLockX.Checked == false)
                    sliderX.Value++;
                if (sliderY.Value < 10 && cbLockY.Checked == false)
                    sliderY.Value++;
            }
            zoomX = Math.Pow(2, sliderX.Value);
            zoomY = 1 / (double)(11 - sliderY.Value);
            Zoomovanie();
        }

        //Form - Uzatvara form (vymaze z listu)
        private void Graphs_FormClosing(object sender, FormClosingEventArgs e)
        {
            graph.Series.Clear();
            main.VymazatGrafForm(this);
        }
        #endregion

        #region Graf
        //Aktualizacia - prida posledny bod
        public void PridatBod()
        {
            double actualY = 10;
            graph.Invoke((MethodInvoker)delegate
            {
                graph.Series[0].Points.AddXY(data.Cas[data.Cas.Count - 1], data.Hladiny[0][data.Hladiny[0].Count - 1]);
                graph.Series[1].Points.AddXY(data.Cas[data.Cas.Count - 1], data.Hladiny[1][data.Hladiny[0].Count - 1]);
                graph.Series[2].Points.AddXY(data.Cas[data.Cas.Count - 1], data.Prietoky[0][data.Hladiny[0].Count - 1]);
                graph.Series[3].Points.AddXY(data.Cas[data.Cas.Count - 1], data.Prietoky[1][data.Hladiny[0].Count - 1]);
                
                
                if (Math.Abs(main.Engine.Pozadovana - data.Hladiny[1][data.Hladiny[0].Count - 1]) < 0.01)
                    graph.Series[1].Points[graph.Series[1].Points.Count - 1].Color = Color.FromArgb(0, 100, 0);
                else
                    graph.Series[1].Points[graph.Series[1].Points.Count - 1].Color = Color.FromArgb(0, 255, 0);
                if (Math.Abs((data.Parametre.Nadrz1Vyska / 2) - data.Hladiny[0][data.Hladiny[0].Count - 1]) < 0.01)
                    graph.Series[0].Points[graph.Series[0].Points.Count - 1].Color = Color.FromArgb(0, 0, 255);
                else
                    graph.Series[0].Points[graph.Series[0].Points.Count - 1].Color = Color.FromArgb(0, 150, 255);

                switch (cbVariable.SelectedIndex)
                {
                    case 0: actualY = data.Hladiny[0][data.Hladiny[0].Count - 1]; break;
                    case 1: actualY = data.Hladiny[1][data.Hladiny[0].Count - 1]; break;
                    case 2: actualY = data.Prietoky[0][data.Hladiny[0].Count - 1]; break;
                    case 3: actualY = data.Prietoky[1][data.Hladiny[0].Count - 1]; break;
                    case 4: actualY = Math.Max(data.Hladiny[0][data.Hladiny[0].Count - 1], data.Hladiny[1][data.Hladiny[0].Count - 1]); break;
                    case 5: actualY = Math.Max(data.Prietoky[0][data.Hladiny[0].Count - 1], data.Prietoky[1][data.Hladiny[0].Count - 1]); break;
                }


                if (data.Cas[data.Cas.Count - 1] > graph.ChartAreas[0].AxisX.Maximum - graph.ChartAreas[0].AxisX.Interval || data.Cas[data.Cas.Count - 1] < graph.ChartAreas[0].AxisX.Minimum)
                {
                    graph.ChartAreas[0].AxisX.Maximum = (int)data.Cas[data.Cas.Count - 1] + Math.Round(INTERVAL_X * zoomX, 2);
                    graph.ChartAreas[0].AxisX.Minimum = Math.Round((int)data.Cas[data.Cas.Count - 1] - graph.ChartAreas[0].AxisX.Interval * (POCET_X - 1), 2);
                }
                if (actualY > graph.ChartAreas[0].AxisY.Maximum - graph.ChartAreas[0].AxisY.Interval || actualY < graph.ChartAreas[0].AxisY.Minimum)
                {
                    graph.ChartAreas[0].AxisY.Maximum = Math.Round(actualY,1) + Math.Round(INTERVAL_Y * zoomX, 2);
                    graph.ChartAreas[0].AxisY.Minimum = Math.Round(Math.Round(actualY, 1) - graph.ChartAreas[0].AxisY.Interval * (POCET_Y - 1), 2);
                }
                Zoomovanie();
            });
        }

        //Prida vsetky zname body (nacitanie
        private void PridadVsetkyBody()
        {
            int s = 0;
            foreach (List<double> ld in data.Hladiny)
            {
                for(int i=0;i<data.Cas.Count;i++)
                    graph.Series[s].Points.AddXY(data.Cas[i], ld[i]);
                s++;
            }
            foreach (List<double> ld in data.Prietoky)
            {
                for (int i = 0; i < data.Cas.Count; i++)
                    graph.Series[s].Points.AddXY(data.Cas[i], ld[i]);
                s++;
            }
        }
        #endregion

        #region Zmena zobrazenia Grafu
        //Pustenie tlacidla mysi
        private void graph_MouseUp(object sender, MouseEventArgs e)
        {
            clickDown = false;
            Cursor = Cursors.Default;
        }

        //Stlacenie tlacidla
        private void graph_MouseDown(object sender, MouseEventArgs e)
        {
            posX = MousePosition.X;
            posY = MousePosition.Y;
            clickDown = true;
            Cursor = Cursors.SizeAll;
        }

        //Pohyb
        private void graph_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickDown)
            {
                if (Math.Abs(MousePosition.X - posX) > 5 && cbLockX.Checked == false)
                {
                    graph.ChartAreas[0].AxisX.Maximum = Math.Round(graph.ChartAreas[0].AxisX.Maximum - (MousePosition.X - posX) / Math.Abs(MousePosition.X - posX) * graph.ChartAreas[0].AxisX.Interval, 2);
                    graph.ChartAreas[0].AxisX.Minimum = Math.Round(graph.ChartAreas[0].AxisX.Minimum - (MousePosition.X - posX) / Math.Abs(MousePosition.X - posX) * graph.ChartAreas[0].AxisX.Interval, 2);
                    posX = MousePosition.X;
                }
                if (Math.Abs(MousePosition.Y - posY) > 10 && cbLockY.Checked == false)
                {                    
                    graph.ChartAreas[0].AxisY.Maximum = Math.Round(graph.ChartAreas[0].AxisY.Maximum + (MousePosition.Y - posY) / Math.Abs(MousePosition.Y - posY) * graph.ChartAreas[0].AxisY.Interval, 2);
                    graph.ChartAreas[0].AxisY.Minimum = Math.Round(graph.ChartAreas[0].AxisY.Minimum + (MousePosition.Y - posY) / Math.Abs(MousePosition.Y - posY) * graph.ChartAreas[0].AxisY.Interval, 2);
                    posY = MousePosition.Y;
                }
            }
        }

        //Zoom
        private void Zoomovanie()
        {
            graph.ChartAreas[0].AxisX.Interval = Math.Round(INTERVAL_X * zoomX, 2);
            graph.ChartAreas[0].AxisX.Maximum = Math.Round(graph.ChartAreas[0].AxisX.Minimum + INTERVAL_X * zoomX * POCET_X, 2);

            if (maxRangeSelect != 0)
                if (zoomY >= 1)
                {
                    graph.ChartAreas[0].AxisY.Interval = Math.Round(maxRangeSelect / POCET_Y, 2);
                    graph.ChartAreas[0].AxisY.Maximum = Math.Round(graph.ChartAreas[0].AxisY.Minimum + maxRangeSelect, 2);
                }
                else
                {
                    graph.ChartAreas[0].AxisY.Interval = Math.Round((maxRangeSelect / POCET_Y) / (1 / (zoomY)), 2);
                    graph.ChartAreas[0].AxisY.Maximum = Math.Round(graph.ChartAreas[0].AxisY.Minimum + (maxRangeSelect * zoomY), 2);
                }
            else
            {
                graph.ChartAreas[0].AxisY.Interval = Math.Round(INTERVAL_Y * zoomY, 2);
                graph.ChartAreas[0].AxisY.Maximum = Math.Round(graph.ChartAreas[0].AxisY.Minimum + (graph.ChartAreas[0].AxisY.Interval * POCET_Y), 2);
            }

            for (int i = 0; i < graph.Series.Count; i++)
                if (i != cbVariable.SelectedIndex && cbVariable.SelectedIndex < 4)
                    graph.Series[i].Enabled = false;

            if (cbVariable.SelectedIndex == 4)
            {
                graph.Series[0].Enabled = true;
                graph.Series[1].Enabled = true;
                graph.Series[2].Enabled = false;
                graph.Series[3].Enabled = false;
            }
            if (cbVariable.SelectedIndex == 5)
            {
                graph.Series[0].Enabled = false;
                graph.Series[1].Enabled = false;
                graph.Series[2].Enabled = true;
                graph.Series[3].Enabled = true;
            }
        }
        #endregion

        #region Ostatne
        //Nastavi Data #main
        public void NastavData(ref Data data)
        {
            this.data = data;
            PridadVsetkyBody();
            ZmenaSledovania();
        }

        //Zmena sledovanej veliciny
        private void ZmenaSledovania()
        {
            switch (cbVariable.SelectedIndex)
            {
                case 0: if (data != null) maxRangeSelect = data.Parametre.Nadrz1Vyska; graph.ChartAreas[0].AxisY.Title = "Hladina (m)"; break;
                case 1: if (data != null) maxRangeSelect = data.Parametre.Nadrz2Vyska; graph.ChartAreas[0].AxisY.Title = "Hladina (m)"; break;
                case 2: if (data != null) maxRangeSelect = Math.PI * Math.Pow(data.Parametre.Rura12Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Parametre.Nadrz1Vyska); graph.ChartAreas[0].AxisY.Title = "Prietok (m^3/s)"; break;
                case 3: if (data != null) maxRangeSelect = Math.PI * Math.Pow(data.Parametre.Rura20Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Parametre.Nadrz2Vyska); graph.ChartAreas[0].AxisY.Title = "Prietok (m^3/s)"; break;
                case 4: if (data != null) maxRangeSelect = Math.Max(data.Parametre.Nadrz1Vyska, data.Parametre.Nadrz2Vyska); graph.ChartAreas[0].AxisY.Title = "Hladina (m)"; break;
                case 5: if (data != null) maxRangeSelect = Math.Max(Math.PI * Math.Pow(data.Parametre.Rura12Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Parametre.Nadrz1Vyska), Math.PI * Math.Pow(data.Parametre.Rura20Priemer / 2, 2) * Math.Sqrt(2 * 9.8065 * data.Parametre.Nadrz2Vyska)); graph.ChartAreas[0].AxisY.Title = "Prietok (m^3/s)"; break;
                default: maxRangeSelect = 0; graph.ChartAreas[0].AxisY.Title = "Y"; break;
            }

            
            Zoomovanie();
        }

        //Vynulovanie grafu #main
        public void Reset()
        {
            for (int i = 0; i < graph.Series.Count; i++)
                graph.Series[i].Points.Clear();

            graph.Series[0].Points.AddXY(0, 0);
            graph.Series[1].Points.AddXY(0, 0);
            graph.Series[2].Points.AddXY(0, 0);
            graph.Series[3].Points.AddXY(0, 0); 
            
            graph.ChartAreas[0].AxisX.Interval = INTERVAL_X;
            graph.ChartAreas[0].AxisX.Maximum = POCET_X * graph.ChartAreas[0].AxisX.Interval;
            graph.ChartAreas[0].AxisX.Minimum = 0;
            graph.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            graph.ChartAreas[0].AxisY.Interval = INTERVAL_Y;
            graph.ChartAreas[0].AxisY.Maximum = POCET_Y * graph.ChartAreas[0].AxisY.Interval;
            graph.ChartAreas[0].AxisY.Minimum = 0;
            graph.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

            graph.ChartAreas[0].AxisX.Crossing = 0;
            graph.ChartAreas[0].AxisY.Crossing = 0;
        }
        #endregion       

        private void sliderX_Scroll(object sender, EventArgs e)
        {
            graph.Focus();
            zoomX = Math.Pow(2, sliderX.Value);
            Zoomovanie();
        }

        private void sliderY_Scroll(object sender, EventArgs e)
        {
            graph.Focus();
            zoomY = 1 / (double)(11 - sliderY.Value);
            Zoomovanie();
        }

        private void cbLockX_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cbLockY.Checked == true && !checking)
            {
                checking = true;
                cbLockY.Checked = false;
                checking = false;
            }
            graph.Focus();
        }

        private void cbLockY_CheckedChanged(object sender, EventArgs e)
        {
            
            if (cbLockX.Checked == true && !checking)
            {
                checking = true;
                cbLockX.Checked = false;
                checking = false;
            }            
            graph.Focus();
        }
    }
}
