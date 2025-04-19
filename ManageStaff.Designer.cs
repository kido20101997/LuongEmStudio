namespace LuongEmStudio
{
    partial class ManageStaff
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageStaff));
            imageList1 = new ImageList(components);
            lbmes = new Label();
            dtgvStaffInfo = new DataGridView();
            groupBox2 = new GroupBox();
            btdelete = new Button();
            btadd = new Button();
            splitContainer4 = new SplitContainer();
            tbnotes = new TextBox();
            label3 = new Label();
            tbaddress = new TextBox();
            btupdate = new Button();
            btfind = new Button();
            tbsdt = new TextBox();
            label7 = new Label();
            label6 = new Label();
            tbName = new TextBox();
            label5 = new Label();
            splitContainer1 = new SplitContainer();
            label2 = new Label();
            label1 = new Label();
            TabControlMake = new TabControl();
            TabBooking = new TabPage();
            splitContainer2 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)dtgvStaffInfo).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            TabControlMake.SuspendLayout();
            TabBooking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "bin.png");
            imageList1.Images.SetKeyName(1, "find.png");
            imageList1.Images.SetKeyName(2, "plus.png");
            imageList1.Images.SetKeyName(3, "recruitment.png");
            imageList1.Images.SetKeyName(4, "update.png");
            // 
            // lbmes
            // 
            lbmes.BackColor = Color.Gainsboro;
            lbmes.Dock = DockStyle.Fill;
            lbmes.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbmes.Location = new Point(0, 0);
            lbmes.Name = "lbmes";
            lbmes.Size = new Size(1180, 32);
            lbmes.TabIndex = 3;
            lbmes.Text = "Message";
            lbmes.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dtgvStaffInfo
            // 
            dtgvStaffInfo.AllowUserToAddRows = false;
            dtgvStaffInfo.AllowUserToDeleteRows = false;
            dtgvStaffInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtgvStaffInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dtgvStaffInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtgvStaffInfo.Dock = DockStyle.Fill;
            dtgvStaffInfo.Location = new Point(3, 26);
            dtgvStaffInfo.Name = "dtgvStaffInfo";
            dtgvStaffInfo.ReadOnly = true;
            dtgvStaffInfo.Size = new Size(1174, 209);
            dtgvStaffInfo.TabIndex = 1;
            dtgvStaffInfo.CellClick += dtgvLichmake_CellClick;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dtgvStaffInfo);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1180, 238);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Thông tin nhân viên";
            // 
            // btdelete
            // 
            btdelete.BackColor = Color.PapayaWhip;
            btdelete.Font = new Font("Rockwell", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btdelete.ImageAlign = ContentAlignment.MiddleLeft;
            btdelete.ImageIndex = 0;
            btdelete.ImageList = imageList1;
            btdelete.Location = new Point(1031, 264);
            btdelete.Name = "btdelete";
            btdelete.Size = new Size(144, 48);
            btdelete.TabIndex = 50;
            btdelete.Text = "         Xóa nhân viên";
            btdelete.UseVisualStyleBackColor = false;
            btdelete.Click += btdelete_Click;
            // 
            // btadd
            // 
            btadd.BackColor = Color.PapayaWhip;
            btadd.Font = new Font("Rockwell", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btadd.ImageAlign = ContentAlignment.MiddleLeft;
            btadd.ImageIndex = 2;
            btadd.ImageList = imageList1;
            btadd.Location = new Point(255, 268);
            btadd.Name = "btadd";
            btadd.Size = new Size(159, 44);
            btadd.TabIndex = 49;
            btadd.Text = "         Thêm nhân viên";
            btadd.UseVisualStyleBackColor = false;
            btadd.Click += btadd_Click;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = DockStyle.Fill;
            splitContainer4.Location = new Point(0, 0);
            splitContainer4.Name = "splitContainer4";
            splitContainer4.Orientation = Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(tbnotes);
            splitContainer4.Panel1.Controls.Add(label3);
            splitContainer4.Panel1.Controls.Add(tbaddress);
            splitContainer4.Panel1.Controls.Add(btupdate);
            splitContainer4.Panel1.Controls.Add(btfind);
            splitContainer4.Panel1.Controls.Add(btdelete);
            splitContainer4.Panel1.Controls.Add(btadd);
            splitContainer4.Panel1.Controls.Add(tbsdt);
            splitContainer4.Panel1.Controls.Add(label7);
            splitContainer4.Panel1.Controls.Add(label6);
            splitContainer4.Panel1.Controls.Add(tbName);
            splitContainer4.Panel1.Controls.Add(label5);
            splitContainer4.Panel1.Font = new Font("Rockwell", 14.25F);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(groupBox2);
            splitContainer4.Size = new Size(1180, 590);
            splitContainer4.SplitterDistance = 348;
            splitContainer4.TabIndex = 47;
            // 
            // tbnotes
            // 
            tbnotes.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbnotes.Location = new Point(201, 195);
            tbnotes.Name = "tbnotes";
            tbnotes.Size = new Size(432, 33);
            tbnotes.TabIndex = 55;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 14.25F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(0, 0, 192);
            label3.Location = new Point(127, 202);
            label3.Name = "label3";
            label3.Size = new Size(70, 22);
            label3.TabIndex = 54;
            label3.Text = "Notes:";
            // 
            // tbaddress
            // 
            tbaddress.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbaddress.Location = new Point(201, 149);
            tbaddress.Name = "tbaddress";
            tbaddress.Size = new Size(432, 33);
            tbaddress.TabIndex = 53;
            tbaddress.TextChanged += tbaddress_TextChanged;
            tbaddress.KeyDown += tbaddress_KeyDown;
            // 
            // btupdate
            // 
            btupdate.BackColor = Color.PapayaWhip;
            btupdate.Font = new Font("Rockwell", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btupdate.ImageAlign = ContentAlignment.MiddleLeft;
            btupdate.ImageIndex = 4;
            btupdate.ImageList = imageList1;
            btupdate.Location = new Point(829, 264);
            btupdate.Name = "btupdate";
            btupdate.Size = new Size(172, 48);
            btupdate.TabIndex = 52;
            btupdate.Text = "       Cập nhật thông tin nhân viên";
            btupdate.UseVisualStyleBackColor = false;
            btupdate.Click += btupdate_Click;
            // 
            // btfind
            // 
            btfind.BackColor = Color.PapayaWhip;
            btfind.Font = new Font("Rockwell", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btfind.ImageAlign = ContentAlignment.MiddleLeft;
            btfind.ImageKey = "find.png";
            btfind.ImageList = imageList1;
            btfind.Location = new Point(829, 202);
            btfind.Name = "btfind";
            btfind.Size = new Size(143, 43);
            btfind.TabIndex = 51;
            btfind.Text = "       Tìm nhân viên";
            btfind.UseVisualStyleBackColor = false;
            btfind.Click += btfind_Click;
            // 
            // tbsdt
            // 
            tbsdt.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbsdt.Location = new Point(201, 97);
            tbsdt.Name = "tbsdt";
            tbsdt.Size = new Size(276, 33);
            tbsdt.TabIndex = 46;
            tbsdt.KeyDown += tbsdt_KeyDown;
            tbsdt.KeyPress += tbsdt_KeyPress;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Arial", 14.25F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(0, 0, 192);
            label7.Location = new Point(123, 102);
            label7.Name = "label7";
            label7.Size = new Size(75, 22);
            label7.TabIndex = 47;
            label7.Text = "Fb/sdt:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Arial", 14.25F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(0, 0, 192);
            label6.Location = new Point(118, 155);
            label6.Name = "label6";
            label6.Size = new Size(80, 22);
            label6.TabIndex = 44;
            label6.Text = "Địa chỉ:";
            // 
            // tbName
            // 
            tbName.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbName.Location = new Point(201, 45);
            tbName.Name = "tbName";
            tbName.Size = new Size(276, 33);
            tbName.TabIndex = 5;
            tbName.TextChanged += tbName_TextChanged;
            tbName.KeyDown += tbName_KeyDown;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial", 14.25F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(0, 0, 192);
            label5.Location = new Point(105, 51);
            label5.Name = "label5";
            label5.Size = new Size(93, 22);
            label5.TabIndex = 6;
            label5.Text = "Họ & Tên :";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(label1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(TabControlMake);
            splitContainer1.Size = new Size(1194, 714);
            splitContainer1.SplitterDistance = 37;
            splitContainer1.TabIndex = 3;
            // 
            // label2
            // 
            label2.BackColor = Color.LightSkyBlue;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(1194, 37);
            label2.TabIndex = 1;
            label2.Text = "Schedule Make-Up";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(1194, 37);
            label1.TabIndex = 0;
            label1.Text = "Lịch Make-Up";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TabControlMake
            // 
            TabControlMake.Controls.Add(TabBooking);
            TabControlMake.Dock = DockStyle.Fill;
            TabControlMake.Font = new Font("Rockwell", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            TabControlMake.ImageList = imageList1;
            TabControlMake.Location = new Point(0, 0);
            TabControlMake.Name = "TabControlMake";
            TabControlMake.SelectedIndex = 0;
            TabControlMake.Size = new Size(1194, 673);
            TabControlMake.TabIndex = 0;
            // 
            // TabBooking
            // 
            TabBooking.Controls.Add(splitContainer2);
            TabBooking.ImageIndex = 3;
            TabBooking.Location = new Point(4, 37);
            TabBooking.Name = "TabBooking";
            TabBooking.Padding = new Padding(3);
            TabBooking.Size = new Size(1186, 632);
            TabBooking.TabIndex = 0;
            TabBooking.Text = "Thông tin nhân viên";
            TabBooking.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(lbmes);
            splitContainer2.Size = new Size(1180, 626);
            splitContainer2.SplitterDistance = 590;
            splitContainer2.TabIndex = 0;
            // 
            // ManageStaff
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1194, 714);
            Controls.Add(splitContainer1);
            Name = "ManageStaff";
            Text = "ManageStaff";
            Load += ManageStaff_Load;
            ((System.ComponentModel.ISupportInitialize)dtgvStaffInfo).EndInit();
            groupBox2.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel1.PerformLayout();
            splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            TabControlMake.ResumeLayout(false);
            TabBooking.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ImageList imageList1;
        private Label lbmes;
        private DataGridView dtgvStaffInfo;
        private GroupBox groupBox2;
        private Button btdelete;
        private Button btadd;
        private SplitContainer splitContainer4;
        private Button btfind;
        private TextBox tbsdt;
        private Label label7;
        private Label label6;
        private TextBox tbName;
        private Label label5;
        private SplitContainer splitContainer1;
        private Label label2;
        private Label label1;
        private TabControl TabControlMake;
        private TabPage TabBooking;
        private SplitContainer splitContainer2;
        private TextBox tbnotes;
        private Label label3;
        private TextBox tbaddress;
        private Button btupdate;
    }
}