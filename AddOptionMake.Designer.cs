namespace LuongEmStudio
{
    partial class AddOptionMake
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
            tbGiaMake = new TextBox();
            btAdd = new Button();
            button1 = new Button();
            tbChiTietMake = new TextBox();
            label16 = new Label();
            label1 = new Label();
            label2 = new Label();
            tbnameOption = new TextBox();
            SuspendLayout();
            // 
            // tbGiaMake
            // 
            tbGiaMake.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbGiaMake.Location = new Point(180, 77);
            tbGiaMake.MaxLength = 1000000;
            tbGiaMake.Name = "tbGiaMake";
            tbGiaMake.Size = new Size(254, 33);
            tbGiaMake.TabIndex = 26;
            tbGiaMake.TextChanged += tbGiaMake_TextChanged;
            tbGiaMake.Enter += tbGiaMake_Enter;
            tbGiaMake.KeyPress += tbGiaMake_KeyPress;
            tbGiaMake.Leave += tbGiaMake_Leave;
            // 
            // btAdd
            // 
            btAdd.Location = new Point(75, 398);
            btAdd.Name = "btAdd";
            btAdd.Size = new Size(99, 31);
            btAdd.TabIndex = 49;
            btAdd.Text = "Thêm";
            btAdd.UseVisualStyleBackColor = true;
            btAdd.Click += btAdd_Click;
            // 
            // button1
            // 
            button1.Location = new Point(242, 398);
            button1.Name = "button1";
            button1.Size = new Size(100, 31);
            button1.TabIndex = 50;
            button1.Text = "Hủy";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btCancel;
            // 
            // tbChiTietMake
            // 
            tbChiTietMake.Font = new Font("Arial Unicode MS", 14.25F, FontStyle.Bold);
            tbChiTietMake.Location = new Point(20, 184);
            tbChiTietMake.MaxLength = 1000000;
            tbChiTietMake.Multiline = true;
            tbChiTietMake.Name = "tbChiTietMake";
            tbChiTietMake.Size = new Size(414, 194);
            tbChiTietMake.TabIndex = 51;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Rockwell", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label16.ForeColor = Color.FromArgb(0, 0, 192);
            label16.Location = new Point(8, 77);
            label16.Name = "label16";
            label16.Size = new Size(166, 24);
            label16.TabIndex = 52;
            label16.Text = "Giá gói Make-up:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Rockwell", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 0, 192);
            label1.Location = new Point(18, 147);
            label1.Name = "label1";
            label1.Size = new Size(201, 24);
            label1.TabIndex = 53;
            label1.Text = "Chi tiết gói Make-up:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Rockwell", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(0, 0, 192);
            label2.Location = new Point(8, 31);
            label2.Name = "label2";
            label2.Size = new Size(168, 24);
            label2.TabIndex = 55;
            label2.Text = "Tên gói Make-up:";
            // 
            // tbnameOption
            // 
            tbnameOption.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbnameOption.Location = new Point(180, 28);
            tbnameOption.Name = "tbnameOption";
            tbnameOption.Size = new Size(254, 29);
            tbnameOption.TabIndex = 56;
            // 
            // AddOptionMake
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 455);
            Controls.Add(tbnameOption);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label16);
            Controls.Add(tbChiTietMake);
            Controls.Add(button1);
            Controls.Add(btAdd);
            Controls.Add(tbGiaMake);
            Name = "AddOptionMake";
            Text = "AddOptionMake";
            Load += AddOptionMake_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbGiaMake;
        private Button btAdd;
        private Button button1;
        private TextBox tbChiTietMake;
        private Label label16;
        private Label label1;
        private Label label2;
        private TextBox tbnameOption;
    }
}