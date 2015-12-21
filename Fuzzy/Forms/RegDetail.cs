using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fuzzy.Classes;
using FuzzyLogic.Functions;
using FuzzyLogic.Controller;

namespace Fuzzy.Forms
{
    public partial class RegDetail : Form
    {
        private MainForm main;
        private Engine engine;
        private Sorter sortovac;

        private MembershipFunction actualMf = null;

        private List<double> inputValues;
        private List<double> outputValues;

        public RegDetail(MainForm main, Engine eng)
        {
            InitializeComponent();
            this.main = main;
            engine = eng;
            sortovac = new Sorter();
            lvRule.ListViewItemSorter = sortovac;

            inputValues = new List<double>();
            outputValues = new List<double>();
        }

        private void RegDetail_Load(object sender, EventArgs e)
        {            
            lSysDefine.Text = (main.SystemPath == string.Empty) ? "v aplikácii" : "zo súboru";
            lSysPath.Text = main.SystemPath;
            if (engine.FuzzyReg != null)
            {
                try
                {
                    lRegInp.Text = engine.FuzzyReg.NumberOfInputs.ToString();
                    lRegOut.Text = engine.FuzzyReg.NumberOfOutputs.ToString();

                    lRegDefine.Text = (main.RegulPath == string.Empty) ? "v aplikácii" : "zo súboru";
                    lRegPath.Text = main.RegulPath;


                    foreach (LinguisticVariable lv in engine.FuzzyReg.Inputs)
                        cbIO.Items.Add(lv.Name);
                    foreach (LinguisticVariable lv in engine.FuzzyReg.Outputs)
                        cbIO.Items.Add(lv.Name);
                    if (cbIO.Items.Count > 0)
                        cbIO.SelectedIndex = 0;
                                        
                    NacitatList();
                }
                catch (Exception)
                { }
            }
        }

        private void cbIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMf.Items.Clear();
            if (cbIO.Items.Count > 0)
            {
                if (cbIO.SelectedIndex < engine.FuzzyReg.Inputs.Count)
                {
                    lRange.Text = "[ " + engine.FuzzyReg.Inputs[cbIO.SelectedIndex].RangeMin + " ; " + engine.FuzzyReg.Inputs[cbIO.SelectedIndex].RangeMax + " ]";

                    for(int i=0;i<engine.FuzzyReg.Inputs[cbIO.SelectedIndex].MFCount;i++)
                        cbMf.Items.Add(engine.FuzzyReg.Inputs[cbIO.SelectedIndex][i].Name);
                }
                else
                {
                    lRange.Text = "[ " + engine.FuzzyReg.Outputs[cbIO.SelectedIndex - engine.FuzzyReg.Inputs.Count].RangeMin + " ; " + engine.FuzzyReg.Outputs[cbIO.SelectedIndex - engine.FuzzyReg.Inputs.Count].RangeMax + " ]";

                    for (int i = 0; i < engine.FuzzyReg.Outputs[cbIO.SelectedIndex - engine.FuzzyReg.Inputs.Count].MFCount; i++)
                        cbMf.Items.Add(engine.FuzzyReg.Outputs[cbIO.SelectedIndex - engine.FuzzyReg.Inputs.Count][i].Name);
                }

            }
            cbMf.SelectedIndex = 0;
        }

        private void cbMf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMf.Items.Count > 0 && cbIO.Items.Count > 0)
            {
                if (cbIO.SelectedIndex < engine.FuzzyReg.Inputs.Count)
                    actualMf = engine.FuzzyReg.Inputs[cbIO.SelectedIndex][cbMf.SelectedIndex];
                else
                    actualMf = engine.FuzzyReg.Outputs[cbIO.SelectedIndex - engine.FuzzyReg.Inputs.Count][cbMf.SelectedIndex];
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                VypocetHodnoty();
        }

        private void VypocetHodnoty()
        {
            if (actualMf != null)
                try
                {
                    double d = Convert.ToDouble(tbSetValue.Text);

                    lRegValue.Text = actualMf.GetValue(d).ToString("0.00");
                }
                catch (Exception)
                {
                    tbSetValue.Text = 0.ToString();
                    lRegValue.Text = actualMf.GetValue(0).ToString("0.00");
                }
        }

        private void lvRule_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == sortovac.SortColumn)
            {
                if (sortovac.Order == SortOrder.Ascending)
                {
                    sortovac.Order = SortOrder.Descending;
                }
                else
                {
                    sortovac.Order = SortOrder.Ascending;
                }
            }
            else
            {
                sortovac.SortColumn = e.Column;
                sortovac.Order = SortOrder.Ascending;
            }
            for (int i = 0; i < lvRule.Columns.Count; i++)
            {
                ColumnHelper.SetColumnHeaderSortIcon(lvRule, i, i == sortovac.SortColumn ? sortovac.Order : SortOrder.None);
            }
            this.lvRule.Sort();
        }

        private void bCalcule_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbInput.Text.Trim() != string.Empty)
                {
                    inputValues.Clear();
                    outputValues.Clear();
                    if (engine.FuzzyReg.NumberOfInputs > 0 && engine.FuzzyReg.NumberOfOutputs > 0)
                    {
                        string[] ss = tbInput.Text.Split(' ');
                        for (int i = 0; i < ss.Length; i++)
                            if (ss[i] != string.Empty)
                                inputValues.Add(Convert.ToDouble(ss[i]));
                        outputValues = engine.FuzzyReg.Calculate(inputValues);
                    }
                    rtbOutput.Text = string.Empty;
                    if (outputValues.Count > 0)
                    {
                        rtbOutput.AppendText(engine.FuzzyReg.Outputs[0].Name + ":  " + outputValues[0].ToString("#,#0.######") + "\n");
                        for (int i = 1; i < outputValues.Count; i++)
                            rtbOutput.AppendText(engine.FuzzyReg.Outputs[i].Name + ":  " + outputValues[i].ToString("#,#0.######") + "\n");
                    }
                    else
                        rtbOutput.AppendText("-");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chybne zadané vstupy!\nSpráva chyby:\n"+ex.Message, "Chyba!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                bCalcule.PerformClick();
        }

        private void NacitatList()
        {
            int pRule = 0;

            for (int i = 0; i < engine.FuzzyReg.Inputs.Count - 1; i++)
            {
                lvRule.Columns.Add(engine.FuzzyReg.Inputs[i].Name);
                lvRule.Columns[lvRule.Columns.Count - 1].Width = 65;
                lvRule.Columns.Add("");
                lvRule.Columns[lvRule.Columns.Count - 1].Width = 25;
                lvRule.Columns[lvRule.Columns.Count - 1].TextAlign = HorizontalAlignment.Center;
            }
            lvRule.Columns.Add(engine.FuzzyReg.Inputs[engine.FuzzyReg.Inputs.Count - 1].Name);
            lvRule.Columns[lvRule.Columns.Count - 1].Width = 65;

            lvRule.Columns.Add("=");
            lvRule.Columns[lvRule.Columns.Count - 1].Width = 20;
            lvRule.Columns[lvRule.Columns.Count - 1].TextAlign = HorizontalAlignment.Center;

            for (int i = 0; i < engine.FuzzyReg.Outputs.Count - 1; i++)
            {
                lvRule.Columns.Add(engine.FuzzyReg.Outputs[i].Name);
                lvRule.Columns[lvRule.Columns.Count - 1].Width = 65;
            }
            lvRule.Columns.Add(engine.FuzzyReg.Outputs[engine.FuzzyReg.Outputs.Count - 1].Name);
            lvRule.Columns[lvRule.Columns.Count - 1].Width = 65;

            foreach (FuzzyRule fr in engine.FuzzyReg.Rules)
            {
                lvRule.Items.Add((++pRule).ToString());
                for (int i = 0; i < fr.Inputs.Count - 1; i++)
                {
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add(fr.Inputs[i].Name);
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add(fr.RMethod == RuleMethod.And ? "&" : "|");
                }
                if (fr.Inputs.Count - 1 >= 0)
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add(fr.Inputs[fr.Inputs.Count - 1].Name);
                else
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add("X");
                lvRule.Items[lvRule.Items.Count - 1].SubItems.Add("=");

                for (int i = 0; i < fr.Outputs.Count - 1; i++)
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add(fr.Outputs[i].Name);
                if (fr.Outputs.Count - 1 >= 0)
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add(fr.Outputs[fr.Outputs.Count - 1].Name);
                else
                    lvRule.Items[lvRule.Items.Count - 1].SubItems.Add("X");
            }
        }
    }
}
