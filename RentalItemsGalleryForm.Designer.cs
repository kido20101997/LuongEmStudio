namespace LuongEmStudio
{
    partial class RentalItemsGalleryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RentalItemsGalleryForm));
            button1 = new Button();
            button2 = new Button();
            lbTrang = new Label();
            imageList1 = new ImageList(components);
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.ImageIndex = 0;
            button1.ImageList = imageList1;
            button1.Location = new Point(404, 639);
            button1.Name = "button1";
            button1.Size = new Size(66, 31);
            button1.TabIndex = 0;
            button1.Text = "     BACK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom;
            button2.ImageAlign = ContentAlignment.MiddleRight;
            button2.ImageIndex = 1;
            button2.ImageList = imageList1;
            button2.Location = new Point(568, 639);
            button2.Name = "button2";
            button2.Size = new Size(62, 31);
            button2.TabIndex = 1;
            button2.Text = "NEXT    ";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lbTrang
            // 
            lbTrang.Anchor = AnchorStyles.Bottom;
            lbTrang.AutoSize = true;
            lbTrang.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTrang.ForeColor = Color.Red;
            lbTrang.Location = new Point(491, 647);
            lbTrang.Name = "lbTrang";
            lbTrang.Size = new Size(54, 17);
            lbTrang.TabIndex = 2;
            lbTrang.Text = "Trang 1";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "arrows.png");
            imageList1.Images.SetKeyName(1, "arrowsnext.png");
            // 
            // RentalItemsGalleryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1026, 672);
            Controls.Add(lbTrang);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "RentalItemsGalleryForm";
            Text = "RentalItemsGalleryForm";
            Load += RentalItemsGalleryForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label lbTrang;
        private ImageList imageList1;
    }
}