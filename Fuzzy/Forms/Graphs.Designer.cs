namespace Fuzzy.Forms
{
    partial class Graphs
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
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graphs));
            this.graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNewGraph = new System.Windows.Forms.ToolStripButton();
            this.tsbFocus = new System.Windows.Forms.ToolStripButton();
            this.tsbSetSizeAll = new System.Windows.Forms.ToolStripButton();
            this.tsbSort = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVariable = new System.Windows.Forms.ComboBox();
            this.cbLockX = new System.Windows.Forms.CheckBox();
            this.cbLockY = new System.Windows.Forms.CheckBox();
            this.sliderX = new System.Windows.Forms.TrackBar();
            this.sliderY = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderY)).BeginInit();
            this.SuspendLayout();
            // 
            // graph
            // 
            this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.IsDockedInsideChartArea = false;
            legend1.MaximumAutoSize = 10F;
            legend1.Name = "Legend1";
            this.graph.Legends.Add(legend1);
            this.graph.Location = new System.Drawing.Point(40, 61);
            this.graph.Name = "graph";
            this.graph.Size = new System.Drawing.Size(457, 211);
            this.graph.TabIndex = 0;
            this.graph.Text = "Graf";
            this.graph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.graph_MouseDown);
            this.graph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graph_MouseMove);
            this.graph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.graph_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewGraph,
            this.tsbFocus,
            this.tsbSetSizeAll,
            this.tsbSort});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(509, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNewGraph
            // 
            this.tsbNewGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewGraph.Image = global::Fuzzy.Properties.Resources.Plus;
            this.tsbNewGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewGraph.Name = "tsbNewGraph";
            this.tsbNewGraph.Size = new System.Drawing.Size(23, 22);
            this.tsbNewGraph.Text = "Nový graf";
            this.tsbNewGraph.Click += new System.EventHandler(this.tsbNewGraph_Click);
            // 
            // tsbFocus
            // 
            this.tsbFocus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbFocus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFocus.Image = global::Fuzzy.Properties.Resources.EyeIcon;
            this.tsbFocus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFocus.Name = "tsbFocus";
            this.tsbFocus.Size = new System.Drawing.Size(23, 22);
            this.tsbFocus.Text = "Zobraz všetky okná grafoch";
            this.tsbFocus.Click += new System.EventHandler(this.tsbFocus_Click);
            // 
            // tsbSetSizeAll
            // 
            this.tsbSetSizeAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSetSizeAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetSizeAll.Image = global::Fuzzy.Properties.Resources.SizeAll;
            this.tsbSetSizeAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetSizeAll.Name = "tsbSetSizeAll";
            this.tsbSetSizeAll.Size = new System.Drawing.Size(23, 22);
            this.tsbSetSizeAll.Text = "Zoradi grafy podľa aktualnej veľkosti";
            this.tsbSetSizeAll.Click += new System.EventHandler(this.tsbSetSizeAll_Click);
            // 
            // tsbSort
            // 
            this.tsbSort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSort.Image = global::Fuzzy.Properties.Resources.Sort;
            this.tsbSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSort.Name = "tsbSort";
            this.tsbSort.Size = new System.Drawing.Size(23, 22);
            this.tsbSort.Text = "Zoradiť grafy na celú obrazovku";
            this.tsbSort.Click += new System.EventHandler(this.tsbSort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Zobraziť:";
            // 
            // cbVariable
            // 
            this.cbVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVariable.FormattingEnabled = true;
            this.cbVariable.Location = new System.Drawing.Point(75, 32);
            this.cbVariable.Name = "cbVariable";
            this.cbVariable.Size = new System.Drawing.Size(121, 23);
            this.cbVariable.TabIndex = 3;
            this.cbVariable.SelectedIndexChanged += new System.EventHandler(this.cbVariable_SelectedIndexChanged);
            // 
            // cbLockX
            // 
            this.cbLockX.AutoSize = true;
            this.cbLockX.Location = new System.Drawing.Point(283, 36);
            this.cbLockX.Name = "cbLockX";
            this.cbLockX.Size = new System.Drawing.Size(34, 19);
            this.cbLockX.TabIndex = 4;
            this.cbLockX.Text = "X";
            this.cbLockX.UseVisualStyleBackColor = true;
            this.cbLockX.CheckedChanged += new System.EventHandler(this.cbLockX_CheckedChanged);
            // 
            // cbLockY
            // 
            this.cbLockY.AutoSize = true;
            this.cbLockY.Location = new System.Drawing.Point(323, 36);
            this.cbLockY.Name = "cbLockY";
            this.cbLockY.Size = new System.Drawing.Size(33, 19);
            this.cbLockY.TabIndex = 5;
            this.cbLockY.Text = "Y";
            this.cbLockY.UseVisualStyleBackColor = true;
            this.cbLockY.CheckedChanged += new System.EventHandler(this.cbLockY_CheckedChanged);
            // 
            // sliderX
            // 
            this.sliderX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sliderX.AutoSize = false;
            this.sliderX.LargeChange = 2;
            this.sliderX.Location = new System.Drawing.Point(40, 278);
            this.sliderX.Minimum = -4;
            this.sliderX.Name = "sliderX";
            this.sliderX.Size = new System.Drawing.Size(150, 32);
            this.sliderX.TabIndex = 6;
            this.sliderX.Scroll += new System.EventHandler(this.sliderX_Scroll);
            // 
            // sliderY
            // 
            this.sliderY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sliderY.AutoSize = false;
            this.sliderY.LargeChange = 2;
            this.sliderY.Location = new System.Drawing.Point(2, 172);
            this.sliderY.Name = "sliderY";
            this.sliderY.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.sliderY.Size = new System.Drawing.Size(32, 100);
            this.sliderY.TabIndex = 7;
            this.sliderY.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.sliderY.Value = 10;
            this.sliderY.Scroll += new System.EventHandler(this.sliderY_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Uzamkni os:";
            // 
            // Graphs
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(509, 312);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sliderY);
            this.Controls.Add(this.sliderX);
            this.Controls.Add(this.cbLockY);
            this.Controls.Add(this.cbLockX);
            this.Controls.Add(this.cbVariable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.graph);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(375, 250);
            this.Name = "Graphs";
            this.Text = "Graphs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Graphs_FormClosing);
            this.Load += new System.EventHandler(this.Graphs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart graph;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNewGraph;
        private System.Windows.Forms.ToolStripButton tsbSetSizeAll;
        private System.Windows.Forms.ToolStripButton tsbFocus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbVariable;
        private System.Windows.Forms.ToolStripButton tsbSort;
        private System.Windows.Forms.CheckBox cbLockX;
        private System.Windows.Forms.CheckBox cbLockY;
        private System.Windows.Forms.TrackBar sliderX;
        private System.Windows.Forms.TrackBar sliderY;
        private System.Windows.Forms.Label label2;
    }
}