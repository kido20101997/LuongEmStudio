using LuongEmStudio.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LuongEmStudio.Core;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Npgsql.Internal;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.Encodings.Web;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuongEmStudio
{
    public partial class ManageThings : Form
    {
        BaseDataSPInfo product;
        Controller controller;
        private byte[] blob3 = { 0 };
        private string pathImage = "", fileExtension = "";
        public ManageThings()
        {
            InitializeComponent();
            controller = new Controller();
            product = new BaseDataSPInfo();
            if (!Directory.Exists(BaseDataSPInfo.ImageFolder))
            {
                Directory.CreateDirectory(BaseDataSPInfo.ImageFolder);
            }
        }

        public void undo()
        {
            tbNameSP.Text = "";
            tbDescSP.Text = "";
            tbSizeSP.Text = "";
            tbQtySP.Text = "";

            tbtensp.Text = "";
            tbsizesanpham.Text = "";
            tbsoluongsp.Text = "";
            tbmotasp.Text = "";

            PictureSP.Image = null;
            PictureSP.Refresh();
            tbNameSP.Focus();
        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            ExecutionResult exeResults = new ExecutionResult();


            if (string.IsNullOrEmpty(tbNameSP.Text.Trim()) || string.IsNullOrEmpty(cbTypeSP.Text.Trim()) || string.IsNullOrEmpty(tbSizeSP.Text.Trim())
                || string.IsNullOrEmpty(tbPriceSP.Text.Trim()) || string.IsNullOrEmpty(tbQtySP.Text.Trim()))
            {
                controller.ErrorMSG(lbmess1, "Cần điền hết các hạng mục trước đã");
                return;
            }
            if (PictureSP.Image == null)
            {
                controller.ErrorMSG(lbmess1, "Chưa chọn ảnh của sản phẩm");
                return;
            }

            product.nameSP = tbNameSP.Text.Trim();
            product.typeSP = cbTypeSP.Text.Trim();
            product.sizeSP = tbSizeSP.Text.Trim();
            product.PriceSP = decimal.Parse(Regex.Match(tbPriceSP.Text.Trim(), @"\d+(\.\d+)?").Value);
            product.QtySP = int.Parse(tbQtySP.Text.Trim());
            product.DescSP = tbDescSP.Text.Trim();

            product.productID = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (blob3.Length != 1 && blob3[0] != 0)
            {
                File.Copy(this.pathImage, BaseDataSPInfo.ImageFolder + product.productID + this.fileExtension, true);
            }

            exeResults = controller.addProductSP(product);
            if (exeResults.Status)
            {
                controller.SuccessMSG(lbmess1, "Thêm sản phẩm thành công !");
                tbPriceSP.SelectedIndex = 0;
                cbTypeSP.SelectedIndex = 0;
                undo();
            }
            else
            {
                controller.ErrorMSG(lbmess1, exeResults.Message);
            }
        }

        private void PictureSP_DoubleClick(object sender, EventArgs e)
        {
            this.pathImage = "";
            this.fileExtension = "";
            blob3 = [0];

            ofdPhoto3.Reset();
            ofdPhoto3.Filter = "Image Files| *.jpg; *.jpeg; *.png; *.gif; *.bmp";

            if (ofdPhoto3.ShowDialog() == DialogResult.OK)
            {
                // Đọc ảnh thành byte[]
                using (FileStream stream = new FileStream(ofdPhoto3.FileName, FileMode.Open, FileAccess.Read))
                {
                    blob3 = new byte[stream.Length];
                    stream.Read(blob3, 0, (int)stream.Length);
                }
                using (var img = Image.FromFile(ofdPhoto3.FileName))
                {
                    PictureSP.Image = new Bitmap(img); // Bản sao -> tránh lỗi khóa file
                }

                this.pathImage = ofdPhoto3.FileName;
                this.fileExtension = Path.GetExtension(ofdPhoto3.FileName);
            }
        }

        #region CheckKeyPress
        private void tbQtySP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        private void ManageThings_Load(object sender, EventArgs e)
        {
            controller.GetTypeSP(cbLoaiSPfind, 0);
            loadSPandPrice();
            controller.SuccessMSG(lbmess1, "Chúc ngày mới vui vẻ ^_^");
            controller.SuccessMSG(lbmesModify, "Chúc ngày mới vui vẻ ^_^");

        }
        public void loadSPandPrice()
        {
            string json = File.ReadAllText(BaseDataSPInfo.FileConfig);
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

            cbTypeSP.Items.Clear();
            cbTypeSP1.Items.Clear();
            List<string> TypeSP = data["TypeSP"];
            cbTypeSP.Items.Add("");
            cbTypeSP.Items.AddRange(TypeSP.ToArray());
            cbTypeSP.SelectedIndex = 0;
            cbTypeSP1.Items.Add("");
            cbTypeSP1.Items.AddRange(TypeSP.ToArray());
            cbTypeSP1.SelectedIndex = 0;

            tbPriceSP.Items.Clear();
            cbgiatheongay.Items.Clear();
            List<string> PriceSP = data["PriceSP"];
            tbPriceSP.Items.Add("");
            cbgiatheongay.Items.Add("");
            tbPriceSP.Items.AddRange(PriceSP.ToArray());
            cbgiatheongay.Items.AddRange(PriceSP.ToArray());
            tbPriceSP.SelectedIndex = 0;
            cbgiatheongay.SelectedIndex = 0;
        }

        private void btFindSP_Click(object sender, EventArgs e)
        {
            product.PriceSP = null;
            product.QtySP = null;
            product.productID = null;
            dgvProducSP.DataSource = null;
            ExecutionResult result = new ExecutionResult();
            product.nameSP = tbtensp.Text.Trim();
            product.sizeSP = tbsizesanpham.Text.Trim();
            product.typeSP = cbTypeSP1.Text.Trim();

            if (!string.IsNullOrEmpty(cbgiatheongay.Text.ToString()))
                product.PriceSP = decimal.Parse(Regex.Match(cbgiatheongay.Text.Trim(), @"\d+(\.\d+)?").Value);
            if (!string.IsNullOrEmpty(tbsoluongsp.Text.ToString()))
                product.QtySP = int.Parse(tbsoluongsp.Text.Trim());
            product.DescSP = tbmotasp.Text.Trim();

            result = controller.FindSPbyProductAll(product);
            if (result.Status)
            {
                DataSet ds = (DataSet)result.Anything;
                dgvProducSP.DataSource = ds.Tables[0];
                controller.SuccessMSG(lbmesModify, "Tìm sản phẩm thành công !");
            }
            else
            {
                controller.ErrorMSG(lbmesModify, "Không tìm thấy sản phẩm!");
            }
        }

        private void picImageModify_Click(object sender, EventArgs e)
        {
            ExecutionResult result = new ExecutionResult();
            DataSet ds = new DataSet();
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = BaseDataSPInfo.ImageFolder, // Đường dẫn mặc định
                Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tất cả (*.*)|*.*",
                Title = "Chọn ảnh muốn sửa đổi"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (var img = Image.FromFile(ofd.FileName))
                {
                    picImageModify.Image = new Bitmap(img); // Bản sao -> tránh lỗi khóa file
                }

                //picImageModify.Image = Image.FromFile(ofd.FileName); // Hiển thị ảnh lên PictureBox
                string nameproductSP = Path.GetFileNameWithoutExtension(ofd.FileName);
                product.productID = long.Parse(nameproductSP);

                result = controller.FindSPbyProductID(product);
                if (result.Status)
                {
                    ds = (DataSet)result.Anything;
                    dgvProducSP.DataSource = ds.Tables[0];
                    controller.SuccessMSG(lbmesModify, "Tìm sản phẩm thành công !");
                }
                else
                {
                    controller.ErrorMSG(lbmesModify, "Không tìm thấy sản phẩm!");
                }
            }
        }

        private void btDeleteProduct_Click(object sender, EventArgs e)
        {
            ExecutionResult result = new ExecutionResult();
            DialogResult kq = MessageBox.Show("Bạn có muốn xóa sp không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
            {
                if (dgvProducSP.SelectedRows.Count == 1)
                {
                    foreach (DataGridViewRow row in dgvProducSP.SelectedRows)
                    {
                        product.productID = long.Parse(row.Cells[0].Value?.ToString());
                        result = controller.DeleteProductSP(product);
                        if (result.Status)
                        {
                            if (picImageModify.Image != null)
                            {
                                picImageModify.Image.Dispose();
                                picImageModify.Image = null;
                            }
                            controller.DeleteImage(product.productID.ToString());
                            dgvProducSP.Rows.Remove(row);
                            undo();
                            controller.SuccessMSG(lbmesModify, "Xóa sản phẩm thành công !");
                            break;
                        }
                        else
                        {
                            controller.ErrorMSG(lbmesModify, "Xóa sản phẩm lỗi !");
                            break;
                        }
                    }

                }
                else
                    controller.ErrorMSG(lbmesModify, "Cần chọn SP cần sửa đổi hoặc xóa trước!");
            }
        }

        private void dgvProducSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                picImageModify.Image = null;
                ExecutionResult result = new ExecutionResult();
                DataGridViewRow row = dgvProducSP.Rows[e.RowIndex];
                //productid as Mã_SP,productname AS Tên_SP, size AS Size_SP,priceperday AS Giá_Thuê1_Ngày, stockquantity as Số_lượng_trong_kho,description as Mô_tả, createdate
                tbtensp.Text = row.Cells[1].Value?.ToString();
                cbTypeSP1.Text = row.Cells[2].Value?.ToString();
                tbsizesanpham.Text = row.Cells[3].Value?.ToString();
                cbgiatheongay.Text = (string)(row.Cells[4].Value + " vnđ");
                tbsoluongsp.Text = row.Cells[5].Value.ToString();
                tbmotasp.Text = row.Cells[6].Value?.ToString();

                controller.ShowImage(row.Cells[0].Value?.ToString(), picImageModify);

            }
        }

        public void undo1()
        {
            if (picImageModify.Image != null)
            {
                picImageModify.Image.Dispose();
                picImageModify.Image = null;
            }
            dgvProducSP.DataSource = null;
            tbtensp.Text = "";
            tbsizesanpham.Text = "";
            cbTypeSP1.SelectedIndex = 0;
            cbgiatheongay.SelectedIndex = 0;
            tbsoluongsp.Text = "";
            tbmotasp.Text = "";
        }
        private void btUpdateSP_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(cbgiatheongay.Text)) || (string.IsNullOrEmpty(tbsizesanpham.Text)) || (string.IsNullOrEmpty(tbsoluongsp.Text))
                || (string.IsNullOrEmpty(cbTypeSP1.Text)) || (string.IsNullOrEmpty(tbtensp.Text)))
            {
                controller.ErrorMSG(lbmesModify, "Vui lòng chọn hết các mục !");
                return;
            }
            if (dgvProducSP.SelectedRows.Count == 1)
            {
                ExecutionResult result = new ExecutionResult();
                foreach (DataGridViewRow row in dgvProducSP.SelectedRows)
                {
                    product.productID = long.Parse(row.Cells[0].Value?.ToString());
                    product.nameSP = tbtensp.Text.Trim();
                    product.sizeSP = tbsizesanpham.Text.Trim();
                    product.typeSP = cbTypeSP1.Text.Trim();
                    product.PriceSP = decimal.Parse(Regex.Match(cbgiatheongay.Text.Trim(), @"\d+(\.\d+)?").Value);
                    product.QtySP = int.Parse(tbsoluongsp.Text.Trim());
                    product.DescSP = tbmotasp.Text.Trim();

                    result = controller.UpdateProductSP(product);
                    if (result.Status)
                    {
                        undo1();
                        controller.SuccessMSG(lbmesModify, "Sửa sản phẩm thành công !");
                    }
                    else
                    {
                        controller.ErrorMSG(lbmesModify, "Sửa sản phẩm lỗi !");
                    }
                }
            }
            else
                controller.ErrorMSG(lbmesModify, "Cần chọn SP cần sửa đổi hoặc xóa trước!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            undo1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("chưa viết ^_^", null, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbNameSP_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;

            // Format chuỗi: viết hoa chữ cái đầu mỗi từ
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (cbLoaiSP.Checked)
            {
                string typeSP = cbLoaiSPfind.Text;
                var data = controller.GetTotalWH(1, rdAll, rdNotReturn, typeSP);
                ShowKhoChartCole(data);
            }
            else
            {
                var data = controller.GetTotalWH(0, rdAll, rdNotReturn, "");
                ShowKhoChart(data);
            }
        }
        public void ShowKhoChartCole(List<ProductStock> data)
        {
            var chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            // Cấu hình ChartArea
            ChartArea area = new ChartArea("MainArea");

            area.AxisY.MajorGrid.LineWidth = 0;
            area.AxisX.MajorGrid.LineWidth = 0;

            area.AxisX.Title = "Loại sản phẩm";
            area.AxisY.Title = "Số lượng tồn kho";

            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            // Tạo series cho biểu đồ tồn kho
            Series series = new Series("Tồn kho")
            {
                ChartType = chartType,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                BorderWidth = 2
            };
            series["PointWidth"] = "0.3";

            var groupedData = data
                .GroupBy(x => x.TypeProduction)
                .OrderBy(g => g.Key);

            // Danh sách màu sắc để gán cho từng cột
            List<Color> colors = new List<Color>
    {
        Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Purple,
        Color.Orange, Color.Cyan, Color.Magenta, Color.Brown, Color.Gray
    };

            int colorIndex = 0;

            foreach (var group in groupedData)
            {
                int quantity = group.Sum(x => x.TotalQuantity);
                int index = series.Points.AddXY(group.Key, quantity);
                series.Points[index].Label = quantity.ToString();

                // Gán màu cho mỗi cột
                series.Points[index].Color = colors[colorIndex % colors.Count];

                // Tăng chỉ số màu
                colorIndex++;
            }

            // Thêm series vào chart
            chart.Series.Add(series);

            // Hiển thị biểu đồ lên form
            groupBox3.Controls.Clear();
            groupBox3.Controls.Add(chart);
        }

        public void ShowKhoChart(List<ProductStock> data)
        {
            var chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            ChartArea area = new ChartArea("MainArea");

            area.AxisY.MajorGrid.LineWidth = 0;
            area.AxisX.MajorGrid.LineWidth = 0;

            area.AxisX.Title = "Loại sản phẩm";
            area.AxisY.Title = "Số lượng tồn kho";

            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            Series series = new Series("Tồn kho")
            {
                ChartType = chartType,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                BorderWidth = 2
            };
            series["PointWidth"] = "0.3";
            var groupedData = data
                .GroupBy(x => x.TypeProduction)
                .OrderBy(g => g.Key);

            foreach (var group in groupedData)
            {
                int quantity = group.Sum(x => x.TotalQuantity);
                int index = series.Points.AddXY(group.Key, quantity);
                series.Points[index].Label = quantity.ToString();
            }

            chart.Series.Add(series);

            // Hiển thị lên form
            groupBox3.Controls.Clear();
            groupBox3.Controls.Add(chart);
        }

        private void tbPriceByday_Enter(object sender, EventArgs e)
        {
            if (tbPriceByday.Text.EndsWith(" Vnđ"))
            {
                tbPriceByday.Text = tbPriceByday.Text.Replace(" Vnđ", "").Trim();
            }
        }
        private void tbPriceByday_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbPriceByday_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPriceByday.Text))
            {
                tbPriceByday.Text = "0 Vnđ";
            }
            else
            {
                tbPriceByday.Text = $"{decimal.Parse(tbPriceByday.Text.Replace(" Vnđ", "").Replace(",", "").Trim()):N0} Vnđ";
            }
        }

        private void tbPriceByday_TextChanged(object sender, EventArgs e)
        {
            string text = tbPriceByday.Text.Replace(" Vnđ", "").Trim();

            if (!decimal.TryParse(text, out _))
            {
                tbPriceByday.Text = "0";
            }
        }

        private void cbAddNewSP_CheckedChanged(object sender, EventArgs e)
        {
            tbTypeSP.Visible = cbAddNewSP.Checked;
            btAddTypeSP.Visible = cbAddNewSP.Checked;
            if (!cbAddNewSP.Checked)
                tbTypeSP.Text = "";
        }

        private void cbAddNewPrice_CheckedChanged(object sender, EventArgs e)
        {
            tbPriceByday.Visible = cbAddNewPrice.Checked;
            btAddPrice.Visible = cbAddNewPrice.Checked;
            if (!cbAddNewPrice.Checked)
                tbPriceByday.Text = "";
        }

        string filePath = BaseDataSPInfo.FileConfig;

        ProductData ReadData()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return System.Text.Json.JsonSerializer.Deserialize<ProductData>(json) ?? new ProductData();
            }

            return new ProductData();
        }

        void WriteData(ProductData data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = System.Text.Json.JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }

        private void btAddTypeSP_Click(object sender, EventArgs e)
        {
            string newType = tbTypeSP.Text.Trim();
            if (string.IsNullOrEmpty(newType))
            {
                controller.ErrorMSG(lbmess1, "Vui lòng nhập loại sản phẩm");
                return;
            }

            var data = ReadData();

            if (!data.TypeSP.Any(t => t.ToUpper() == newType.ToUpper()))
            {
                data.TypeSP.Add(newType);
                WriteData(data);
                controller.SuccessMSG(lbmess1, "Thêm loại sản phẩm thành công!");
                loadSPandPrice();
                tbTypeSP.Text = "";
                tbTypeSP.Focus();
            }
            else
            {
                controller.ErrorMSG(lbmess1, "Loại sản phẩm đã tồn tại!");
                tbTypeSP.SelectAll();
                tbTypeSP.Focus();
            }
        }

        private void btAddPrice_Click(object sender, EventArgs e)
        {
            string newPrice = tbPriceByday.Text.Trim();
            if (string.IsNullOrEmpty(newPrice))
            {
                controller.ErrorMSG(lbmess1, "Vui lòng nhập giá sản phẩm");
                return;
            }

            var data = ReadData();

            if (!data.PriceSP.Any(p => p.ToUpper() == newPrice.ToUpper()))
            {
                data.PriceSP.Add(newPrice);
                WriteData(data);
                controller.SuccessMSG(lbmess1, "Thêm giá sản phẩm thành công!");
                loadSPandPrice();
                tbPriceByday.Text = "";
                tbPriceByday.Focus();
            }
            else
            {
                controller.ErrorMSG(lbmess1, "Giá sản phẩm đã tồn tại!!");
                tbPriceByday.SelectAll();
                tbPriceByday.Focus();
            }
        }

        private void tbTypeSP_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;

            // Format chuỗi: viết hoa chữ cái đầu mỗi từ
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }

        private void btDeleteTypeSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbTypeSP.Text.Trim()))
            {
                controller.ErrorMSG(lbmess1, "Chọn loại sản phẩm cần xóa!");
                return;
            }
            var data = ReadData();
            string deleteType = cbTypeSP.Text.Trim();

            var matchedType = data.TypeSP.FirstOrDefault(t => t.ToUpper() == deleteType.ToUpper());

            if (matchedType != null)
            {
                data.TypeSP.Remove(matchedType);
                WriteData(data);
                controller.SuccessMSG(lbmess1, "Đã xóa loại sản phẩm khỏi danh sách!");
                loadSPandPrice();
            }
            else
            {
                controller.ErrorMSG(lbmess1, "Không tìm thấy loại sản phẩm cần xóa!");
            }
        }

        private void btDeletePrice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbPriceSP.Text.Trim()))
            {
                controller.ErrorMSG(lbmess1, "Chọn giá sản phẩm cần xóa!");
                return;
            }
            var data = ReadData();
            string deleteType = tbPriceSP.Text.Trim();

            var matchedType = data.PriceSP.FirstOrDefault(t => t.ToUpper() == deleteType.ToUpper());

            if (matchedType != null)
            {
                data.PriceSP.Remove(matchedType);
                WriteData(data);
                controller.SuccessMSG(lbmess1, "Đã xóa giá sản phẩm khỏi danh sách!");
                loadSPandPrice();
            }
            else
            {
                controller.ErrorMSG(lbmess1, "Không tìm thấy giá sản phẩm cần xóa!");
            }
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
