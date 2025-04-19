
using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using LuongEmStudio.DataGetWay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static LuongEmStudio.AddOptionMake;


namespace LuongEmStudio
{
    public partial class MakeupSchedule : Form
    {
        Controller controller;
        List<MakeUpPackage> makeupPackages = new List<MakeUpPackage>();
        BaseDataMakeInfo MakeInfo;
        public MakeupSchedule()
        {
            InitializeComponent();
            controller = new Controller();
            MakeInfo = new BaseDataMakeInfo();
        }

        private void MakeupSchedule_Load(object sender, EventArgs e)
        {
            DateMake.CustomFormat = "dd/MM/yyyy";
            HourMake.Format = DateTimePickerFormat.Time;
            HourMake.ShowUpDown = true;
            int month = DateTime.Now.Month;

            groupBox3.Text = "Lịch make-up tháng " + month;
            groupBox2.Text = "Lịch make-up tháng " + month;

            controller.LoadMakeBookings(dtgvLichmake, month);
            loadGoiMakeup();
            ExecutionResult result = controller.SearchStaff("", "");
            if (result.Status)
            {
                DataSet ds = (DataSet)result.Anything;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    cbNV1.Items.Add(row["namestaff"].ToString() + "                                                            | " + row["staffid"].ToString());
                    cbNvMake.Items.Add(row["namestaff"].ToString() + "                                                         | " + row["staffid"].ToString());
                }
            }
            else
                result.Message = "Không tìm thấy nv nào như trên";
            changesize(dtgvLichmake, groupBox2);
        }

        public void loadGoiMakeup()
        {
            makeupPackages.Clear();
            cbgiagoimake.Items.Clear();
            string filePath = Path.Combine(BaseDataSPInfo.MakeUpOption, "MakeUpOption.json");
            if (!Directory.Exists(BaseDataSPInfo.MakeUpOption))
            {
                Directory.CreateDirectory(BaseDataSPInfo.MakeUpOption);
            }

            if (!File.Exists(filePath))
            {
                List<MakeUpPackage> defaultData = new List<MakeUpPackage> {
                    new MakeUpPackage {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Cô dâu",
                        GiaMake = "800,000",
                        ChiTietMake = "Trang điểm cô dâu chuyên nghiệp" }
                };

                string defaultJson = JsonConvert.SerializeObject(defaultData, Formatting.Indented);
                File.WriteAllText(filePath, defaultJson);
            }
            string json = File.ReadAllText(filePath);

            List<MakeUpPackage> data = JsonConvert.DeserializeObject<List<MakeUpPackage>>(json);

            if (data != null)
            {
                foreach (var item in data)
                {
                    cbgiagoimake.Items.Add($" {item.GiaMake} Vnđ  | {item.Name}                                                  |{item.Id}");
                }
            }

            if (File.Exists(filePath))
            {
                makeupPackages = JsonConvert.DeserializeObject<List<MakeUpPackage>>(json);
            }

        }
        private void btAdd_Click(object sender, EventArgs e)
        {
            ExecutionResult result = new ExecutionResult();

            controller.CloseOrderDuoDate();
            if (string.IsNullOrEmpty(cbgiagoimake.Text) || string.IsNullOrEmpty(tbNameKhach.Text) || string.IsNullOrEmpty(tbSdtKhach.Text) || string.IsNullOrEmpty(tbQTYkhach.Text)
            || string.IsNullOrEmpty(tbTiencoc.Text) || string.IsNullOrEmpty(cbNvMake.Text) || string.IsNullOrEmpty(tbLocationMake.Text) || string.IsNullOrEmpty(cbTimeUocTinh.Text))
            {
                controller.ErrorMSG(lbmes, "Vui lòng điền đầy đủ thông tin trước");
                return;
            }

            int hourClose = int.Parse(cbTimeUocTinh.Text.Split(" ")[0].ToString());

            DateTime SCheduleMake = DateMake.Value.Date + HourMake.Value.TimeOfDay;
            DateTime timeClose = SCheduleMake.AddHours(hourClose);

            string IDNVMake = cbNvMake.Text.Split('|')[1].Trim();

            if (btAdd.Text == "    Thêm Lịch")
            {
                result = controller.CheckDupScheduleMake(SCheduleMake, IDNVMake); //check xem có trùng lịch không
                if (!result.Status)
                {
                    controller.ErrorMSG(lbmes, result.Message);
                    return;
                }
            }

            if (int.Parse(tbQTYkhach.Text) == 0)
            {
                controller.ErrorMSG(lbmes, "Vui lòng điền số lượng khách make-up trước");
                tbQTYkhach.SelectAll();
                tbQTYkhach.Focus();
                return;
            }

            MakeInfo.GoiMake = cbgiagoimake.Text.Split('|')[0].Replace("Vnđ", "").Trim();
            MakeInfo.IDNVMake = cbNvMake.Text.Split('|')[1].Trim();
            MakeInfo.NameKach = tbNameKhach.Text.Trim();
            MakeInfo.SdtKhach = tbSdtKhach.Text.Trim();
            MakeInfo.Tiencoc = tbTiencoc.Text.Replace(",", "").Replace("Vnđ", "").Trim();
            MakeInfo.BookingID = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            MakeInfo.LocationMake = tbLocationMake.Text.Trim();
            MakeInfo.QTYkhach = int.Parse(tbQTYkhach.Text.Trim());

            MakeInfo.TotalPrice = decimal.Parse(MakeInfo.Tiencoc) + decimal.Parse(MakeInfo.GoiMake);
            MakeInfo.NameBooking = cbgiagoimake.Text.Split('|')[1].Trim();

            if (int.Parse(MakeInfo.Tiencoc) == 0)
            {
                var result1 = MessageBox.Show("Bạn có chắc không muốn thu tiền cọc?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result1 == DialogResult.No)
                {
                    tbTiencoc.Focus();
                    return;
                }
            }

            if (btAdd.Text == "    Thêm Lịch")
                result = controller.InsertMakeSchedule(MakeInfo, SCheduleMake, cbkhachquen, timeClose);
            else
            {
                string userID = ((DataSet)controller.GetforUpdate(BaseDataMakeInfo.IDlichMake).Anything).Tables[0].Rows[0]["userid"].ToString();
                result = controller.UpdateMakeSchedule(MakeInfo, SCheduleMake, userID, timeClose);
                if (result.Status)
                {
                    controller.SuccessMSG(lbmes, result.Message);
                    btAdd.Text = "    Thêm Lịch";
                    btModifyLichMake.Visible = false;
                    int month = DateTime.Now.Month;
                    dtgvLichmake.Rows.Clear();
                    dtgvLichmake.Columns.Clear();
                    controller.LoadMakeBookings(dtgvLichmake, month);
                    changesize(dtgvLichmake, groupBox2);
                    undo();
                }
            }

            if (result.Status)
            {
                controller.SuccessMSG(lbmes, result.Message);
                btUpdateOrderDuoDate_Click(sender, e);
                undo();
                cbkhachquen.Checked = false;
            }
            else
            {
                controller.ErrorMSG(lbmes, result.Message);
            }
        }
        public void undo()
        {
            cbgiagoimake.Text = null;
            cbNvMake.Text = null;
            tbNameKhach.Text = null;
            tbSdtKhach.Text = null;
            tbLocationMake.Text = null;
            cbgiagoimake.Text = null;
            tbChiTietMake.Text = null;
            cbTimeUocTinh.SelectedIndex = 0;
        }

        private void btQuery_Click(object sender, EventArgs e)
        {
            int month = DateTime.Now.Month;
            dtgvLichmake.Rows.Clear();
            dtgvLichmake.Columns.Clear();
            controller.LoadMakeBookings(dtgvLichmake, month);

        }

        private void dtgvLichmake_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dtgvLichmake.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddOptionMake addOptionMake = new AddOptionMake();
            addOptionMake.ShowDialog();
            if (addOptionMake.DialogResult == DialogResult.OK)
            {
                loadGoiMakeup();
            }
        }

        private void TabControlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = DateTime.Now.Month;
            controller.CloseOrderDuoDate();
            if (TabControlMake.SelectedTab.Name == "TabBooking")
            {
                dtgvLichmake.Rows.Clear();
                dtgvLichmake.Columns.Clear();
                controller.LoadMakeBookings(dtgvLichmake, month);
                changesize(dtgvLichmake, groupBox2);
            }
            else
            {
                dtgvModify.Rows.Clear();
                dtgvModify.Columns.Clear();
                controller.LoadMakeBookings(dtgvModify, month);
                changesize(dtgvModify, groupBox3);
            }
        }

        private void cbgiatheongay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbgiagoimake.SelectedIndex != -1)
            {
                tbChiTietMake.Text = "";
                //loadGoiMakeup();

                string selectedText = cbgiagoimake.SelectedItem.ToString();
                string selectedID = selectedText.Split('|')[2].Trim();
                MakeUpPackage selectedPackage = makeupPackages.FirstOrDefault(x => x.Id == selectedID);

                if (selectedPackage != null)
                {
                    tbChiTietMake.Text = selectedPackage.ChiTietMake;
                }
                else
                {
                    tbChiTietMake.Text = "Không tìm thấy thông tin.";
                }
            }
        }

        private void btDelGoiMake_Click(object sender, EventArgs e)
        {
            if (cbgiagoimake.SelectedIndex != -1)
            {
                string selectedText = cbgiagoimake.SelectedItem.ToString();
                string selectedID = selectedText.Split('|')[2].Trim();

                var result = MessageBox.Show("Bạn có chắc muốn xóa gói Makeup này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteMakeupPackage(selectedID);
                    loadGoiMakeup();
                    tbChiTietMake.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn gói Makeup để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteMakeupPackage(string idToDelete)
        {
            string filePath = Path.Combine(BaseDataSPInfo.MakeUpOption, "MakeUpOption.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<MakeUpPackage> makeupPackages = JsonConvert.DeserializeObject<List<MakeUpPackage>>(json);

                // Tìm và xóa gói có ID tương ứng
                MakeUpPackage packageToRemove = makeupPackages.FirstOrDefault(p => p.Id == idToDelete);
                if (packageToRemove != null)
                {
                    makeupPackages.Remove(packageToRemove);

                    // Ghi lại danh sách mới vào file JSON
                    string updatedJson = JsonConvert.SerializeObject(makeupPackages, Formatting.Indented);
                    File.WriteAllText(filePath, updatedJson);

                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy gói Makeup!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("File dữ liệu không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox2_SizeChanged(object sender, EventArgs e)
        {
            changesize(dtgvLichmake, groupBox2);
        }
        public void changesize(DataGridView vdtgv, GroupBox vgroupBox)
        {
            vdtgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            int rowCount = vdtgv.RowCount + 1;

            if (rowCount > 0)
            {
                int newRowHeight = (vgroupBox.Height - vdtgv.ColumnHeadersHeight) / rowCount;
                foreach (DataGridViewRow row in vdtgv.Rows)
                {
                    row.Height = newRowHeight;
                }

            }
        }

        private void tbTiencoc_TextChanged(object sender, EventArgs e)
        {
            string text = tbTiencoc.Text.Replace(" Vnđ", "").Trim();

            if (!decimal.TryParse(text, out _))
            {
                tbTiencoc.Text = "0";
            }
        }

        private void tbTiencoc_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTiencoc.Text))
            {
                tbTiencoc.Text = "0 Vnđ";
            }
            else
            {
                tbTiencoc.Text = $"{decimal.Parse(tbTiencoc.Text.Replace(" Vnđ", "").Replace(",", "").Trim()):N0} Vnđ";
            }
        }

        private void tbTiencoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void tbTiencoc_Enter(object sender, EventArgs e)
        {
            if (tbTiencoc.Text.EndsWith(" Vnđ"))
            {
                tbTiencoc.Text = tbTiencoc.Text.Replace(" Vnđ", "").Trim();
            }
        }

        private void dtgvLichmake_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is ShowDetailScheduleMake)
                {
                    openForm.Close();
                    break;
                }
            }

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            string value = dtgvLichmake.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            string day = value.Split(' ')[0];

            var parts = value.Split(' ');
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
                return;
            if (day != null && int.TryParse(day.ToString(), out int days))
            {
                try
                {
                    DateTime fullDate = new DateTime(year, month, days);

                    ShowDetailScheduleMake childForm = new ShowDetailScheduleMake(fullDate);
                    childForm.ShowDialog();
                    if (childForm.DialogResult == DialogResult.OK && !string.IsNullOrWhiteSpace(BaseDataMakeInfo.IDlichMake))
                    {
                        ExecutionResult ex1 = controller.GetScheduleMakeByDatetimeByID(BaseDataMakeInfo.IDlichMake);

                        DataSet dataSet = (DataSet)ex1.Anything;

                        string input = dataSet.Tables[0].Rows[0]["scheduleddate"].ToString();
                        DateTime dt1 = DateTime.Parse(input);

                        DateMake.Text = dt1.ToShortDateString();
                        HourMake.Text = dt1.ToString("HH:mm");

                        tbLocationMake.Text = dataSet.Tables[0].Rows[0]["locationmake"].ToString();
                        tbNameKhach.Text = dataSet.Tables[0].Rows[0]["fullname"].ToString();
                        tbSdtKhach.Text = dataSet.Tables[0].Rows[0]["facebookphone"].ToString();
                        tbTiencoc.Text = dataSet.Tables[0].Rows[0]["moneycoc"].ToString();
                        tbQTYkhach.Text = dataSet.Tables[0].Rows[0]["qty"].ToString();

                        if (string.IsNullOrWhiteSpace(tbTiencoc.Text))
                        {
                            tbTiencoc.Text = "0 Vnđ";
                        }
                        else
                        {
                            tbTiencoc.Text = $"{decimal.Parse(tbTiencoc.Text.Replace(" Vnđ", "").Replace(",", "").Trim()):N0} Vnđ";
                        }

                        btModifyLichMake.Visible = true;
                        btAdd.Text = "      Cập Nhật Lịch";
                        controller.SuccessMSG(lbmes, "Ok có thể sửa và cập nhật lại lịch");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ngày không hợp lệ: " + ex.Message);
                }
            }
        }

        private void tbLocationMake_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tbSdtKhach.Focus();
        }

        private void tbNameKhach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tbTiencoc.Focus();
        }

        private void tbTiencoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tbSdtKhach.Focus();
        }

        private void btUpdateOrderDuoDate_Click(object sender, EventArgs e)
        {
            controller.CloseOrderDuoDate();
            int month = DateTime.Now.Month;
            dtgvLichmake.Rows.Clear();
            dtgvLichmake.Columns.Clear();
            controller.LoadMakeBookings(dtgvLichmake, month);
            changesize(dtgvLichmake, groupBox2);
        }

        private void btModifyLichMake_Click(object sender, EventArgs e)
        {
            btModifyLichMake.Visible = false;
            undo();
            cbkhachquen.Enabled = false;
            controller.SuccessMSG(lbmes, "Hủy sửa lịch makeup thành công !");
            btAdd.Text = "    Thêm Lịch";
        }

        private void groupBox3_SizeChanged(object sender, EventArgs e)
        {
            changesize(dtgvModify, groupBox3);
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            int month1 = DateTime.Now.Month;
            dataGridView2.DataSource = null;
            int? month = null;
            string? IDnv = null;
            string status = "";
            int year = DateTime.Now.Year;

            if (rdAll.Checked)
                status = "ALL";
            if (rdNotDone.Checked)
                status = "OPEN";
            if (rdDone.Checked)
                status = "CLOSE";
            if (rdCancel.Checked)
                status = "CANCEL";

            if (cbBymoth.Checked && !string.IsNullOrEmpty(cbMoth.Text))
            {
                month = int.Parse(cbMoth.Text);
                month1 = int.Parse(cbMoth.Text);
            }
            if (cbNV.Checked && !string.IsNullOrEmpty(cbNV1.Text))
                IDnv = cbNV1.Text = cbNV1.Text.Split('|')[1].Trim();

            dtgvModify.Rows.Clear();
            dtgvModify.Columns.Clear();
            controller.LoadMakeBookings(dtgvModify, month1);
            groupBox3.Text = "Lịch make-up tháng " + month1;
            changesize(dtgvModify, groupBox3);

            ExecutionResult result = new ExecutionResult();
            List<BookingData> sampleData = controller.GetScheduleMakeByDatetimeByID(dataGridView2, status, year, month, IDnv);


            CreateChartByStaffAndTotal(sampleData);
        }
        public void CreateChartByStaffAndTotal(List<BookingData> dataList)
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

            // === 1. Lấy danh sách ngày duy nhất để chuẩn hóa trục X ===
            var allDates = dataList.Select(x => x.Date.Date).Distinct().OrderBy(d => d).ToList();

            // === 2. Tổng số đơn theo ngày (biểu đồ cột) ===
            var totalPerDay = dataList
                .GroupBy(x => x.Date.Date)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Count));

            Series totalSeries = new Series("Tổng đơn")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Green,
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

            // === 3. Biểu đồ đường theo từng nhân viên ===
            var staffGroups = dataList.GroupBy(x => x.StaffName);
            foreach (var group in staffGroups)
            {
                string staffName = group.Key;
                var dataByDate = group
                    .GroupBy(x => x.Date.Date)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Count));

                Series lineSeries = new Series(staffName)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2
                };

                foreach (var date in allDates)
                {
                    string label = date.ToString("dd/MM");
                    int count = dataByDate.ContainsKey(date) ? dataByDate[date] : 0;
                    lineSeries.Points.AddXY(label, count);
                }

                chart1.Series.Add(lineSeries);
            }

            // === 4. Legend
            chart1.Legends.Add(new Legend("Legend"));

            groupBox4.Controls.Clear();
            groupBox4.Controls.Add(chart1);
        }

        private void tbLocationMake_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }

        private void tbNameKhach_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int selectionStart = tb.SelectionStart;
            string formatted = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text.ToLower());

            if (tb.Text != formatted)
            {
                tb.Text = formatted;
                tb.SelectionStart = selectionStart;
            }
        }

        private void tbSdtKhach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string contact = tbSdtKhach.Text.Trim();
                if (!string.IsNullOrEmpty(contact))
                {
                    ExecutionResult exe = controller.CheckContactKH(contact);
                    DataSet ds = (DataSet)exe.Anything;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tbNameKhach.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                        cbkhachquen.Checked = true;
                    }
                    else
                    {
                        tbNameKhach.Text = "";
                        cbkhachquen.Checked = false;
                    }
                }
                tbTiencoc.Focus();
            }
        }

        private void tbSdtKhach_Leave(object sender, EventArgs e)
        {
            string contact = tbSdtKhach.Text.Trim();
            if (!string.IsNullOrEmpty(contact))
            {
                ExecutionResult exe = controller.CheckContactKH(contact);
                DataSet ds = (DataSet)exe.Anything;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tbNameKhach.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                    cbkhachquen.Checked = true;
                }
                else
                {
                    tbNameKhach.Text = "";
                    cbkhachquen.Checked = false;
                }
                tbTiencoc.Focus();
            }
        }

        private void tbQTYthue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbNvMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbQTYkhach.Focus();
        }

        private void tbQTYkhach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tbLocationMake.Focus();
            }
        }

        private void tbSdtKhach_TextChanged(object sender, EventArgs e)
        {
            string contact = tbSdtKhach.Text.Trim();
            if (!string.IsNullOrEmpty(contact))
            {
                ExecutionResult exe = controller.CheckContactKH(contact);
                DataSet ds = (DataSet)exe.Anything;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tbNameKhach.Text = ds.Tables[0].Rows[0]["fullname"].ToString();
                    cbkhachquen.Checked = true;
                }
                else
                {
                    tbNameKhach.Text = "";
                    cbkhachquen.Checked = false;
                }    

            }
        }
    }
}
