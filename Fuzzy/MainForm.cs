using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Diagnostics;
using System.IO;

using Fuzzy.Forms;
using Fuzzy.Classes;
using Fuzzy.Classes.Systems;
using Fuzzy.Classes.Saving;

using FuzzyLogic.Functions;
using FuzzyLogic.Functions.MF;
using FuzzyLogic.Controller;

namespace Fuzzy
{
    public partial class MainForm : Form
    {
        private Details detailsForm = null;
        private List<Graphs> graphsForms;

        private Engine engine = null;        
        private Parametre parametre = null;
        private Data data;
        private Graphics novyObraz;

        private Thread kreslenie;
        private ManualResetEvent vykresluj;
        private int akC1;
        private int akC2;
        private int max1;
        private int max2;

        private bool runButton;

        private bool pause = false;
        private double pauseTime = 0;

        private string systemPath = string.Empty;
        private string regulPath = string.Empty;
        
        private bool manual = false;


        private List<Label> vsetkyLabel;

        #region Konstanty
        private const int NADRZ1_X_MIN = 200;
        private const int NADRZ1_X_MAX = 700;
        private const int NADRZ2_X_MIN = 200;
        private const int NADRZ2_X_MAX = 700;

        private const int HLADINA1_MAX = 100;
        private const int HLADINA1_MIN = 300;

        private const int HLADINA2_MAX = 400;
        private const int HLADINA2_MIN = 600;

        private const int PRITOK_X = 268;
        private const int PRITOK_Y = 60;

        private const int VENTIL1_X = 420;
        private const int VENTIL1_Y = 301;

        private const int VENTIL2_X = 420;
        private const int VENTIL2_Y = 601;

        private const int PRIETOK_X = 420;
        private const int PRIETOK_Y = 308;

        private const int ODTOK_X = 420;
        private const int ODTOK_Y = 608;
        #endregion

        #region Get
        public List<Graphs> GraphsForms
        {
            get { return graphsForms; }
        }

        public Engine Engine
        {
            get { return engine; }
        }

        public Parametre Parametre
        {
            get { return parametre; }
        }

        public string SystemPath
        {
            get { return systemPath; }
        }

        public string RegulPath
        {
            get { return regulPath; }
        }

        public bool Manual
        {
            get { return manual; }
        }

        public double PauseTime
        {
            get { return pauseTime; }
            set { pauseTime = value; }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        #region Eventy
        //Form - Nacitanie Formy
        private void Nadrze_Load(object sender, EventArgs e)
        {
            engine = new Engine(this);

            spustiťMenuItem.Enabled = false;
            gbSetings.Enabled = false;
            tsbRunPause.Enabled = false;
            tsbStop.Enabled = false;
            zastaviťMenuItem1.Enabled = false;
            saveSysMenuItem.Enabled = false;

            runButton = false;
            pause = false;
            cbManual.Checked = false;

            cbManual.Checked = false;
            manual = false;
            slidVentil1.Enabled = manual;
            slidVentil2.Enabled = manual;

            graphsForms = new List<Graphs>();

            obraz.SizeMode = PictureBoxSizeMode.StretchImage;
            obraz.BackgroundImageLayout = ImageLayout.Stretch;
            obraz.Image = new Bitmap(900, 700);
            novyObraz = Graphics.FromImage(obraz.Image);
            novyObraz.Clear(Color.Transparent);
            obraz.Refresh();

            vykresluj = new ManualResetEvent(false);
            kreslenie = new Thread(new ThreadStart(Vykreslovanie));
            kreslenie.Start();
            akC1 = 0;
            akC2 = 0;
            max1 = 0;
            max2 = 0;

            vsetkyLabel = new List<Label>();
            vsetkyLabel.Add(lOdtok12);
            vsetkyLabel.Add(lOdtok20);
            vsetkyLabel.Add(lHladina1);
            vsetkyLabel.Add(lHladina2);
            vsetkyLabel.Add(lPozadovana);
            lStatus.Text = "Načítané";
            lStatusReg.Text = "Prednastavený";
        }

        #region Tlacidla TSB
        //Tlacidlo TSB - Nastavenie Parametrov (nových) #Form
        private void tsbSet_Click(object sender, EventArgs e)
        {
            parametre = null;

            Setting set = new Setting(this);
            if (set.ShowDialog() == DialogResult.OK)
            {
                parametre = new Parametre(set.Parametre);
                if (data == null)
                {
                    data = new Data(parametre);
                    data.InitialtData();
                }
                PociatocneNast();                
                systemPath = string.Empty;
                lStatus.Text = "Nastavené parametre simulácie";

                vykresluj.Set();
                Thread.Sleep(1);
                vykresluj.Reset();
            }
            set = null;
        }

        //Tlacidlo TSB - Spusta/Pauzuje simulaciu
        private void tsbRunPause_Click(object sender, EventArgs e)
        {
            if (!runButton)
            {
                engine.Spustene.Set();
                lStatus.Text = "Simulácia spustená";
                tsbRunPause.Image = Image.FromFile(@"Icons\pause.png");
                tsbRunPause.Text = "Pozastaviť";
                spustiťMenuItem.Text = "Pozastaviť";
                runButton = true;

                loadRegMenuItem.Enabled = false;
                saveSysMenuItem.Enabled = false;

                vykresluj.Set();
            }
            else
            {
                engine.Spustene.Reset();
                lStatus.Text = "Simulácia pozastavená";
                tsbRunPause.Image = Image.FromFile(@"Icons\play.png");
                tsbRunPause.Text = "Spustiť";
                spustiťMenuItem.Text = "Spustiť";
                runButton = false;

                loadRegMenuItem.Enabled = true;
                saveSysMenuItem.Enabled = true;

                vykresluj.Reset();
            }
        }

        //Tlacidlo TSB - Zastavi simulaciu, zrusi parametre
        private void tsbStop_Click(object sender, EventArgs e)
        {
            tsbRunPause.Image = Image.FromFile(@"Icons\play.png");
            spustiťMenuItem.Text = "Spustiť";
            tsbRunPause.Text = "Spustiť";

            tsbSet.Enabled = true;
            nastaviťMenuItem1.Enabled = true;
            loadSysMenuItem.Enabled = true;
            loadRegMenuItem.Enabled = true;

            gbSetings.Enabled = false;
            spustiťMenuItem.Enabled = false;
            zastaviťMenuItem1.Enabled = false;
            saveSysMenuItem.Enabled = false;
            tsbRunPause.Enabled = false;
            tsbStop.Enabled = false;

            runButton = false;
            pause = false;

            systemPath = string.Empty;

            cbManual.Checked = false;

            parametre = null;
            data = null;

            if (detailsForm != null)
                detailsForm.AktualizaciaZero();
            foreach (Graphs g in graphsForms)
                g.Reset();
            foreach (Label l in vsetkyLabel)
            {
                l.Text = "0,00";
            }
            lTime.Text = "00:00.00";
            lStatus.Text = "Simulácia ukončená";
            lStatusMs.Text = "G " + 0 + ":" + 0 + " ms";
            lStatusMs2.Text = "G " + 0 + ":" + 0 + " ms";

            akC1 = 0;
            akC2 = 0;
            max1 = 0;
            max2 = 0;
            vykresluj.Reset();
            Graphics novyObraz = Graphics.FromImage(obraz.Image);
            novyObraz.Clear(Color.Transparent);
            obraz.Refresh();

            engine.Reset();
            if (regulPath != string.Empty)
                engine.FuzzyReg = FuzzyLogic.SaveLoad.Load.ControllerLoad(regulPath);
        }

        //Tlacidlo TSB - Uklada data simulacie
        private void uložiťMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Data simulacie (.dat)|*.dat";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SaveLoad.SaveData(sfd.FileName, data);
                    lStatus.Text = "Úspešné uloženie dát simulácie";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba pri ukladaní dát simulácie!\n" + ex.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lStatus.Text = "Chyba v ukadaní dát";
            }
        }

        //Tlacidlo TSB - Nacitanie dat simulacie
        private void načítaťMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Filter = "Dáta simulácie (.dat)|*dat";
                if (ofd.ShowDialog() == DialogResult.OK)
                    SaveLoad.LoadData(systemPath = ofd.FileName, this);
            }
            catch (Exception ex)
            {
                data = null;
                if (detailsForm != null)
                    detailsForm.AktualizaciaZero();
                lStatus.Text = "Chyba v načítaní dát";
                MessageBox.Show("Nastala chyba pri načítavaní dát!\n" + ex.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        //Tlacidlo TSB - Nacitanie regulatora
        private void loadRegMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Filter = "Fuzzy regulátor (.fcl)|*fcl";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    engine.FuzzyReg = FuzzyLogic.SaveLoad.Load.ControllerLoad(ofd.FileName);
                    regulPath = ofd.FileName;
                    lStatusReg.Text = ofd.SafeFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nastala chyba pri načítavaní regulátora!\nSpráva chyby:\n" + ex.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lStatusReg.Text = "Chyba v načítaní regulátora";
            }
        }

        //Tlacidlo TSB - Zoradi vsetky okna na obrazovku
        private void tsbSort_Click(object sender, EventArgs e)
        {
            bool nullGraphs = false;
            int x = this.Size.Width;
            int y = this.Size.Height;
            int locX = 0;
            int locY = 0;
            if (this.WindowState == FormWindowState.Maximized)
                x = Screen.PrimaryScreen.WorkingArea.Width;
            this.WindowState = FormWindowState.Normal;

            if (detailsForm != null)
                if (graphsForms.Count < 1)
                {
                    locX = this.Location.X;
                    locY = this.Location.Y;
                    if (x + detailsForm.Width >= Screen.PrimaryScreen.WorkingArea.Width)
                        x = Screen.PrimaryScreen.WorkingArea.Width - detailsForm.Width;
                    if (locX + x + detailsForm.Width > Screen.PrimaryScreen.WorkingArea.Width)
                        locX = Screen.PrimaryScreen.WorkingArea.Width - x - detailsForm.Width;
                    if (locY + y > Screen.PrimaryScreen.WorkingArea.Height)
                        locY = Screen.PrimaryScreen.WorkingArea.Height - y;
                }
                else
                    if (this.Size.Height + detailsForm.Height > Screen.PrimaryScreen.WorkingArea.Height)
                        y = Screen.PrimaryScreen.WorkingArea.Height - detailsForm.Height;

            this.Size = new Size(x, y);
            
            this.DesktopLocation = new Point(locX, locY);

            switch (graphsForms.Count)
            {
                case 0: nullGraphs = true; break;
                case 1:
                    {
                        graphsForms[0].WindowState = FormWindowState.Normal;
                        graphsForms[0].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width, Screen.PrimaryScreen.WorkingArea.Height);
                        graphsForms[0].DesktopLocation = new Point(this.Size.Width, 0);
                        graphsForms[0].Focus();
                        break;
                    }
                default:
                    {
                        for (int i = 0; i < graphsForms.Count; i++)
                        {
                            graphsForms[i].WindowState = FormWindowState.Normal;
                            if ((graphsForms.Count % 2 == 0 && graphsForms.Count != 2) || graphsForms.Count > 6)
                            {
                                graphsForms[i].Size = new Size((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height / ((graphsForms.Count + 1) / 2));
                                if (i % 2 == 0)
                                    graphsForms[i].DesktopLocation = new Point(this.Size.Width, (i / 2 * graphsForms[i].Size.Height));
                                else
                                    graphsForms[i].DesktopLocation = new Point(this.Size.Width + graphsForms[0].Size.Width, (i / 2 * graphsForms[i].Size.Height));
                            }
                            else
                            {
                                graphsForms[i].Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width, Screen.PrimaryScreen.WorkingArea.Height / graphsForms.Count);
                                graphsForms[i].DesktopLocation = new Point(this.Size.Width, i * graphsForms[i].Size.Height);
                            }
                            graphsForms[i].Focus();
                        }
                        break;
                    }
            }

            if (detailsForm != null)
            {
                detailsForm.WindowState = FormWindowState.Normal;
                if (nullGraphs == true)
                    detailsForm.DesktopLocation = new Point((this.Location.X + this.Size.Width + 5) < Screen.PrimaryScreen.WorkingArea.Width ? this.Location.X + this.Size.Width + 5 : Screen.PrimaryScreen.WorkingArea.Height - 10, this.Location.Y + 5);
                else
                    detailsForm.DesktopLocation = new Point(5, (this.Size.Height + 5) < Screen.PrimaryScreen.WorkingArea.Height ? this.Size.Height + 5 : Screen.PrimaryScreen.WorkingArea.Height - 10);
                detailsForm.Focus();
            }

            this.Focus();
        }

        //Tlacidlo TSB - Zobrazenie okna parametrov
        private void parametreMenuItem_Click(object sender, EventArgs e)
        {
            RegDetail regDetForm = new RegDetail(this, engine);
            regDetForm.ShowDialog();
            regDetForm = null;
        }

        //Tlacidlo TSB - Focus okien
        private void tsbFocus_Click(object sender, EventArgs e)
        {
            if (detailsForm != null)
            {
                detailsForm.WindowState = FormWindowState.Normal;
                detailsForm.Focus();
            }
            ZobrazVsetkyGrafForm();
            this.Focus();
        }
        #endregion

        #region Tlacidla
        //Tlacidlo - Otvori/Zobrazi okno grafu/ov
        private void bGraphs_Click(object sender, EventArgs e)
        {
            if (graphsForms.Count < 1)
            {
                graphsForms.Add(new Graphs(this, ref data));
                graphsForms[0].Show();
            }
            else
                ZobrazVsetkyGrafForm();
        }

        //Tlacidlo - Zobrazenie detailov (vsetkych) #form
        private void bDetails_Click(object sender, EventArgs e)
        {
            if (detailsForm == null)
            {
                detailsForm = new Details(this, ref data);
                detailsForm.Show();
            }
            else
                detailsForm.Focus();
        }

        //Tlacidlo - Zmena pozadovanej hodnoty
        private void bChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (data.Parametre.Nadrz2Vyska >= Convert.ToDouble(ntbSetValue.Text) && 0 <= Convert.ToDouble(ntbSetValue.Text))
                    engine.Pozadovana = Convert.ToDouble(ntbSetValue.Text);
                else
                {
                    if (data.Parametre.Nadrz2Vyska < Convert.ToDouble(ntbSetValue.Text))
                        engine.Pozadovana = data.Parametre.Nadrz2Vyska;
                    else
                        engine.Pozadovana = 0;
                    ntbSetValue.Text = engine.Pozadovana.ToString();
                }
                if (Convert.ToInt32(ntbTimePause.Text) > 0)
                {
                    pause = true;
                    pauseTime = Convert.ToInt32(ntbTimePause.Text) + engine.CasSimulacie;
                    if (pauseTime > 0)
                        lPause.Text = "- " + Convert.ToInt32(ntbTimePause.Text) + " s";
                }
                else
                {
                    pause = false;
                    pauseTime = 0;
                    lPause.Text = "";
                }
                ntbTimePause.Text = "0";

                lPozadovana.Text = engine.Pozadovana.ToString("# ##0.00");
            }
            catch (Exception)
            {
                lPozadovana.Text = engine.Pozadovana.ToString("# ##0.00");
                lPause.Text = "0";
            }
        }
        #endregion

        #region Slidery
        //Slider - Nastavenie Rychlosti simulacie
        private void sliderTick_Scroll(object sender, EventArgs e)
        {
            //engine.Tick = sliderTick.Value * 250;     Min 1 Max 7 def 4
            //lTick.Text = Math.Abs(2 - ((double)engine.Tick / 1000)).ToString("#0");

            engine.Tick = 1000 / sliderTick.Value;
            lTick.Text = sliderTick.Value.ToString();
        }

        //Slider - Zmena frekvencie
        private void sliderTickCount_Scroll(object sender, EventArgs e)
        {
            if (engine != null)
                engine.TickCount = sliderTickCount.Value;
            lTickCount.Text = sliderTickCount.Value.ToString();
        }

        //Slider - Manualne ovladanie ventilu 1
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (cbManual.Checked)
                engine.nastavVentil(slidVentil1.Value, 1);
        }

        //Slider - Manualne ovladanie ventilu 2
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if (cbManual.Checked)
                engine.nastavVentil(slidVentil2.Value, 2);
        }
        #endregion

        //Form - Ukoncenie Aplikacie (menu tool strip)
        private void koniecMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Form - Ukonci thready pri uzatvarani
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (engine != null)
                engine.UkoncitEngine();
            vykresluj.Set();
            kreslenie.Abort();
        }

        //Klaves - Zmena pozadovanej hodnoty
        private void ntbSetValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                bChange.PerformClick();
        }
        #endregion

        #region Systems
        //Akutalizuje premenne kazdy cyklus #engine
        public void AktualizovatPremenne()
        {
            gbVariable.Invoke((MethodInvoker)delegate
            {
                if (data != null)
                {
                    lHladina1.Text = data.Hladiny[0][data.Hladiny[0].Count - 1].ToString("#0.00");
                    lHladina2.Text = data.Hladiny[1][data.Hladiny[1].Count - 1].ToString("#0.00");

                    //lPritok21.Text = prietok[2].ToString("#0.00");
                    lOdtok20.Text = data.Prietoky[1][data.Prietoky[1].Count - 1].ToString("#0.00");

                    lOdtok12.Text = data.Prietoky[0][data.Prietoky[0].Count - 1].ToString("#0.00");
                    //lOdtok21.Text = prietok[2].ToString("#0.00");

                    lTime.Text = ((engine.CasSimulacie / 60 / 60) % 60).ToString("00") + ":" + ((engine.CasSimulacie / 60) % 60).ToString("00") + "." + (engine.CasSimulacie % 60).ToString("00");
                }
            });

            for (int i = 0; i < graphsForms.Count; i++)
                graphsForms[i].PridatBod();

            if (detailsForm != null)
                detailsForm.Aktualizacia();
            //int pocetForm = graphsForms.Count;
            //for (int i = 0; i < pocetForm - 1; i++)
            //    graphsForms[i].PridatBod(engine.CasSimulacie);
        }

        public void AktualizaciaTrvanie(long trvanie)
        {
            if (++akC1 % (int)(9 + engine.TickCount) == 0)
            {
                akC1 = 0;
                toolStrip1.Invoke((MethodInvoker)delegate
                {
                    lStatusMs.Text = "S " + max1 + ":" + (engine.Tick / engine.TickCount) + " ms";
                });
                max1 = 0;
            }
            else
                max1 = Math.Max(max1, (int)trvanie);
        }

        //Aktualizuje premenne pri zmene pritoku #engine
        public void AktualizovatPremenne(double[] prietok)
        {
            gbVariable.Invoke((MethodInvoker)delegate
            {
                lOdtok12.Text = prietok[1].ToString("#0.00");

                lOdtok20.Text = prietok[2].ToString("#0.00");
            });
            if (detailsForm != null)
                detailsForm.Aktualizacia();
        }

        //Vykresluje do picture boxu
        public void Vykreslovanie()
        {
            try
            {
                Stopwatch stopky = new Stopwatch();
                while (true)
                {
                    vykresluj.WaitOne();
                    stopky.Reset();
                    stopky.Start();

                    try
                    {
                        Graphics novyObraz = Graphics.FromImage(obraz.Image);
                        novyObraz.Clear(Color.Transparent);
                        novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), NADRZ1_X_MIN, HLADINA1_MIN - PixelPrepocet(data.Hladiny[0][data.Hladiny[0].Count - 1], true), NADRZ1_X_MAX - NADRZ1_X_MIN, PixelPrepocet(data.Hladiny[0][data.Hladiny[0].Count - 1], true) + 1);
                        novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), NADRZ2_X_MIN, HLADINA2_MIN - PixelPrepocet(data.Hladiny[1][data.Hladiny[1].Count - 1], false), NADRZ2_X_MAX - NADRZ2_X_MIN, PixelPrepocet(data.Hladiny[1][data.Hladiny[1].Count - 1], false) + 1);

                        novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), PRITOK_X, PRITOK_Y, 20, HLADINA1_MIN - PRITOK_Y);

                        novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), VENTIL1_X + 10 - (int)(20 * data.OtvorenieVentilu[0]) / 2, VENTIL1_Y, (int)(data.OtvorenieVentilu[0] * 10000 > 1 ? 1 + 20 * data.OtvorenieVentilu[0] : 20 * data.OtvorenieVentilu[0]), 7);
                        novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), VENTIL2_X + 10 - (int)(20 * data.OtvorenieVentilu[1]) / 2, VENTIL2_Y, (int)(data.OtvorenieVentilu[1] * 10000 > 1 ? 1 + 20 * data.OtvorenieVentilu[1] : 20 * data.OtvorenieVentilu[1]), 7);

                        if (data.Prietoky[0][data.Prietoky[0].Count - 1] > 0.00)
                            novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), PRIETOK_X + 10 - (int)(20 * data.OtvorenieVentilu[0]) / 2, PRIETOK_Y, (int)(data.OtvorenieVentilu[0] * 10000 > 1 ? 1 + 20 * data.OtvorenieVentilu[0] : 20 * data.OtvorenieVentilu[0]), HLADINA2_MIN - PRIETOK_Y);
                        if (data.Prietoky[1][data.Prietoky[1].Count - 1] > 0.00)
                            novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), ODTOK_X + 10 - (int)(20 * data.OtvorenieVentilu[1]) / 2, ODTOK_Y, (int)(data.OtvorenieVentilu[1] * 10000 > 1 ? 1 + 20 * data.OtvorenieVentilu[1] : 20 * data.OtvorenieVentilu[1]), 200);

                        obraz.Invoke((MethodInvoker)delegate
                        {
                            obraz.Refresh();
                        });
                    }
                    catch (InvalidOperationException)
                    { }

                    stopky.Stop();
                    if (40 - stopky.ElapsedMilliseconds > 0)
                        Thread.Sleep((int)(40 - stopky.ElapsedMilliseconds));
                    if (++akC2 % 5 == 0)
                    {
                        akC2 = 0;
                        toolStrip1.Invoke((MethodInvoker)delegate
                        {
                            lStatusMs2.Text = "G " + max2 + ":" + 40 + " ms";
                        });
                        max2 = 0;
                    }
                    else
                        max2 = Math.Max(max2, (int)stopky.ElapsedMilliseconds);
                }
            }
            catch (ThreadAbortException)
            { }
            catch (Exception ex)
            {
                VypisChybu(ex);
            }
        }
        #endregion

        #region Ostatne
        //Zmena nastaveni pri nacitani
        private void PociatocneNast()
        {
            if (detailsForm != null)
            {
                detailsForm.NastavData(ref data);
                detailsForm.Aktualizacia();
            }
            foreach (Graphs g in graphsForms)
                g.NastavData(ref data);

            engine.Inicializovat(ref data);
            engine.Tick = 1000 / sliderTick.Value;
            engine.TickCount = sliderTickCount.Value;
            engine.CasSimulacie = data.Cas[data.Cas.Count - 1];
            engine.Pozadovana = data.Parametre.Pozadovana;

            ntbSetValue.Text = engine.Pozadovana.ToString();

            spustiťMenuItem.Enabled = true;
            gbSetings.Enabled = true;
            slidVentil1.Enabled = manual;
            slidVentil2.Enabled = manual;

            tsbRunPause.Enabled = true;
            tsbStop.Enabled = true;
            zastaviťMenuItem1.Enabled = true;
            saveSysMenuItem.Enabled = true;

            loadSysMenuItem.Enabled = false;
            nastaviťMenuItem1.Enabled = false;
            tsbSet.Enabled = false;

            Graphics novyObraz = Graphics.FromImage(obraz.Image);
            novyObraz.Clear(Color.Transparent);
            novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), NADRZ1_X_MIN, HLADINA1_MIN - PixelPrepocet(data.Hladiny[0][data.Hladiny[0].Count - 1], true), NADRZ1_X_MAX - NADRZ1_X_MIN, PixelPrepocet(data.Hladiny[0][data.Hladiny[0].Count - 1], true) + 1);
            novyObraz.FillRectangle(new SolidBrush(Color.DeepSkyBlue), NADRZ2_X_MIN, HLADINA2_MIN - PixelPrepocet(data.Hladiny[1][data.Hladiny[1].Count - 1], false), NADRZ2_X_MAX - NADRZ2_X_MIN, PixelPrepocet(data.Hladiny[1][data.Hladiny[1].Count - 1], false) + 1);
            obraz.Refresh();

            AktualizovatPremenne();
            lPozadovana.Text = engine.Pozadovana.ToString("# ##0.00");
        }

        //Vytvorenie dát (nacitanie zo suboru)
        public void CreateDataFromFile(Parametre parametre, List<double> cas, List<List<List<double>>> hodnoty, double[] otvory)
        {
            this.parametre = parametre;
            data = new Data(parametre, cas, hodnoty, otvory);

            PociatocneNast();

            vykresluj.Set();
            Thread.Sleep(1);
            vykresluj.Reset();

            lStatus.Text = "Načítané parametre simulácie";
            lStatusReg.Text = regulPath != string.Empty ? Path.GetFileName(regulPath) : "Prednastavený";
        }

        //Nastavenie detailsForm na null #form Details 
        public void ClosingOtherForm(Form form)
        {
            if (form.GetType() == typeof(Details))
                detailsForm = null;
        }

        //Prepocet na pixelove hodnoty
        public int PixelPrepocet(double aktualVyska, bool modraNadrz)
        {
            if (modraNadrz == true)
                return Convert.ToInt32((aktualVyska * (HLADINA1_MIN - HLADINA1_MAX)) / parametre.Nadrz1Vyska);
            else
                return Convert.ToInt32(((aktualVyska * (HLADINA2_MIN - HLADINA2_MAX)) / parametre.Nadrz2Vyska));
        }

        //Vypisuje chybu
        public void VypisChybu(Exception exception)
        {
            if (exception.Message.Contains("hladiny") == true)
            {
                toolStrip1.Invoke((MethodInvoker)delegate
                {
                    tsbRunPause.PerformClick();
                });
                if (MessageBox.Show(exception.Message, "Upozornenie!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    toolStrip1.Invoke((MethodInvoker)delegate
                    {
                        tsbRunPause.PerformClick();
                    });
            }
            else
            {
                MessageBox.Show("Nastala chyba pri simulácii!\nSpráva chyby:\n" + exception.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    menuStrip1.Invoke((MethodInvoker)delegate
                    {
                        tsbStop.PerformClick();
                    });
                    engine.FuzzyReg = null;
                    lStatusReg.Text = "Prednastavený";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                lStatus.Text = "Simulácia ukončená s chybou!";
            }
        }

        //Pridava form do listu
        public void PridatGrafForm(Graphs form)
        {
            graphsForms.Add(form);
        }

        //Odobera form z listu
        public void VymazatGrafForm(Graphs form)
        {
            graphsForms.Remove(form);
        }
        
        public void ZobrazVsetkyGrafForm()
        {
            for (int i = 0; i < graphsForms.Count; i++)
            {
                graphsForms[i].WindowState = FormWindowState.Normal;
                graphsForms[i].Focus();
            }
        }

        //Zapina / Vypina manualne ovladanie ventilov
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            manual = cbManual.Checked;
            slidVentil1.Enabled = manual;
            slidVentil2.Enabled = manual;
            if (manual == true && data != null)
            {
                slidVentil1.Value = Convert.ToInt32(data.OtvorenieVentilu[0] * 100);
                slidVentil2.Value = Convert.ToInt32(data.OtvorenieVentilu[1] * 100);
            }
        }

        //Aktualny odpocet pauzy
        public void PauseAktual()
        {
            lPause.Invoke((MethodInvoker)delegate
            {
                if (pause == true)
                    if (pauseTime <= engine.CasSimulacie)
                    {
                        toolStrip1.Invoke((MethodInvoker)delegate
                        {
                            tsbRunPause.PerformClick();
                            pause = false;
                        });
                        lPause.Text = "";
                    }
                    else
                    {
                        lPause.Text = "- " + (pauseTime - engine.CasSimulacie).ToString("0") + " s";
                    }
            });
        }
        #endregion
    }
}