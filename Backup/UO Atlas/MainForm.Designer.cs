namespace UO_Atlas
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.ToolStrip();
            this.menuApplication = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuTrackPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHideControls = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblZoom = new System.Windows.Forms.ToolStripLabel();
            this.cbZoom = new System.Windows.Forms.ToolStripComboBox();
            this.menuAbout = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblHoveredCoordsTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHoveredCoords = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMaps = new System.Windows.Forms.TabControl();
            this.tabPageFelucca = new System.Windows.Forms.TabPage();
            this.tabPageTrammel = new System.Windows.Forms.TabPage();
            this.tabPageIlshenar = new System.Windows.Forms.TabPage();
            this.tabPageMalas = new System.Windows.Forms.TabPage();
            this.tabPageTokuno = new System.Windows.Forms.TabPage();
            this.timerTrackPlayer = new System.Windows.Forms.Timer(this.components);
            this.mapViewer = new UO_Atlas.MapViewer();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControlMaps.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuApplication,
            this.toolStripSeparator2,
            this.lblZoom,
            this.cbZoom,
            this.menuAbout});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip.Size = new System.Drawing.Size(592, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "Main Menu";
            // 
            // menuApplication
            // 
            this.menuApplication.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuApplication.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTrackPlayer,
            this.menuHideControls,
            this.menuSettings,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuApplication.Image = ((System.Drawing.Image)(resources.GetObject("menuApplication.Image")));
            this.menuApplication.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuApplication.Name = "menuApplication";
            this.menuApplication.Size = new System.Drawing.Size(72, 22);
            this.menuApplication.Text = "&Application";
            // 
            // menuTrackPlayer
            // 
            this.menuTrackPlayer.CheckOnClick = true;
            this.menuTrackPlayer.Name = "menuTrackPlayer";
            this.menuTrackPlayer.Size = new System.Drawing.Size(190, 22);
            this.menuTrackPlayer.Text = "Track &Player";
            this.menuTrackPlayer.CheckedChanged += new System.EventHandler(this.menuTrackPlayer_CheckedChanged);
            // 
            // menuHideControls
            // 
            this.menuHideControls.CheckOnClick = true;
            this.menuHideControls.Name = "menuHideControls";
            this.menuHideControls.Size = new System.Drawing.Size(190, 22);
            this.menuHideControls.Text = "&Hide Controls";
            this.menuHideControls.CheckedChanged += new System.EventHandler(this.menuHideControls_CheckedChanged);
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(190, 22);
            this.menuSettings.Text = "&Generate map-images";
            this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(190, 22);
            this.menuExit.Text = "&Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblZoom
            // 
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(37, 22);
            this.lblZoom.Text = "Zoom:";
            // 
            // cbZoom
            // 
            this.cbZoom.AutoSize = false;
            this.cbZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbZoom.Items.AddRange(new object[] {
            "    50%",
            "  100%",
            "  200%",
            "  400%",
            "  800%",
            "1600%",
            "3200%"});
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Size = new System.Drawing.Size(60, 21);
            this.cbZoom.SelectedIndexChanged += new System.EventHandler(this.cbZoom_SelectedIndexChanged);
            // 
            // menuAbout
            // 
            this.menuAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuAbout.Image = global::UO_Atlas.Properties.Resources.info_16;
            this.menuAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(23, 22);
            this.menuAbout.Text = "toolStripButton1";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHoveredCoordsTitle,
            this.lblHoveredCoords});
            this.statusStrip.Location = new System.Drawing.Point(0, 444);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(592, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblHoveredCoordsTitle
            // 
            this.lblHoveredCoordsTitle.Name = "lblHoveredCoordsTitle";
            this.lblHoveredCoordsTitle.Size = new System.Drawing.Size(116, 17);
            this.lblHoveredCoordsTitle.Text = "Hovered Coordinates: ";
            // 
            // lblHoveredCoords
            // 
            this.lblHoveredCoords.Name = "lblHoveredCoords";
            this.lblHoveredCoords.Size = new System.Drawing.Size(56, 17);
            this.lblHoveredCoords.Text = "no Coords";
            // 
            // tabControlMaps
            // 
            this.tabControlMaps.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlMaps.Controls.Add(this.tabPageFelucca);
            this.tabControlMaps.Controls.Add(this.tabPageTrammel);
            this.tabControlMaps.Controls.Add(this.tabPageIlshenar);
            this.tabControlMaps.Controls.Add(this.tabPageMalas);
            this.tabControlMaps.Controls.Add(this.tabPageTokuno);
            this.tabControlMaps.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlMaps.Location = new System.Drawing.Point(0, 25);
            this.tabControlMaps.Name = "tabControlMaps";
            this.tabControlMaps.SelectedIndex = 0;
            this.tabControlMaps.Size = new System.Drawing.Size(592, 20);
            this.tabControlMaps.TabIndex = 3;
            this.tabControlMaps.SelectedIndexChanged += new System.EventHandler(this.tabControlMaps_SelectedIndexChanged);
            // 
            // tabPageFelucca
            // 
            this.tabPageFelucca.Location = new System.Drawing.Point(4, 25);
            this.tabPageFelucca.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFelucca.Name = "tabPageFelucca";
            this.tabPageFelucca.Size = new System.Drawing.Size(584, 0);
            this.tabPageFelucca.TabIndex = 0;
            this.tabPageFelucca.Text = "Felucca";
            this.tabPageFelucca.UseVisualStyleBackColor = true;
            // 
            // tabPageTrammel
            // 
            this.tabPageTrammel.Location = new System.Drawing.Point(4, 25);
            this.tabPageTrammel.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageTrammel.Name = "tabPageTrammel";
            this.tabPageTrammel.Size = new System.Drawing.Size(584, -9);
            this.tabPageTrammel.TabIndex = 1;
            this.tabPageTrammel.Text = "Trammel";
            this.tabPageTrammel.UseVisualStyleBackColor = true;
            // 
            // tabPageIlshenar
            // 
            this.tabPageIlshenar.Location = new System.Drawing.Point(4, 25);
            this.tabPageIlshenar.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageIlshenar.Name = "tabPageIlshenar";
            this.tabPageIlshenar.Size = new System.Drawing.Size(584, -9);
            this.tabPageIlshenar.TabIndex = 2;
            this.tabPageIlshenar.Text = "Ilshenar";
            this.tabPageIlshenar.UseVisualStyleBackColor = true;
            // 
            // tabPageMalas
            // 
            this.tabPageMalas.Location = new System.Drawing.Point(4, 25);
            this.tabPageMalas.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMalas.Name = "tabPageMalas";
            this.tabPageMalas.Size = new System.Drawing.Size(584, -9);
            this.tabPageMalas.TabIndex = 3;
            this.tabPageMalas.Text = "Malas";
            this.tabPageMalas.UseVisualStyleBackColor = true;
            // 
            // tabPageTokuno
            // 
            this.tabPageTokuno.Location = new System.Drawing.Point(4, 25);
            this.tabPageTokuno.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageTokuno.Name = "tabPageTokuno";
            this.tabPageTokuno.Size = new System.Drawing.Size(584, -9);
            this.tabPageTokuno.TabIndex = 4;
            this.tabPageTokuno.Text = "Tokuno";
            this.tabPageTokuno.UseVisualStyleBackColor = true;
            // 
            // timerTrackPlayer
            // 
            this.timerTrackPlayer.Interval = 1000;
            this.timerTrackPlayer.Tick += new System.EventHandler(this.timerTrackPlayer_Tick);
            // 
            // mapViewer
            // 
            this.mapViewer.BackColor = System.Drawing.Color.Transparent;
            this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer.Location = new System.Drawing.Point(0, 45);
            this.mapViewer.Map = null;
            this.mapViewer.Margin = new System.Windows.Forms.Padding(0);
            this.mapViewer.Name = "mapViewer";
            this.mapViewer.Size = new System.Drawing.Size(592, 399);
            this.mapViewer.TabIndex = 1;
            this.mapViewer.ZoomLevel = UO_Atlas.ZoomLevel.Percent100;
            this.mapViewer.DoubleClick += new System.EventHandler(this.mapViewer_DoubleClick);
            this.mapViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapViewer_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 466);
            this.Controls.Add(this.mapViewer);
            this.Controls.Add(this.tabControlMaps);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Name = "MainForm";
            this.Text = "UO Atlas";
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseWheel);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControlMaps.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip menuStrip;
        private MapViewer mapViewer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripDropDownButton menuApplication;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuHideControls;
        private System.Windows.Forms.TabControl tabControlMaps;
        private System.Windows.Forms.TabPage tabPageFelucca;
        private System.Windows.Forms.TabPage tabPageTrammel;
        private System.Windows.Forms.TabPage tabPageIlshenar;
        private System.Windows.Forms.TabPage tabPageMalas;
        private System.Windows.Forms.TabPage tabPageTokuno;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblZoom;
        private System.Windows.Forms.ToolStripComboBox cbZoom;
        private System.Windows.Forms.ToolStripStatusLabel lblHoveredCoordsTitle;
        private System.Windows.Forms.ToolStripStatusLabel lblHoveredCoords;
        private System.Windows.Forms.ToolStripMenuItem menuTrackPlayer;
        private System.Windows.Forms.Timer timerTrackPlayer;
        private System.Windows.Forms.ToolStripButton menuAbout;
    }
}

