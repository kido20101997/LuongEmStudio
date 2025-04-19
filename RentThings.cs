using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using Npgsql.Internal;
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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LuongEmStudio
{
    public partial class RentThings : Form
    {
        private Controller controller;
        BaseDataBorrowReturn baseDataBorrowReturn;
        private string producID = "";
        public RentThings()
        {
            InitializeComponent();
            controller = new Controller();
            baseDataBorrowReturn = new BaseDataBorrowReturn();
        }
        public void undo()
        {
            SelectedProductInfo.ClearProductInfo();
            picSP.Image = null;
            picSP.ImageLocation = null;
            this.producID = "";
            tbDescSP.Text = "";
            tbQtySP.Text = "";
            tbPriceSP.Text = "";
            tbSizeSP.Text = "";
            cbTiencoc.SelectedIndex = 0;
            tbQTYthue.Text = "";
            cbtienphatsinh.SelectedIndex = 0;
        }
        private void cbTypeSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecutionResult exeResults = new ExecutionResult();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            undo();

            dt.Columns.Add("ImagePath");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("productid");
            dt.Columns.Add("productname");
            dt.Columns.Add("description");
            dt.Columns.Add("priceperday");
            dt.Columns.Add("size");
            dt.Columns.Add("stockquantity");
            dt.Columns.Add("saveqty");

            exeResults = controller.GetPictureByTypeSP(cbTypeSP.Text);
            ds = (DataSet)exeResults.Anything;
            if (exeResults.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(BaseDataSPInfo.ImageFolder + ds.Tables[0].Rows[i]["productid"].ToString() + @".jpg",
                        "Giá:" + ds.Tables[0].Rows[i]["priceperday"].ToString() + ",Size:" + ds.Tables[0].Rows[i]["size"].ToString(),
                        ds.Tables[0].Rows[i]["productid"].ToString(), ds.Tables[0].Rows[i]["productname"].ToString(),
                        ds.Tables[0].Rows[i]["description"].ToString(), ds.Tables[0].Rows[i]["priceperday"].ToString(),
                        ds.Tables[0].Rows[i]["size"].ToString(), ds.Tables[0].Rows[i]["stockquantity"].ToString(), ds.Tables[0].Rows[i]["saveqty"].ToString());
                }

                var galleryForm = new RentalItemsGalleryForm(dt);
                galleryForm.ShowDialog();
                if (galleryForm.DialogResult == DialogResult.OK)
                {
                    this.producID = SelectedProductInfo.ProductId;
                    tbDescSP.Text = SelectedProductInfo.Description;
                    tbQtySP.Text = SelectedProductInfo.stockquantity;
                    tbPriceSP.Text = int.Parse(SelectedProductInfo.PricePerDay.Replace(".", "")).ToString("N0") + " vnđ";
                    tbSizeSP.Text = SelectedProductInfo.Size;
                    picSP.Image = Image.FromFile(SelectedProductInfo.imagePath);
                    controller.SuccessMSG(lbmess1, "Chọn đồ thành công");
                    lbqtyinwh.Text = $"Số lượng còn {SelectedProductInfo.QTYinWH}";
                }
            }
            else
                controller.ErrorMSG(lbmess1, "Buồn quá không còn hàng tồn trong kho rồi");
        }

        private void RentThings_Load(object sender, EventArgs e)
        {
            controller.GetTypeSP(cbTypeSP, 0);
            tbNameKH.Focus();
        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNameKH.Text) || string.IsNullOrEmpty(tbContactKH.Text) || string.IsNullOrEmpty(cbTypeSP.Text) || string.IsNullOrEmpty(tbQTYthue.Text) || string.IsNullOrEmpty(cbTiencoc.Text) || string.IsNullOrEmpty(cbtienphatsinh.Text))
            {
                controller.ErrorMSG(lbmess1, "Vui lòng điền đầy đủ thông tin trước đã");
                return;
            }
            string notes = tbNotes.Text.Trim();
            string typeSP = cbTypeSP.Text;
            string Tiencoc = cbTiencoc.Text;
            string QTYthue = tbQTYthue.Text;
            string priceperday = tbPriceSP.Text;
            string size = tbSizeSP.Text;

            if (int.Parse(QTYthue) == 0)
            {
                controller.ErrorMSG(lbmess1, "Điền lại cái số lượng khách thuê phát");
                tbQTYthue.SelectAll(); tbQTYthue.Focus();
                return;
            }

            tbtotaltien.Text = ((int.Parse(Tiencoc.Replace("vnđ", "").Replace(",", "").Trim()) + int.Parse(priceperday.Replace("vnđ", "").Replace(",", "").Trim())) * int.Parse(QTYthue)).ToString("N0") + " vnđ";

            tbTongtiencoc.Text = (int.Parse(Tiencoc.Replace("vnđ", "").Replace(",", "").Trim()) * int.Parse(QTYthue)).ToString("N0") + " vnđ";

            tbTongtienthue.Text = (int.Parse(priceperday.Replace("vnđ", "").Replace(",", "").Trim()) * int.Parse(QTYthue)).ToString("N0") + " vnđ";

            string addlist = typeSP + "-" + Tiencoc + "-" + QTYthue + "-" + notes + "-" + priceperday + "-" + size + "-" + this.producID + "-" + cbtienphatsinh.Text + "-" + tbtotaltien.Text;

            lblistsp.Items.Add(addlist);

            TinhTongTien();

            tbNameKH.ReadOnly = true;
            tbContactKH.ReadOnly = true;
            controller.SuccessMSG(lbmess1, "Thêm vào danh sách đồ thuê ok");
            undo();
        }
        private void TinhTongTien()
        {
            int tongTien = 0;
            int tongTienCoc = 0;
            int tongTienThue = 0;

            foreach (var item in lblistsp.Items)
            {
                string[] parts = item.ToString().Split('-');
                if (parts.Length >= 6)
                {
                    string tienCocStr = parts[1].Replace("vnđ", "").Replace(",", "").Trim();
                    string soLuongStr = parts[2].Trim();
                    string giaThueStr = parts[4].Replace("vnđ", "").Replace(",", "").Trim();

                    if (int.TryParse(tienCocStr, out int tienCoc) &&
                        int.TryParse(giaThueStr, out int giaThue) &&
                        int.TryParse(soLuongStr, out int soLuong))
                    {
                        tongTienCoc += tienCoc * soLuong;
                        tongTienThue += giaThue * soLuong;
                    }
                }
            }

            tongTien = tongTienCoc + tongTienThue;

            tbtotaltien.Text = tongTien.ToString("N0") + " vnđ";
            tbTongtiencoc.Text = tongTienCoc.ToString("N0") + " vnđ";
            tbTongtienthue.Text = tongTienThue.ToString("N0") + " vnđ";
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btthue_Click(object sender, EventArgs e)
        {
            if (lblistsp.Items.Count > 0)
            {
                //string addlist = typeSP + "-" + Tiencoc + "-" + QTYthue + "-" + notes + "-" + priceperday + "-" + size + "-" + this.producID;
                List<BaseDataBorrowReturn> danhSachDonHang = new List<BaseDataBorrowReturn>();
                foreach (var item in lblistsp.Items)
                {
                    string[] parts = item.ToString().Split('-');
                    string tienCocStr = parts[1].Replace("vnđ", "").Replace(",", "").Trim();
                    string soLuongStr = parts[2].Trim();
                    string notes = parts[3].Trim();
                    string giaThueStr = parts[4].Replace("vnđ", "").Replace(",", "").Trim();
                    string productid = parts[6].Replace("vnđ", "").Replace(",", "").Trim();

                    string tongtien = parts[8].Replace("vnđ", "").Replace(",", "").Trim();
                    string tongtienphatsinh = parts[7].Replace("vnđ", "").Replace(",", "").Trim();

                    if (int.TryParse(soLuongStr, out int soLuong) &&
                        decimal.TryParse(tongtienphatsinh, out decimal totalmoneyphatsinh) &&
                        decimal.TryParse(tongtien, out decimal totalmoney) &&
                        decimal.TryParse(tienCocStr, out decimal totalmoneycoc) &&
                        long.TryParse(productid, out long productID))
                    {
                        var baseData = new BaseDataBorrowReturn
                        {
                            TotalPrice = totalmoney,
                            Tiencoc = totalmoneycoc,
                            QTYThue = soLuong,
                            productID = productID,
                            NameKach = tbNameKH.Text.Trim(),
                            SdtKhach = tbContactKH.Text.Trim(),
                            notes = notes,
                            Tienphatsinh = totalmoneyphatsinh
                        };

                        danhSachDonHang.Add(baseData);
                    }
                }
                ExecutionResult result = controller.AddOrder(danhSachDonHang, cbkhachquen, tbContactKH.Text.Trim());
                if (result.Status)
                {
                    tbNameKH.ReadOnly = false;
                    tbContactKH.ReadOnly = false;
                    lblistsp.Items.Clear();
                    tbtotaltien.Text = "";
                    tbTongtiencoc.Text = "";
                    tbTongtienthue.Text = "";
                    tbNameKH.Text = "";
                    tbContactKH.Text = "";
                    tbNotes.Text = "";
                    undo();
                    cbkhachquen.Checked = false;
                    controller.SuccessMSG(lbmess1, "Thêm đơn thuê đồ thành công");
                }
            }
            else
            {
                controller.ErrorMSG(lbmess1, "Có gì đâu mà thuê hahaha");
            }
        }

        private void tbNameKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbNotes.Focus();
            }
        }

        private void tbContactKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string contact = tbContactKH.Text.Trim();
                if (!string.IsNullOrEmpty(contact))
                {
                    ExecutionResult exe = controller.CheckContactKH(contact);
                    DataSet ds = (DataSet)exe.Anything;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tbNameKH.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                        cbkhachquen.Checked = true;
                        tbNameKH.Focus();
                    }
                    else
                    {
                        tbNameKH.Text = "";
                        cbkhachquen.Checked = false;
                    }
                }
                tbNameKH.Focus();
            }

        }

        private void tbNameKH_TextChanged(object sender, EventArgs e)
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

        private void tbContactKH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbInfoKH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbInfoKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(tbInfoKH.Text))
                {
                    controller.ErrorMSG(lbmess1, "Nhập thông khách để tìm đơn thuê");
                    return;
                }
                ExecutionResult exeResults = controller.GetOrderNo(tbInfoKH.Text.Trim());
                DataSet ds = (DataSet)exeResults.Anything;
                if (exeResults.Status && ds.Tables[0].Rows.Count > 0)
                {
                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.HeaderText = "Không trả";
                    checkBoxColumn.Name = "checkColumn";
                    checkBoxColumn.Width = 70;
                    checkBoxColumn.TrueValue = true;
                    checkBoxColumn.FalseValue = false;

                    undo2();
                    dgvProducSP.DataSource = ds.Tables[0];
                    if (!dgvProducSP.Columns.Contains("checkColumn"))
                    {
                        dgvProducSP.Columns.Insert(0, checkBoxColumn); // Thêm vào đầu bảng
                    }

                    int tongtiencoc = 0, tongtiendathu = 0, tongTienPhatSinh = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int tienCoc = int.Parse(ds.Tables[0].Rows[i]["moneycoc"].ToString().Replace(",", "").Replace("vnđ", "").Trim());
                        int tienDaThu = int.Parse(ds.Tables[0].Rows[i]["totalamount"].ToString().Replace(",", "").Replace("vnđ", "").Trim());

                        TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                        DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                        DateTime ngayThue = Convert.ToDateTime(ds.Tables[0].Rows[i]["borrowdate"]);
                        int tienPhatSinhMotNgay = int.Parse(ds.Tables[0].Rows[i]["tienphatsinh"].ToString());

                        int soNgayThue = (gioVN.Date - ngayThue.Date).Days;
                        int soNgayPhatSinh = Math.Max(soNgayThue - 1, 0); // trừ 1 ngày đầu

                        int tienPhatSinh = soNgayPhatSinh * tienPhatSinhMotNgay;

                        tongtiencoc += tienCoc;
                        tongtiendathu += tienDaThu;
                        tongTienPhatSinh += tienPhatSinh;
                    }

                    // Tổng tiền cần trả lại = Tổng tiền cọc - Tổng tiền phát sinh
                    int tienCanTraLai = Math.Max(tongtiencoc - tongTienPhatSinh, 0);

                    tballtiencocdathu.Text = tongtiencoc.ToString("N0") + " vnđ";
                    tballTiendathu.Text = tongtiendathu.ToString("N0") + " vnđ";
                    tbtongtiencantra.Text = tienCanTraLai.ToString("N0") + " vnđ";

                    controller.SuccessMSG(lbmesModify, "Tìm đơn hàng ok:" + tbInfoKH.Text);
                }
                else
                    controller.ErrorMSG(lbmesModify, "Không tìm thấy đơn nào theo thông tin này:" + tbInfoKH.Text);
                tbInfoKH.SelectAll();
                tbInfoKH.Focus();
            }
        }

        private void dgvProducSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ExecutionResult result = new ExecutionResult();
                DataGridViewRow row = dgvProducSP.Rows[e.RowIndex];
                //producid,fullname,borrowdate,type_production,moneycoc,qty,notes,priceperday,totalamount,

                controller.ShowImage(row.Cells[1].Value?.ToString(), pictureTrado);
                tbtenkh.Text = row.Cells[2].Value?.ToString();
                if (row.Cells[3].Value != null && DateTime.TryParse(row.Cells[3].Value.ToString(), out DateTime borrowDate))
                {
                    tbdateborrow.Text = borrowDate.ToString("d/M/yyyy hh:mm:ss tt");
                }
                tbloaisp.Text = row.Cells[4].Value?.ToString();
                tbtiencoc.Text = Convert.ToDecimal(row.Cells[5].Value?.ToString()).ToString("N0") + " vnđ";
                tbslthue.Text = row.Cells[6].Value?.ToString();
                tbnotess.Text = row.Cells[7].Value?.ToString();
                tbgiathue1day.Text = Convert.ToDecimal(row.Cells[8].Value?.ToString().Replace(".", "")).ToString("N0") + " vnđ";
                tbtongtiendathu.Text = Convert.ToDecimal(row.Cells[9].Value?.ToString()).ToString("N0") + " vnđ";
                tbtienphatsinh.Text = Convert.ToDecimal(row.Cells[10].Value?.ToString()).ToString("N0") + " vnđ";

                TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
                DateTime ngayThue = Convert.ToDateTime(row.Cells[3].Value?.ToString());
                TimeSpan thoiGianThue = gioVN - ngayThue;
                tbngaydathue.Text = $"{thoiGianThue.Days} ngày {thoiGianThue.Hours} giờ {thoiGianThue.Minutes} phút";

                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvProducSP.Columns[e.ColumnIndex].Name == "checkColumn")
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvProducSP.Rows[e.RowIndex].Cells["checkColumn"];
                    bool isChecked = (checkCell.Value == null ? false : (bool)checkCell.Value);
                    checkCell.Value = !isChecked;
                    recheckMoney();
                }
            }
        }

        public void recheckMoney()
        {
            int tongtiencoc = 0, tongtiendathu = 0, tongTienPhatSinh = 0;
            foreach (DataGridViewRow row in dgvProducSP.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["checkColumn"].Value);
                if (!isChecked)
                {
                    int tienCoc = int.Parse(row.Cells["moneycoc"].Value.ToString().Trim());
                    int tienDaThu = int.Parse(row.Cells["totalamount"].Value.ToString().Trim());

                    TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

                    DateTime ngayThue = Convert.ToDateTime(row.Cells["borrowdate"].Value.ToString());
                    int tienPhatSinhMotNgay = int.Parse(row.Cells["tienphatsinh"].Value.ToString());

                    int soNgayThue = (gioVN.Date - ngayThue.Date).Days;
                    int soNgayPhatSinh = Math.Max(soNgayThue - 1, 0); // trừ 1 ngày đầu

                    int tienPhatSinh = soNgayPhatSinh * tienPhatSinhMotNgay;

                    tongtiencoc += tienCoc;
                    tongtiendathu += tienDaThu;
                    tongTienPhatSinh += tienPhatSinh;


                }
            }
            int tienCanTraLai = Math.Max(tongtiencoc - tongTienPhatSinh, 0);

            tballtiencocdathu.Text = tongtiencoc.ToString("N0") + " vnđ";
            tballTiendathu.Text = tongtiendathu.ToString("N0") + " vnđ";
            tbtongtiencantra.Text = tienCanTraLai.ToString("N0") + " vnđ";
        }
        private void btclear_Click(object sender, EventArgs e)
        {
            undo1();

        }
        public void undo2()
        {
            tbngaydathue.Text = "";
            tbtenkh.Text = "";
            tbdateborrow.Text = "";
            tbloaisp.Text = "";
            tbtiencoc.Text = "";
            tbslthue.Text = "";
            tbnotess.Text = "";
            tbgiathue1day.Text = "";
            tbtongtiendathu.Text = "";
            tbtienphatsinh.Text = "";
            pictureTrado.Image = null;
            tbtongtiencantra.Text = "";
            tballTiendathu.Text = "";
            tballtiencocdathu.Text = "";
            tbInfoKH.Focus();
        }
        public void undo1()
        {
            tbngaydathue.Text = "";
            tbtenkh.Text = "";
            tbdateborrow.Text = "";
            tbloaisp.Text = "";
            tbtiencoc.Text = "";
            tbslthue.Text = "";
            tbnotess.Text = "";
            tbgiathue1day.Text = "";
            tbtongtiendathu.Text = "";
            tbtienphatsinh.Text = "";
            pictureTrado.Image = null;
            tbtongtiencantra.Text = "";
            tballTiendathu.Text = "";
            tballtiencocdathu.Text = "";
            dgvProducSP.DataSource = null;
            tbInfoKH.Text = "";
            tbInfoKH.Focus();
        }

        private void btUpdateSP_Click(object sender, EventArgs e)
        {
            if (dgvProducSP.Rows.Count > 0 && dgvProducSP.DataSource != null)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn trả đồ cho bạn {tbtenkh.Text}", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    List<Tuple<long, int, long, decimal>> orderList = new List<Tuple<long, int, long, decimal>>();

                    foreach (DataGridViewRow row in dgvProducSP.Rows)
                    {
                        bool isChecked = Convert.ToBoolean(row.Cells["checkColumn"].Value);
                        if (!isChecked)
                        {
                            if (row.Cells["orderid"].Value != null && row.Cells["qty"].Value != null)
                            {
                                long id = long.Parse(row.Cells["orderid"].Value.ToString());
                                int sl = int.Parse(row.Cells["qty"].Value.ToString());
                                long productid = long.Parse(row.Cells["productid"].Value.ToString());

                                int cocDaThuInt = int.Parse(tballTiendathu.Text.ToString().Replace(",", "").Replace("vnđ", "").Trim());
                                int tongTienCanTraInt = int.Parse(tbtongtiencantra.Text.ToString().Replace(",", "").Replace("vnđ", "").Trim());

                                int tienLaiInt = cocDaThuInt - tongTienCanTraInt;
                                decimal tienLai = Convert.ToDecimal(tienLaiInt);

                                orderList.Add(Tuple.Create(id, sl, productid, tienLai));
                            }
                        }
                    }

                    ExecutionResult ex = controller.DoneOrder(orderList);
                    if (ex.Status)
                    {
                        controller.SuccessMSG(lbmesModify, ex.Message);
                        undo1();
                    }
                    else
                    {
                        controller.ErrorMSG(lbmesModify, ex.Message);
                    }
                }
                else
                    controller.ErrorMSG(lbmesModify, "Không có đơn nào để trả");
            }
        }

        private void tbSumBorrowReturn_Click(object sender, EventArgs e)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string status = "";
            int? moth = null;
            int year = gioVN.Year;

            if (cball.Checked)
                status = "ALL";
            if (cbreturn.Checked)
                status = "RETURN";
            if (cbNotReturn.Checked)
                status = "BORROW";
            if (cbHuy.Checked)
                status = "CANCEL";

            if (cbbyyear.Checked && !string.IsNullOrEmpty(tbyear.Text))
                year = int.Parse(tbyear.Text);

            if (cbBymoth.Checked && !string.IsNullOrEmpty(cbMoth.Text))
                moth = int.Parse(cbMoth.Text);

            dtgvBorrowReturn.DataSource = null;
            List<RentalSummary> sampleData = controller.GetSumBorrowReturn(dtgvBorrowReturn, status, year, moth);
            CreateChartByDayAndType(sampleData);
            controller.SuccessMSG(lbmesModify, "Tìm kiếm hoàn thành");
        }
        public void CreateChartByDayAndType(List<RentalSummary> dataList)
        {
            Chart chart1 = new Chart
            {
                Name = "chart1",
                Dock = DockStyle.Fill
            };

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Legends.Clear();

            ChartArea area = new ChartArea("MainArea");
            area.AxisY.MajorGrid.LineWidth = 0;
            area.AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas.Add(area);

            // === 1. Lấy danh sách ngày có mặt trong data ===
            var allDates = dataList.Select(x => x.Date.Date).Distinct().OrderBy(d => d).ToList();

            // === 2. Tổng số đơn theo ngày (cột) ===
            var totalPerDay = dataList
                .GroupBy(x => x.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(x => x.Quantity)
                }).ToDictionary(x => x.Date, x => x.Total);

            Series totalSeries = new Series("Tổng số lượt thuê")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.SteelBlue,
                BorderWidth = 1
            };
            totalSeries["PointWidth"] = "0.3";

            foreach (var date in allDates)
            {
                string label = date.ToString("dd/MM");
                int total = totalPerDay.ContainsKey(date) ? totalPerDay[date] : 0;
                totalSeries.Points.AddXY(label, total);
            }

            chart1.Series.Add(totalSeries);

            // === 3. Biểu đồ đường theo từng loại ===
            var typeGroups = dataList.GroupBy(x => x.Type);

            foreach (var group in typeGroups)
            {
                string typeName = group.Key;
                var dataByDate = group.GroupBy(x => x.Date.Date).ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                Series lineSeries = new Series(typeName)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2
                };

                foreach (var date in allDates)
                {
                    string label = date.ToString("dd/MM");
                    int value = dataByDate.ContainsKey(date) ? dataByDate[date] : 0;
                    lineSeries.Points.AddXY(label, value);
                }

                chart1.Series.Add(lineSeries);
            }

            chart1.Legends.Add(new Legend("Legend"));

            groupBox7.Controls.Clear();
            groupBox7.Controls.Add(chart1);
        }

        private void tbQTYthue_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbQTYthue_Enter(object sender, EventArgs e)
        {
        }

        private void tbQTYthue_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbTypeSP.Text))
            {
                controller.ErrorMSG(lbmess1, "Chọn Loại sản phẩm trước đã");
                return;
            }
            if (string.IsNullOrEmpty(cbTypeSP.Text)) cbTypeSP.Text = "0";

            if (int.Parse(tbQTYthue.Text) == 0) return;

            ExecutionResult ex = controller.CheckQTYinWH(cbTypeSP.Text, int.Parse(tbQTYthue.Text), long.Parse(this.producID));
            if (ex.Status && ((DataSet)ex.Anything).Tables[0].Rows.Count > 0)
            {
                controller.SuccessMSG(lbmess1, "Check số lượng trong kho ok");
            }
            else
            {
                ExecutionResult ex1 = controller.CheckQTYinWH1(cbTypeSP.Text, long.Parse(this.producID));
                controller.ErrorMSG(lbmess1, $"Số lượng tồn trong kho không đủ , số lượng trong kho còn {((DataSet)ex1.Anything).Tables[0].Rows[0]["saveqty"].ToString()}");
                tbQTYthue.SelectAll();
                tbQTYthue.Focus();
            }

        }

        private void tbyear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducSP.Rows.Count > 0 && dgvProducSP.DataSource != null)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn HỦY đơn cho bạn {tbtenkh.Text}", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    List<Tuple<long, int, long>> orderList = new List<Tuple<long, int, long>>();

                    foreach (DataGridViewRow row in dgvProducSP.Rows)
                    {
                        bool isChecked = Convert.ToBoolean(row.Cells["checkColumn"].Value);
                        if (isChecked)
                        {
                            if (row.Cells["orderid"].Value != null && row.Cells["qty"].Value != null)
                            {
                                long id = long.Parse(row.Cells["orderid"].Value.ToString());
                                int sl = int.Parse(row.Cells["qty"].Value.ToString());
                                long productid = long.Parse(row.Cells["productid"].Value.ToString());

                                orderList.Add(Tuple.Create(id, sl, productid));
                            }
                        }
                    }

                    ExecutionResult ex = controller.CancelOrder(orderList);
                    if (ex.Status)
                    {
                        controller.SuccessMSG(lbmesModify, ex.Message);
                        undo1();
                    }
                    else
                    {
                        controller.ErrorMSG(lbmesModify, ex.Message);
                    }
                }
                else
                    controller.ErrorMSG(lbmesModify, "Không có đơn nào để hủy");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabBorrow")
                tbNameKH.Focus();
            else
                tbInfoKH.Focus();
        }

        private void tbContactKH_Leave(object sender, EventArgs e)
        {
            string contact = tbContactKH.Text.Trim();
            if (!string.IsNullOrEmpty(contact))
            {
                ExecutionResult exe = controller.CheckContactKH(contact);
                DataSet ds = (DataSet)exe.Anything;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tbNameKH.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                    cbkhachquen.Checked = true;
                    tbNameKH.Focus();
                }
                else
                {
                    tbNameKH.Text = "";
                    cbkhachquen.Checked = false;
                }
            }
        }

        private void btcancel_Click(object sender, EventArgs e)
        {
            tbNameKH.ReadOnly = false;
            tbContactKH.ReadOnly = false;
            lblistsp.Items.Clear();
            tbtotaltien.Text = "";
            tbTongtiencoc.Text = "";
            tbTongtienthue.Text = "";
            tbNameKH.Text = "";
            tbContactKH.Text = "";
            tbNotes.Text = "";
            undo();
            cbkhachquen.Checked = false;
            controller.SuccessMSG(lbmess1, "Hủy lên đơn thuê đồ thành công");

        }

        private void tbContactKH_TextChanged(object sender, EventArgs e)
        {
            string contact = tbContactKH.Text.Trim();
            if (!string.IsNullOrEmpty(contact))
            {
                ExecutionResult exe = controller.CheckContactKH(contact);
                DataSet ds = (DataSet)exe.Anything;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tbNameKH.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                    cbkhachquen.Checked = true;
                }
                else
                {
                    tbNameKH.Text = "";
                    cbkhachquen.Checked = false;
                }

            }
        }
    }
}

