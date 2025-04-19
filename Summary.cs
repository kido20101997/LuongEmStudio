using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace LuongEmStudio
{
    public partial class Summary : Form
    {
        Controller controller;
        public Summary()
        {
            InitializeComponent();
            controller = new Controller();
        }

        private void Summary_Load(object sender, EventArgs e)
        {
            controller.GetTypeSP(cbTypeSP, 1);
            tbNam.Text = DateTime.Now.Year.ToString();
            tbtheoyear.Text = DateTime.Now.Year.ToString();
            cldNgay.SelectionStart = DateTime.Today;
            cldmake.SelectionStart = DateTime.Today;

            ExecutionResult result = controller.SearchStaff("", "");
            DataSet ds = (DataSet)result.Anything;
            List<string> list = ds.Tables[0]
                .AsEnumerable()
                .Select(row => row["namestaff"].ToString() + "                         | " + row["staffid"].ToString())
                .OrderBy(x => x)
                .ToList();
            cbNV.Items.Clear();
            list.Insert(0, "All_NV");
            cbNV.Items.AddRange(list.ToArray());
            cbNV.SelectedIndex = 0;
        }
        private void tbNam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void tbtheoyear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void btFindThuedo_Click(object sender, EventArgs e)
        {
            List<RevenueRecord> sampleData = new List<RevenueRecord>();
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string typeSP = "", type = "";
            int? moth = null, day = null;
            int year = gioVN.Year;

            if (cbtheonam.Checked && !string.IsNullOrEmpty(tbNam.Text))
            {
                year = int.Parse(tbNam.Text);
                type = "year";
            }

            if (cbtheothang.Checked && !string.IsNullOrEmpty(cbthang.Text))
            {
                moth = int.Parse(cbthang.Text);
                type = "month";
            }

            if (cbtheongay.Checked)
            {
                DateTime selectedDate = cldNgay.SelectionStart;
                day = selectedDate.Day;
                type = "day";
            }

            if (cbLoaiSP.Checked)
            {
                typeSP = cbTypeSP.Text;
                if (typeSP == "All_SP")
                    typeSP = "";

                sampleData = controller.GetDoanhThuThueDoUocTinh(cbDTuoctinh, typeSP, year, moth, day);

                if (cbtheothang.Checked)
                {
                    var dataTheoThang = sampleData
                                       .Where(x => x.RentalDate.Month == moth && x.RentalDate.Year == year)
                                       .ToList();
                    ShowThuedoTheoThang(dataTheoThang);
                }
                else if (cbtheonam.Checked)
                {
                    var dataTheoNam = sampleData
                                     .Where(x => x.RentalDate.Year == year)
                                     .ToList();
                    ShowThuedoTheoNam(dataTheoNam);
                }
                else
                    ShowThuedoTheoNgay(sampleData, type);
            }
            else
            {
                typeSP = "";
                sampleData = controller.GetDoanhThuThueDoUocTinh(cbDTuoctinh, typeSP, year, moth, day);

                if (cbtheonam.Checked)
                {
                    ShowDoanhThuTheoNam(sampleData);
                }

                if (cbtheothang.Checked)
                {
                    ShowDoanhThuTrongThang(sampleData);
                }

                if (cbtheongay.Checked)
                {
                    ShowDoanhThuTheoNgay(sampleData);

                }
            }

        }

        #region theo type cua sp
        public void ShowThuedoTheoThang(List<RevenueRecord> data)
        {
            var chartType = SeriesChartType.Column;
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                chartType = cbTypeOfChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else { }
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
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            // Luôn group theo từng ngày
            var groupedData = data.GroupBy(x => x.RentalDate.ToString("dd/MM/yyyy"));

            var allTypes = data.Select(x => x.ProductType).Distinct();

            foreach (var type in allTypes)
            {
                Series series = new Series(type)
                {
                    ChartType = chartType,
                    BorderWidth = 2,
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };
                series["PointWidth"] = "0.2";

                foreach (var group in groupedData.OrderBy(g => DateTime.ParseExact(g.Key, "dd/MM/yyyy", null)))
                {
                    var doanhthu = group
                        .Where(x => x.ProductType == type)
                        .Sum(x => x.Revenue);

                    int pointIndex = series.Points.AddXY(group.Key, doanhthu);
                    series.Points[pointIndex].Label = doanhthu.ToString("N0") + " vnđ";
                }

                chart.Series.Add(series);
            }

            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                grThuedo.Controls.Clear();
                grThuedo.Controls.Add(chart);
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                grMakeup.Controls.Clear();
                grMakeup.Controls.Add(chart);
            }
            else { }
        }
        public void ShowThuedoTheoNam(List<RevenueRecord> data) // okay
        {
            var chartType = SeriesChartType.Column;
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                chartType = cbTypeOfChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else { }
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
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            // Group theo tháng trong năm (MM/yyyy)
            var groupedData = data.GroupBy(x => x.RentalDate.ToString("MM/yyyy")).ToList();

            // Lấy danh sách loại sản phẩm
            var allTypes = data.Select(x => x.ProductType).Distinct();

            foreach (var type in allTypes)
            {
                Series series = new Series(type)
                {
                    ChartType = chartType,
                    BorderWidth = 2,
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };
                series["PointWidth"] = "0.2";
                // Sắp xếp theo thời gian tăng dần
                foreach (var group in groupedData.OrderBy(g => DateTime.ParseExact("01/" + g.Key, "dd/MM/yyyy", null)))
                {
                    var doanhthu = group
                        .Where(x => x.ProductType == type)
                        .Sum(x => x.Revenue);

                    int pointIndex = series.Points.AddXY(group.Key, doanhthu);
                    series.Points[pointIndex].Label = doanhthu.ToString("N0") + " vnđ";
                }

                chart.Series.Add(series);
            }
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                grThuedo.Controls.Clear();
                grThuedo.Controls.Add(chart);
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                grMakeup.Controls.Clear();
                grMakeup.Controls.Add(chart);
            }
            else { }
        }
        public void ShowThuedoTheoNgay(List<RevenueRecord> data, string groupBy)
        {
            var chartType = SeriesChartType.Column;
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                chartType = cbTypeOfChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else { }
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
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            // Group dữ liệu
            var groupedData = data.GroupBy(x =>
            {
                switch (groupBy.ToLower())
                {
                    case "day":
                        return x.RentalDate.ToString("dd/MM/yyyy");
                    case "month":
                        return x.RentalDate.ToString("MM/yyyy");
                    case "year":
                        return x.RentalDate.Year.ToString();
                    default:
                        return x.RentalDate.ToString("dd/MM/yyyy");
                }
            }).ToList(); // ✅ convert to list để dễ xử lý sắp xếp

            // Sắp xếp theo thời gian thực
            IEnumerable<IGrouping<string, RevenueRecord>> orderedGroups;
            if (groupBy.ToLower() == "year")
            {
                orderedGroups = groupedData.OrderBy(g => int.Parse(g.Key));
            }
            else if (groupBy.ToLower() == "month")
            {
                orderedGroups = groupedData.OrderBy(g => DateTime.ParseExact("01/" + g.Key, "dd/MM/yyyy", null));
            }
            else // day
            {
                orderedGroups = groupedData.OrderBy(g => DateTime.ParseExact(g.Key, "dd/MM/yyyy", null));
            }

            // Lấy danh sách loại sản phẩm
            var allTypes = data.Select(x => x.ProductType).Distinct();

            foreach (var type in allTypes)
            {
                Series series = new Series(type)
                {
                    ChartType = groupBy.ToLower() == "day" ? SeriesChartType.Column : SeriesChartType.Line,
                    BorderWidth = 2,
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };
                series["PointWidth"] = "0.2";

                foreach (var group in orderedGroups)
                {
                    var doanhthu = group
                        .Where(x => x.ProductType == type)
                        .Sum(x => x.Revenue);

                    int pointIndex = series.Points.AddXY(group.Key, doanhthu);
                    series.Points[pointIndex].Label = doanhthu.ToString("N0") + " vnđ";
                }

                chart.Series.Add(series);
            }

            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                grThuedo.Controls.Clear();
                grThuedo.Controls.Add(chart);
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                grMakeup.Controls.Clear();
                grMakeup.Controls.Add(chart);
            }
            else { }
        }
        #endregion

        #region only doanh thu
        public void ShowDoanhThuTheoNgay(List<RevenueRecord> data)
        {
            var chartType = SeriesChartType.Column;
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                chartType = cbTypeOfChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else { }
            Chart chart = new Chart { Dock = DockStyle.Fill };
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            ChartArea area = new ChartArea("MainArea");
            area.AxisY.MajorGrid.LineWidth = 0;
            area.AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            var groupedData = data.GroupBy(x => x.RentalDate.Date)
                                  .OrderBy(g => g.Key);

            Series series = new Series("Tổng doanh thu")
            {
                ChartType = chartType,
                BorderWidth = 2,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black
            };
            series["PointWidth"] = "0.3";

            foreach (var group in groupedData)
            {
                var label = group.Key.ToString("dd/MM/yyyy");
                decimal doanhthu = group.Sum(x => x.Revenue);
                int pointIndex = series.Points.AddXY(label, doanhthu);
                series.Points[pointIndex].Label = doanhthu.ToString("N0") + " vnđ";
            }

            chart.Series.Add(series);

            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                grThuedo.Controls.Clear();
                grThuedo.Controls.Add(chart);
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                grMakeup.Controls.Clear();
                grMakeup.Controls.Add(chart);
            }
            else { }
        }
        public void ShowDoanhThuTrongThang(List<RevenueRecord> data)
        {
            int thang = DateTime.Now.Month;
            int nam = DateTime.Now.Year;

            var filteredData = data.Where(x => x.RentalDate.Month == thang && x.RentalDate.Year == nam).ToList();
            ShowDoanhThuTheoNgay(filteredData);
        }
        public void ShowDoanhThuTheoNam(List<RevenueRecord> data)
        {
            var chartType = SeriesChartType.Column;
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                chartType = cbTypeOfChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                chartType = cbtypecuaChart.Checked ? SeriesChartType.Line : SeriesChartType.Column;
            }
            else { }

            int nam = DateTime.Now.Year;
            var filteredData = data.Where(x => x.RentalDate.Year == nam);

            Chart chart = new Chart { Dock = DockStyle.Fill };
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();

            ChartArea area = new ChartArea("MainArea");
            area.AxisY.MajorGrid.LineWidth = 0;
            area.AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("Legend"));

            var groupedData = filteredData
                .GroupBy(x => new { x.RentalDate.Year, x.RentalDate.Month })
                .OrderBy(g => g.Key.Month);


            Series series = new Series("Tổng doanh thu")
            {
                ChartType = chartType,
                BorderWidth = 2,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black
            };
            series["PointWidth"] = "0.3";

            foreach (var group in groupedData)
            {
                string label = $"{group.Key.Month:D2}/{group.Key.Year}";
                decimal doanhthu = group.Sum(x => x.Revenue);
                int pointIndex = series.Points.AddXY(label, doanhthu);
                series.Points[pointIndex].Label = doanhthu.ToString("N0") + " vnđ";
            }

            chart.Series.Add(series);
            if (tabTotalDoanhThu.SelectedTab.Name == "tabRentThings")
            {
                grThuedo.Controls.Clear();
                grThuedo.Controls.Add(chart);
            }
            else if (tabTotalDoanhThu.SelectedTab.Name == "tabMakeup")
            {
                grMakeup.Controls.Clear();
                grMakeup.Controls.Add(chart);
            }
            else { }
        }
        #endregion

        private void btMakeFind_Click(object sender, EventArgs e)
        {
            List<RevenueRecord> sampleData = new List<RevenueRecord>();
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string type = "";
            string? idNV = null;
            int? moth = null, day = null;
            int year = gioVN.Year;

            if (rdbyyear.Checked && !string.IsNullOrEmpty(tbtheoyear.Text))
            {
                year = int.Parse(tbtheoyear.Text);
                type = "year";
            }

            if (rdbymoth.Checked && !string.IsNullOrEmpty(theomoth.Text))
            {
                moth = int.Parse(theomoth.Text);
                type = "month";
            }

            if (rdtheongay.Checked)
            {
                DateTime selectedDate = cldmake.SelectionStart;
                day = selectedDate.Day;
                type = "day";
            }

            if (cbbyNV.Checked && !string.IsNullOrEmpty(cbNV.Text))
            {
                if (cbNV.Text != "All_NV")
                {
                    string[] parts = cbNV.Text.Split('|');
                    idNV = parts[1].Trim();
                }
            }

            if (cbbyNV.Checked)
            {
                string typecheckNV = "Only1";
                if (cbNV.Text == "All_NV")
                    typecheckNV = "ALL";
                sampleData = controller.GetDoanhThuMakeUp(1, typecheckNV, year, idNV, moth, day);

                if (rdbymoth.Checked)
                {
                    var dataTheoThang = sampleData
                                       .Where(x => x.RentalDate.Month == moth && x.RentalDate.Year == year)
                                       .ToList();
                    ShowThuedoTheoThang(dataTheoThang);
                }
                else if (rdbyyear.Checked)
                {
                    var dataTheoNam = sampleData
                                     .Where(x => x.RentalDate.Year == year)
                                     .ToList();
                    ShowThuedoTheoNam(dataTheoNam);
                }
                else
                    ShowThuedoTheoNgay(sampleData, type);
            }
            else
            {
                sampleData = controller.GetDoanhThuMakeUp(2, "", year, idNV, moth, day);

                if (cbtheonam.Checked)
                {
                    ShowDoanhThuTheoNam(sampleData);
                }

                if (cbtheothang.Checked)
                {
                    ShowDoanhThuTrongThang(sampleData);
                }

                if (cbtheongay.Checked)
                {
                    ShowDoanhThuTheoNgay(sampleData);

                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbDTuoctinh_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDTuoctinh.Checked)
                cbDTchinhxac.Checked = false;
        }

        private void cbDTchinhxac_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDTchinhxac.Checked)
                cbDTuoctinh.Checked = false;
        }
    }
}
