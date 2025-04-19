namespace LuongEmStudio
{
    partial class MainMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            picLogo = new PictureBox();
            menuStrip1 = new MenuStrip();
            homeToolStripMenuItem = new ToolStripMenuItem();
            RentClothings = new ToolStripMenuItem();
            MakeupSchedule = new ToolStripMenuItem();
            manageClothings = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            report = new ToolStripMenuItem();
            closeApp = new ToolStripMenuItem();
            picBackground = new PictureBox();
            panel1 = new Panel();
            splitContainer5 = new SplitContainer();
            panelBackground = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBackground).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            panelBackground.SuspendLayout();
            SuspendLayout();
            // 
            // picLogo
            // 
            picLogo.Dock = DockStyle.Fill;
            picLogo.Image = Properties.Resources._0906654e_c7e9_4432_a15d_d4d70ddd4749;
            picLogo.Location = new Point(0, 0);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(274, 271);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            picLogo.DoubleClick += picLogo_DoubleClick;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.AntiqueWhite;
            menuStrip1.Dock = DockStyle.Left;
            menuStrip1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            menuStrip1.GripMargin = new Padding(20, 20, 0, 20);
            menuStrip1.ImageScalingSize = new Size(40, 40);
            menuStrip1.Items.AddRange(new ToolStripItem[] { homeToolStripMenuItem, RentClothings, MakeupSchedule, manageClothings, toolStripMenuItem1, toolStripMenuItem2, report, closeApp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(294, 700);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // homeToolStripMenuItem
            // 
            homeToolStripMenuItem.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            homeToolStripMenuItem.Image = Properties.Resources.home;
            homeToolStripMenuItem.ImageAlign = ContentAlignment.MiddleLeft;
            homeToolStripMenuItem.Margin = new Padding(10, 40, 10, 10);
            homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            homeToolStripMenuItem.Size = new Size(261, 44);
            homeToolStripMenuItem.Text = " Home";
            homeToolStripMenuItem.Click += homeToolStripMenuItem_Click;
            // 
            // RentClothings
            // 
            RentClothings.Image = Properties.Resources.note;
            RentClothings.Margin = new Padding(10);
            RentClothings.Name = "RentClothings";
            RentClothings.Size = new Size(261, 44);
            RentClothings.Text = " Cho thuê - trả đồ";
            RentClothings.Click += RentClothings_Click;
            // 
            // MakeupSchedule
            // 
            MakeupSchedule.Image = Properties.Resources.calendar;
            MakeupSchedule.ImageAlign = ContentAlignment.MiddleLeft;
            MakeupSchedule.Margin = new Padding(10);
            MakeupSchedule.Name = "MakeupSchedule";
            MakeupSchedule.Size = new Size(261, 44);
            MakeupSchedule.Text = " Lịch Make-up";
            MakeupSchedule.Click += MakeupSchedule_Click;
            // 
            // manageClothings
            // 
            manageClothings.Image = Properties.Resources.project_management;
            manageClothings.ImageAlign = ContentAlignment.MiddleLeft;
            manageClothings.Margin = new Padding(10);
            manageClothings.Name = "manageClothings";
            manageClothings.Size = new Size(261, 44);
            manageClothings.Text = " Quản lý đồ";
            manageClothings.Click += manageClothings_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Image = Properties.Resources.recruitment;
            toolStripMenuItem1.ImageAlign = ContentAlignment.MiddleLeft;
            toolStripMenuItem1.Margin = new Padding(10);
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(261, 44);
            toolStripMenuItem1.Text = " Nhân sự";
            toolStripMenuItem1.TextAlign = ContentAlignment.MiddleLeft;
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Image = Properties.Resources.technology1;
            toolStripMenuItem2.ImageAlign = ContentAlignment.MiddleLeft;
            toolStripMenuItem2.Margin = new Padding(10);
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(261, 44);
            toolStripMenuItem2.Text = " Lịch chụp";
            toolStripMenuItem2.TextAlign = ContentAlignment.MiddleLeft;
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // report
            // 
            report.Image = Properties.Resources.analysis;
            report.ImageAlign = ContentAlignment.MiddleLeft;
            report.Margin = new Padding(10);
            report.Name = "report";
            report.Size = new Size(261, 44);
            report.Text = " Báo cáo";
            report.TextAlign = ContentAlignment.MiddleLeft;
            report.Click += report_Click;
            // 
            // closeApp
            // 
            closeApp.Image = Properties.Resources.power_button;
            closeApp.ImageAlign = ContentAlignment.MiddleLeft;
            closeApp.Margin = new Padding(10);
            closeApp.Name = "closeApp";
            closeApp.Size = new Size(261, 44);
            closeApp.Text = " Thoát";
            closeApp.TextAlign = ContentAlignment.MiddleLeft;
            closeApp.Click += closeApp_Click;
            // 
            // picBackground
            // 
            picBackground.Dock = DockStyle.Fill;
            picBackground.Image = Properties.Resources.hay_vung_tin_vao_nhung_uoc_mong_ban_da_gui_den_vu_tru;
            picBackground.Location = new Point(0, 0);
            picBackground.Name = "picBackground";
            picBackground.Size = new Size(1225, 975);
            picBackground.SizeMode = PictureBoxSizeMode.StretchImage;
            picBackground.TabIndex = 1;
            picBackground.TabStop = false;
            picBackground.DoubleClick += picBackground_DoubleClick;
            // 
            // panel1
            // 
            panel1.Controls.Add(splitContainer5);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(274, 975);
            panel1.TabIndex = 2;
            // 
            // splitContainer5
            // 
            splitContainer5.Dock = DockStyle.Fill;
            splitContainer5.Location = new Point(0, 0);
            splitContainer5.Name = "splitContainer5";
            splitContainer5.Orientation = Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(picLogo);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(menuStrip1);
            splitContainer5.Size = new Size(274, 975);
            splitContainer5.SplitterDistance = 271;
            splitContainer5.TabIndex = 0;
            // 
            // panelBackground
            // 
            panelBackground.Controls.Add(picBackground);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(274, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(1225, 975);
            panelBackground.TabIndex = 4;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 900000;
            timer1.Tick += timer1_Tick;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1499, 975);
            Controls.Add(panelBackground);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "MainMenu";
            Text = "LuongEM Studio Rent&MakeUp (1.0.0.3)";
            Load += MainMenu_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picBackground).EndInit();
            panel1.ResumeLayout(false);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
            splitContainer5.ResumeLayout(false);
            panelBackground.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer3;
        private SplitContainer splitContainer2;
        private PictureBox picLogo;
        private PictureBox picBackground;
        private Panel panel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem homeToolStripMenuItem;
        private ToolStripMenuItem MakeupSchedule;
        private ToolStripMenuItem manageClothings;
        private ToolStripMenuItem RentClothings;
        private ToolStripMenuItem report;
        private SplitContainer splitContainer5;
        private SplitContainer splitContainer4;
        private Panel panelBackground;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Timer timer1;
        private ToolStripMenuItem closeApp;
    }
}
