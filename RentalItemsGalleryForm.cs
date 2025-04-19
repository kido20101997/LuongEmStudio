using LuongEmStudio.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuongEmStudio
{
    public partial class RentalItemsGalleryForm : Form
    {
        private FlowLayoutPanel flowPanel;
        int currentPage = 1;
        int itemsPerPage = 40; // 5 ảnh mỗi hàng, 10 hàng
        List<DataRow> allItems = new List<DataRow>();

        public RentalItemsGalleryForm(DataTable items)
        {
            InitializeComponent();
            Text = "Chọn đồ cần thuê";
            Width = 800;
            Height = 600;

            flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            Controls.Add(flowPanel);
            LoadItems(items);
        }

        private void RentalItemsGalleryForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void LoadItems(DataTable items)
        {
            allItems = items.AsEnumerable().ToList(); // <- Gán dữ liệu từ DataTable vào allItems
            currentPage = 1;
            LoadItemsPage(currentPage); // Load trang đầu tiên
            lbTrang.Text = "Trang 1";
        }
        private void LoadItemsPage(int page)
        {
            flowPanel.Controls.Clear();

            int skip = (page - 1) * itemsPerPage;
            var itemsToShow = allItems.Skip(skip).Take(itemsPerPage);

            foreach (DataRow row in itemsToShow)
            {
                string imagePath = row["ImagePath"].ToString();
                string itemName = row["ItemName"].ToString();

                // Gán tag là object của ProductTag
                ProductTag productTag = new ProductTag
                {
                    ProductId = row["productid"].ToString(),
                    ProductName = row["productname"].ToString(),
                    Description = row["description"].ToString(),
                    PricePerDay = row["priceperday"].ToString(),
                    Size = row["size"].ToString(),
                    StockQuantity = row["stockquantity"].ToString(),
                    ImagePath = imagePath,
                    QTYinWH = row["saveqty"].ToString()
                };

                PictureBox pic = new PictureBox
                {
                    Width = 170,
                    Height = 210,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = new Padding(15),
                    Tag = productTag
                };

                if (File.Exists(imagePath))
                {
                    pic.Image = Image.FromFile(imagePath);
                }
                else
                {
                    pic.BackColor = Color.Gray;
                }

                Label label = new Label
                {
                    Text = itemName,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom,
                    AutoSize = false,
                    Height = 30
                };

                Panel container = new Panel
                {
                    Width = 180,
                    Height = 240
                };

                container.Controls.Add(pic);
                container.Controls.Add(label);

                pic.Click += (s, e) =>
                {
                    var tag = (ProductTag)((PictureBox)s).Tag;

                    SelectedProductInfo.ProductId = tag.ProductId;
                    SelectedProductInfo.ProductName = tag.ProductName;
                    SelectedProductInfo.Description = tag.Description;
                    SelectedProductInfo.PricePerDay = tag.PricePerDay;
                    SelectedProductInfo.Size = tag.Size;
                    SelectedProductInfo.stockquantity = tag.StockQuantity;
                    SelectedProductInfo.imagePath = tag.ImagePath;
                    SelectedProductInfo.QTYinWH = tag.QTYinWH;

                    this.DialogResult = DialogResult.OK;
                    Close();
                };

                flowPanel.Controls.Add(container);
            }
        }
        int trang = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)allItems.Count / itemsPerPage);
            if (currentPage < maxPage)
            {
                trang++;
                currentPage++;
                LoadItemsPage(currentPage);
                lbTrang.Text= $"Trang {trang.ToString()}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                trang--;
                currentPage--;
                LoadItemsPage(currentPage);
                lbTrang.Text = $"Trang {trang.ToString()}";
            }
        }
    }
}
