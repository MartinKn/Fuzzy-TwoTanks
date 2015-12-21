namespace Fuzzy.Forms
{
    partial class Setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.gbTank1 = new System.Windows.Forms.GroupBox();
            this.tbHladina1 = new Fuzzy.Classes.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSirka1 = new Fuzzy.Classes.NumericTextBox();
            this.tbDlzka1 = new Fuzzy.Classes.NumericTextBox();
            this.tbVyska1 = new Fuzzy.Classes.NumericTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbTank2 = new System.Windows.Forms.GroupBox();
            this.tbHladina2 = new Fuzzy.Classes.NumericTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbSirka2 = new Fuzzy.Classes.NumericTextBox();
            this.tbDlzka2 = new Fuzzy.Classes.NumericTextBox();
            this.tbVyska2 = new Fuzzy.Classes.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lAbleChar = new System.Windows.Forms.Label();
            this.lDescrip = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbPrierez1 = new Fuzzy.Classes.NumericTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelX = new System.Windows.Forms.Label();
            this.tbPritok01 = new Fuzzy.Classes.NumericTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbPrierez2 = new Fuzzy.Classes.NumericTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbM3 = new System.Windows.Forms.RadioButton();
            this.rbLitre = new System.Windows.Forms.RadioButton();
            this.gbTank1.SuspendLayout();
            this.gbTank2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Výška (m):";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(114, 431);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 6;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            this.bOk.Enter += new System.EventHandler(this.bOk_Enter);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(195, 431);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Storno";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            this.bCancel.Enter += new System.EventHandler(this.bOk_Enter);
            // 
            // gbTank1
            // 
            this.gbTank1.Controls.Add(this.tbHladina1);
            this.gbTank1.Controls.Add(this.label8);
            this.gbTank1.Controls.Add(this.tbSirka1);
            this.gbTank1.Controls.Add(this.tbDlzka1);
            this.gbTank1.Controls.Add(this.tbVyska1);
            this.gbTank1.Controls.Add(this.label3);
            this.gbTank1.Controls.Add(this.label2);
            this.gbTank1.Controls.Add(this.label1);
            this.gbTank1.Location = new System.Drawing.Point(12, 12);
            this.gbTank1.Name = "gbTank1";
            this.gbTank1.Size = new System.Drawing.Size(177, 150);
            this.gbTank1.TabIndex = 0;
            this.gbTank1.TabStop = false;
            this.gbTank1.Text = "Nádrž 1 (Modrá)";
            // 
            // tbHladina1
            // 
            this.tbHladina1.AllowSpace = false;
            this.tbHladina1.Location = new System.Drawing.Point(91, 113);
            this.tbHladina1.Name = "tbHladina1";
            this.tbHladina1.Size = new System.Drawing.Size(67, 22);
            this.tbHladina1.TabIndex = 4;
            this.tbHladina1.Text = "0";
            this.tbHladina1.Click += new System.EventHandler(this.tbHladina1_Enter);
            this.tbHladina1.Enter += new System.EventHandler(this.tbHladina1_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 116);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 16);
            this.label8.TabIndex = 10;
            this.label8.Text = "Hladina (m):";
            // 
            // tbSirka1
            // 
            this.tbSirka1.AllowSpace = false;
            this.tbSirka1.Location = new System.Drawing.Point(91, 85);
            this.tbSirka1.Name = "tbSirka1";
            this.tbSirka1.Size = new System.Drawing.Size(67, 22);
            this.tbSirka1.TabIndex = 2;
            this.tbSirka1.Text = "10";
            this.tbSirka1.Click += new System.EventHandler(this.tbSirka1_Enter);
            this.tbSirka1.Enter += new System.EventHandler(this.tbSirka1_Enter);
            // 
            // tbDlzka1
            // 
            this.tbDlzka1.AllowSpace = false;
            this.tbDlzka1.Location = new System.Drawing.Point(91, 57);
            this.tbDlzka1.Name = "tbDlzka1";
            this.tbDlzka1.Size = new System.Drawing.Size(67, 22);
            this.tbDlzka1.TabIndex = 1;
            this.tbDlzka1.Text = "10";
            this.tbDlzka1.Click += new System.EventHandler(this.tbDlzka1_Enter);
            this.tbDlzka1.Enter += new System.EventHandler(this.tbDlzka1_Enter);
            // 
            // tbVyska1
            // 
            this.tbVyska1.AllowSpace = false;
            this.tbVyska1.Location = new System.Drawing.Point(91, 29);
            this.tbVyska1.Name = "tbVyska1";
            this.tbVyska1.Size = new System.Drawing.Size(67, 22);
            this.tbVyska1.TabIndex = 0;
            this.tbVyska1.Text = "10";
            this.tbVyska1.Click += new System.EventHandler(this.tbVyska1_Enter);
            this.tbVyska1.Enter += new System.EventHandler(this.tbVyska1_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Šírka (m):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Dĺžka (m):";
            // 
            // gbTank2
            // 
            this.gbTank2.Controls.Add(this.tbHladina2);
            this.gbTank2.Controls.Add(this.label9);
            this.gbTank2.Controls.Add(this.tbSirka2);
            this.gbTank2.Controls.Add(this.tbDlzka2);
            this.gbTank2.Controls.Add(this.tbVyska2);
            this.gbTank2.Controls.Add(this.label4);
            this.gbTank2.Controls.Add(this.label5);
            this.gbTank2.Controls.Add(this.label6);
            this.gbTank2.Location = new System.Drawing.Point(195, 12);
            this.gbTank2.Name = "gbTank2";
            this.gbTank2.Size = new System.Drawing.Size(177, 150);
            this.gbTank2.TabIndex = 1;
            this.gbTank2.TabStop = false;
            this.gbTank2.Text = "Nádrž 2 (Zelená)";
            // 
            // tbHladina2
            // 
            this.tbHladina2.AllowSpace = false;
            this.tbHladina2.Location = new System.Drawing.Point(91, 113);
            this.tbHladina2.Name = "tbHladina2";
            this.tbHladina2.Size = new System.Drawing.Size(67, 22);
            this.tbHladina2.TabIndex = 4;
            this.tbHladina2.Text = "0";
            this.tbHladina2.Click += new System.EventHandler(this.tbHladina2_Enter);
            this.tbHladina2.Enter += new System.EventHandler(this.tbHladina2_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 116);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 16);
            this.label9.TabIndex = 12;
            this.label9.Text = "Hladina (m):";
            // 
            // tbSirka2
            // 
            this.tbSirka2.AllowSpace = false;
            this.tbSirka2.Location = new System.Drawing.Point(91, 85);
            this.tbSirka2.Name = "tbSirka2";
            this.tbSirka2.Size = new System.Drawing.Size(67, 22);
            this.tbSirka2.TabIndex = 2;
            this.tbSirka2.Text = "10";
            this.tbSirka2.Click += new System.EventHandler(this.tbSirka2_Enter);
            this.tbSirka2.Enter += new System.EventHandler(this.tbSirka2_Enter);
            // 
            // tbDlzka2
            // 
            this.tbDlzka2.AllowSpace = false;
            this.tbDlzka2.Location = new System.Drawing.Point(91, 57);
            this.tbDlzka2.Name = "tbDlzka2";
            this.tbDlzka2.Size = new System.Drawing.Size(67, 22);
            this.tbDlzka2.TabIndex = 1;
            this.tbDlzka2.Text = "10";
            this.tbDlzka2.Click += new System.EventHandler(this.tbDlzka2_Enter);
            this.tbDlzka2.Enter += new System.EventHandler(this.tbDlzka2_Enter);
            // 
            // tbVyska2
            // 
            this.tbVyska2.AllowSpace = false;
            this.tbVyska2.Location = new System.Drawing.Point(91, 30);
            this.tbVyska2.Name = "tbVyska2";
            this.tbVyska2.Size = new System.Drawing.Size(67, 22);
            this.tbVyska2.TabIndex = 0;
            this.tbVyska2.Text = "10";
            this.tbVyska2.Click += new System.EventHandler(this.tbVyska2_Enter);
            this.tbVyska2.Enter += new System.EventHandler(this.tbVyska2_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 88);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Šírka (m):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 60);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Dĺžka (m):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Výška (m):";
            // 
            // lAbleChar
            // 
            this.lAbleChar.AutoSize = true;
            this.lAbleChar.Location = new System.Drawing.Point(135, 386);
            this.lAbleChar.Name = "lAbleChar";
            this.lAbleChar.Size = new System.Drawing.Size(12, 16);
            this.lAbleChar.TabIndex = 11;
            this.lAbleChar.Text = "-";
            // 
            // lDescrip
            // 
            this.lDescrip.Location = new System.Drawing.Point(61, 345);
            this.lDescrip.Name = "lDescrip";
            this.lDescrip.Size = new System.Drawing.Size(300, 32);
            this.lDescrip.TabIndex = 13;
            this.lDescrip.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "Priemer (cm):";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbPrierez1);
            this.groupBox1.Location = new System.Drawing.Point(12, 168);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 68);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nádržové potrubie";
            // 
            // tbPrierez1
            // 
            this.tbPrierez1.AllowSpace = false;
            this.tbPrierez1.Location = new System.Drawing.Point(91, 30);
            this.tbPrierez1.Name = "tbPrierez1";
            this.tbPrierez1.Size = new System.Drawing.Size(67, 22);
            this.tbPrierez1.TabIndex = 0;
            this.tbPrierez1.Text = "30";
            this.tbPrierez1.Click += new System.EventHandler(this.tbPrierez1_Enter);
            this.tbPrierez1.Enter += new System.EventHandler(this.tbPrierez1_Enter);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 345);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 16);
            this.label13.TabIndex = 19;
            this.label13.Text = "Popis:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 386);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 16);
            this.label14.TabIndex = 18;
            this.label14.Text = "Povolene hodnoty:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelX);
            this.groupBox3.Controls.Add(this.tbPritok01);
            this.groupBox3.Location = new System.Drawing.Point(12, 242);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 80);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Prítokové Potrubie";
            // 
            // labelX
            // 
            this.labelX.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelX.Location = new System.Drawing.Point(23, 33);
            this.labelX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(61, 32);
            this.labelX.TabIndex = 12;
            this.labelX.Text = "Prítok:\r\n(m^3/s)";
            this.labelX.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbPritok01
            // 
            this.tbPritok01.AllowSpace = false;
            this.tbPritok01.Location = new System.Drawing.Point(91, 30);
            this.tbPritok01.Name = "tbPritok01";
            this.tbPritok01.Size = new System.Drawing.Size(67, 22);
            this.tbPritok01.TabIndex = 0;
            this.tbPritok01.Text = "1";
            this.tbPritok01.Click += new System.EventHandler(this.tbPotr3_Enter);
            this.tbPritok01.Enter += new System.EventHandler(this.tbPotr3_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.tbPrierez2);
            this.groupBox4.Location = new System.Drawing.Point(195, 168);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(177, 68);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Odtokové Potrubie";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 33);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 16);
            this.label10.TabIndex = 12;
            this.label10.Text = "Priemer (cm):";
            // 
            // tbPrierez2
            // 
            this.tbPrierez2.AllowSpace = false;
            this.tbPrierez2.Location = new System.Drawing.Point(91, 30);
            this.tbPrierez2.Name = "tbPrierez2";
            this.tbPrierez2.Size = new System.Drawing.Size(67, 22);
            this.tbPrierez2.TabIndex = 0;
            this.tbPrierez2.Text = "30";
            this.tbPrierez2.Click += new System.EventHandler(this.tbPrierez2_Enter);
            this.tbPrierez2.Enter += new System.EventHandler(this.tbPrierez2_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbM3);
            this.groupBox2.Controls.Add(this.rbLitre);
            this.groupBox2.Location = new System.Drawing.Point(195, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(177, 80);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Jednotky";
            // 
            // rbM3
            // 
            this.rbM3.AutoSize = true;
            this.rbM3.Checked = true;
            this.rbM3.Location = new System.Drawing.Point(23, 31);
            this.rbM3.Name = "rbM3";
            this.rbM3.Size = new System.Drawing.Size(62, 20);
            this.rbM3.TabIndex = 0;
            this.rbM3.TabStop = true;
            this.rbM3.Text = "m^3/s";
            this.rbM3.UseVisualStyleBackColor = true;
            this.rbM3.CheckedChanged += new System.EventHandler(this.rbChange_CheckedChanged);
            this.rbM3.Enter += new System.EventHandler(this.rbM3_Enter);
            // 
            // rbLitre
            // 
            this.rbLitre.AutoSize = true;
            this.rbLitre.Location = new System.Drawing.Point(91, 32);
            this.rbLitre.Name = "rbLitre";
            this.rbLitre.Size = new System.Drawing.Size(58, 20);
            this.rbLitre.TabIndex = 1;
            this.rbLitre.Text = "litre/s";
            this.rbLitre.UseVisualStyleBackColor = true;
            this.rbLitre.Enter += new System.EventHandler(this.rbM3_Enter);
            // 
            // Setting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 472);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lDescrip);
            this.Controls.Add(this.lAbleChar);
            this.Controls.Add(this.gbTank2);
            this.Controls.Add(this.gbTank1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Nastavenie parametrov";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Setting_KeyPress);
            this.gbTank1.ResumeLayout(false);
            this.gbTank1.PerformLayout();
            this.gbTank2.ResumeLayout(false);
            this.gbTank2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox gbTank1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbTank2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Classes.NumericTextBox tbVyska1;
        private Classes.NumericTextBox tbSirka1;
        private Classes.NumericTextBox tbDlzka1;
        private Classes.NumericTextBox tbSirka2;
        private Classes.NumericTextBox tbDlzka2;
        private Classes.NumericTextBox tbVyska2;
        private System.Windows.Forms.Label lAbleChar;
        private System.Windows.Forms.Label lDescrip;
        private Classes.NumericTextBox tbPrierez1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelX;
        private Classes.NumericTextBox tbPritok01;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private Classes.NumericTextBox tbPrierez2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbM3;
        private System.Windows.Forms.RadioButton rbLitre;
        private Classes.NumericTextBox tbHladina1;
        private System.Windows.Forms.Label label8;
        private Classes.NumericTextBox tbHladina2;
        private System.Windows.Forms.Label label9;
    }
}