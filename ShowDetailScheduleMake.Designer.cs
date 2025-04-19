namespace LuongEmStudio
{
    partial class ShowDetailScheduleMake
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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowDetailScheduleMake));
            splitContainer1 = new SplitContainer();
            label2 = new Label();
            splitContainer2 = new SplitContainer();
            btupdate = new Button();
            btcancel = new Button();
            dtgvLichMake = new DataGridView();
            imageList1 = new ImageList(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtgvLichMake).BeginInit();
            SuspendLayout();
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
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1334, 575);
            splitContainer1.SplitterDistance = 40;
            splitContainer1.TabIndex = 0;
            // 
            // label2
            // 
            label2.BackColor = Color.LightSkyBlue;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(1334, 40);
            label2.TabIndex = 2;
            label2.Text = "Chi tiết lịch Make-Up";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.BackColor = Color.Transparent;
            splitContainer2.Panel1.Controls.Add(btupdate);
            splitContainer2.Panel1.Controls.Add(btcancel);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dtgvLichMake);
            splitContainer2.Size = new Size(1334, 531);
            splitContainer2.SplitterDistance = 42;
            splitContainer2.TabIndex = 1;
            // 
            // btupdate
            // 
            btupdate.BackColor = Color.GreenYellow;
            btupdate.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btupdate.ImageAlign = ContentAlignment.MiddleLeft;
            btupdate.ImageIndex = 1;
            btupdate.ImageList = imageList1;
            btupdate.Location = new Point(882, 4);
            btupdate.Name = "btupdate";
            btupdate.Size = new Size(90, 36);
            btupdate.TabIndex = 1;
            btupdate.Text = "  Sửa";
            btupdate.UseVisualStyleBackColor = false;
            btupdate.Click += btupdate_Click;
            // 
            // btcancel
            // 
            btcancel.BackColor = Color.GreenYellow;
            btcancel.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            btcancel.ImageAlign = ContentAlignment.MiddleLeft;
            btcancel.ImageIndex = 0;
            btcancel.ImageList = imageList1;
            btcancel.Location = new Point(361, 2);
            btcancel.Name = "btcancel";
            btcancel.Size = new Size(104, 36);
            btcancel.TabIndex = 0;
            btcancel.Text = "   Hủy lịch";
            btcancel.UseVisualStyleBackColor = false;
            btcancel.Click += btcancel_Click;
            // 
            // dtgvLichMake
            // 
            dtgvLichMake.AllowUserToAddRows = false;
            dtgvLichMake.AllowUserToDeleteRows = false;
            dtgvLichMake.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgvLichMake.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dtgvLichMake.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dtgvLichMake.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dtgvLichMake.DefaultCellStyle = dataGridViewCellStyle8;
            dtgvLichMake.Dock = DockStyle.Fill;
            dtgvLichMake.Location = new Point(0, 0);
            dtgvLichMake.Name = "dtgvLichMake";
            dtgvLichMake.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dtgvLichMake.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dtgvLichMake.Size = new Size(1334, 485);
            dtgvLichMake.TabIndex = 0;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "close.png");
            imageList1.Images.SetKeyName(1, "note.png");
            // 
            // ShowDetailScheduleMake
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1334, 575);
            Controls.Add(splitContainer1);
            Name = "ShowDetailScheduleMake";
            Text = "ShowDetailScheduleMake";
            FormClosed += ShowDetailScheduleMake_FormClosed;
            Load += ShowDetailScheduleMake_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtgvLichMake).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label2;
        private DataGridView dtgvLichMake;
        private SplitContainer splitContainer2;
        private Button btupdate;
        private Button btcancel;
        private ImageList imageList1;
    }
}