using LuongEmStudio.BaseData;
using LuongEmStudio.DataGetWay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LuongEmStudio.Core
{
    public class Controller
    {
        #region showMessageColor
        public void ErrorMSG(Label label, string text)
        {
            label.ForeColor = Color.Red;
            label.Text = text;
        }
        public void SuccessMSG(Label label, string text)
        {
            label.ForeColor = Color.Blue;
            label.Text = text;
        }
        #endregion

        UpdateGW updateGW;
        SelectGW selectGW;
        public Controller()
        {
            selectGW = new SelectGW();
            updateGW = new UpdateGW();
        }
        public void CreateListSP()
        {
            var productData = new ProductData
            {
                TypeSP = new List<string>
            {
                "Áo dài"
            },
                PriceSP = new List<string>
            {
                "10.000 vnđ", "15.000 vnđ", "100.000 vnđ"
            }
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Để file dễ nhìn hơn
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Để giữ nguyên ký tự đặc biệt
            };

            string jsonString = JsonSerializer.Serialize(productData, options);

            // Đường dẫn để lưu file
            string path = BaseDataSPInfo.FileConfig;
            File.WriteAllText(path, jsonString);
        }

        public ExecutionResult addProductSP(BaseDataSPInfo product)
        {
            return updateGW.InserProduct(product);
        }
        public ExecutionResult DeleteProductSP(BaseDataSPInfo product)
        {
            return updateGW.DeleteProduct(product);
        }
        public ExecutionResult UpdateProductSP(BaseDataSPInfo product)
        {
            return updateGW.UpdateProductSP(product);
        }
        public ExecutionResult FindSPbyProductID(BaseDataSPInfo product)
        {
            return selectGW.GetProductByID(product);
        }
        public ExecutionResult FindSPbyProductAll(BaseDataSPInfo product)
        {
            ExecutionResult result = new ExecutionResult();
            DataSet ds = new DataSet();
            result = selectGW.GetProductByID(product);
            if (result.Status)
            {
                ds = (DataSet)result.Anything;
            }
            return result;
        }
        public ExecutionResult DeleteImage(string productID)
        {
            ExecutionResult executionResult = new ExecutionResult();
            string imageDirectory = BaseDataSPInfo.ImageFolder;
            string[] matchingFiles = Directory.GetFiles(imageDirectory, productID + ".*");

            if (matchingFiles.Length > 0) // Kiểm tra xem có file nào khớp không
            {
                foreach (string file in matchingFiles)
                {
                    File.Delete(file); // Xóa từng file tìm thấy
                }
                executionResult.Message = "Sản phẩm và tất cả ảnh liên quan đã được xóa!";
            }
            else
            {
                executionResult.Message = "Sản phẩm đã được xóa nhưng không tìm thấy ảnh!";
            }
            return executionResult;
        }
        public ExecutionResult ShowImage(string productID, PictureBox pictureBox)
        {
            ExecutionResult executionResult = new ExecutionResult();
            string imageDirectory = BaseDataSPInfo.ImageFolder; // Thư mục chứa ảnh
            string[] matchingFiles = Directory.GetFiles(imageDirectory, productID + ".*"); // Tìm file theo productID

            if (matchingFiles.Length > 0)
            {
                string imagePath = matchingFiles[0];
                using (var img = Image.FromFile(imagePath))
                {
                    pictureBox.Image = new Bitmap(img); // Bản sao -> tránh lỗi khóa file
                }
                //pictureBox.Image = Image.FromFile(imagePath);
            }
            else
            {
                executionResult.Message = "Không tìm thấy ảnh trên máy!";
                pictureBox.Image = null;
            }

            return executionResult;
        }
        public ExecutionResult LoadMakeBookings(DataGridView dataGridViewCalendar, int month)
        {
            ExecutionResult execution = new ExecutionResult();
            int year = DateTime.Now.Year;

            this.CloseOrderDuoDate();
            execution = selectGW.LoadCalendar(month);
            Dictionary<DateTime, int> appointments = (Dictionary<DateTime, int>)execution.Anything;
            for (int i = 0; i < 7; i++)
            {
                dataGridViewCalendar.Columns.Add("col" + i, DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName((DayOfWeek)i));
            }

            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int startDay = (int)firstDayOfMonth.DayOfWeek; // Lấy thứ của ngày đầu tháng

            int rowCount = (int)Math.Ceiling((daysInMonth + startDay) / 7.0); // Tính số hàng cần thiết
            for (int i = 0; i < rowCount; i++)
            {
                dataGridViewCalendar.Rows.Add(); // Thêm hàng mới
            }

            // Gán giá trị vào từng ô trong DataGridView
            int day = 1;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < startDay)
                        continue; // Bỏ qua các ô trước ngày đầu tiên của tháng

                    if (day > daysInMonth)
                        break; // Dừng khi hết số ngày trong tháng

                    DateTime cellDate = new DateTime(year, month, day);
                    if (appointments.ContainsKey(cellDate))
                    {
                        dataGridViewCalendar.Rows[i].Cells[j].Value = $"{day} ({appointments[cellDate]} ca)";
                        dataGridViewCalendar.Rows[i].Cells[j].Style.BackColor = Color.Lime; // Đánh dấu ô có lịch makeup
                    }
                    else
                    {
                        dataGridViewCalendar.Rows[i].Cells[j].Value = day.ToString();
                        dataGridViewCalendar.Rows[i].Cells[j].Style.BackColor = Color.Beige;
                    }

                    day++;
                }
            }
            return execution;
        }
        public ExecutionResult AddStaff(string name, string sdt, string address, string notes)
        {
            ExecutionResult result = new ExecutionResult();
            DataSet ds = new DataSet();
            string id = Guid.NewGuid().ToString();

            result = updateGW.InserNewStaff(id, name, sdt, address, notes);
            if (result.Status)
            {
                ds = (DataSet)result.Anything;
            }
            return result;
        }
        public ExecutionResult UpdateStaff(string id, string name, string sdt, string address, string notes)
        {
            ExecutionResult result = new ExecutionResult();
            DataSet ds = new DataSet();

            result = updateGW.UpdateStaff(id, name, sdt, address, notes);
            if (result.Status)
            {
                ds = (DataSet)result.Anything;
            }
            return result;
        }
        public ExecutionResult SearchStaff(string name, string sdt)
        {
            ExecutionResult result = new ExecutionResult();

            result = selectGW.GetStaffInfo(name, sdt);
            if (result.Status)
            {
                result.Message = "Tìm nv ok";
            }
            else
                result.Message = "Không tìm thấy nv nào như trên";
            return result;
        }
        public ExecutionResult DeleteStaff(string id)
        {
            return updateGW.DeleteStaff(id);
        }

        public ExecutionResult InsertMakeSchedule(BaseDataMakeInfo MakeInfo, DateTime SCheduleMake, CheckBox cbkhachquen, DateTime timeClose)
        {
            ExecutionResult result = new ExecutionResult();
            string contactKH = "";
            if (cbkhachquen.Checked)
                contactKH = ((DataSet)selectGW.GetContactKH(MakeInfo.SdtKhach).Anything).Tables[0].Rows[0]["userid"].ToString();
            result = updateGW.InsertMakeSchedule(MakeInfo, SCheduleMake, cbkhachquen, contactKH, timeClose);
            if (result.Status)
            {
                result.Message = "Thêm lịch makeup thành công !";
            }
            return result;
        }
        public ExecutionResult UpdateMakeSchedule(BaseDataMakeInfo MakeInfo, DateTime SCheduleMake, string userID, DateTime timeClose)
        {
            ExecutionResult result = new ExecutionResult();

            result = updateGW.UpdateMakeSchedule(MakeInfo, SCheduleMake, userID, timeClose);
            if (result.Status)
            {
                result.Message = "Sửa lịch makeup thành công !";
                BaseDataMakeInfo.IDlichMake = "";
            }
            return result;
        }
        public ExecutionResult GetforUpdate(string id)
        {
            return selectGW.GetforUpdate(id);
        }
        public ExecutionResult CheckDupScheduleMake(DateTime SCheduleMake, string IDnvmake)
        {
            ExecutionResult result = new ExecutionResult();

            result = selectGW.GetScheduleMake(SCheduleMake, IDnvmake);
            if (result.Status)
            {
                if (((DataSet)result.Anything).Tables[0].Rows.Count > 0)
                {
                    result.Status = false;
                    result.Message = $"NV {((DataSet)result.Anything).Tables[0].Rows[0]["namestaff"].ToString()} Đã có lịch Make-up vào lúc {((DataSet)result.Anything).Tables[0].Rows[0]["scheduleddate"].ToString()}";
                }
                else
                {
                    result.Status = true;
                }
            }
            return result;
        }
        public ExecutionResult GetScheduleMakeByDatetime(DateTime dateMake, DataGridView dtgv)
        {
            ExecutionResult result = new ExecutionResult();
            DataSet ds = new DataSet();
            result = selectGW.GetScheduleMakeByDatetime(dateMake);
            if (result.Status)
            {
                ds = (DataSet)result.Anything;
                dtgv.DataSource = ds.Tables[0];
            }
            return result;
        }
        public void CloseOrderDuoDate()
        {
            updateGW.CloseOrderDuoDate();
        }
        public ExecutionResult CancelMakeUp(string BookingID)
        {
            return updateGW.CancelMakeUp(BookingID);
        }
        public ExecutionResult GetScheduleMakeByDatetimeByID(string BookingID)
        {
            return selectGW.GetScheduleMakeByDatetimeByID(BookingID);
        }
        public List<BookingData> GetScheduleMakeByDatetimeByID(DataGridView dgv, string status, int year, int? month = null, string staffId = null)
        {
            ExecutionResult exe = selectGW.GetBookingData(status, year, month, staffId);
            DataSet ds = (DataSet)exe.Anything;
            var data = new List<BookingData>();
            if (exe.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var bookingData = new BookingData
                    {
                        Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["scheduleddate"]),
                        StaffName = ds.Tables[0].Rows[i]["namestaff"].ToString(),
                        Count = Convert.ToInt32(ds.Tables[0].Rows[i]["total"])
                    };
                    data.Add(bookingData);
                }
                ExecutionResult exe1 = selectGW.GetBookingData1(status, year, month, staffId);
                DataSet ds1 = (DataSet)exe1.Anything;
                dgv.DataSource = ds1.Tables[0];
            }
            return data;
        }

        public void GetTypeSP(ComboBox cbTypeSP, int check)
        {
            ExecutionResult exe = selectGW.GetTypeSP();
            DataSet ds = (DataSet)exe.Anything;
            List<string> list = ds.Tables[0]
                      .AsEnumerable()
                      .Select(row => row["type_production"].ToString())
                      .OrderBy(x => x) // Sắp xếp tăng dần
                      .ToList();
            if (check == 1)
                list.Insert(0, "All_SP");
            cbTypeSP.Items.AddRange(list.ToArray());
            if (check == 1)
                cbTypeSP.SelectedIndex = 0;

        }
        public ExecutionResult GetPictureByTypeSP(string TypeSP)
        {
            return selectGW.GetPictureByTypeSP(TypeSP, 0, 0);
        }
        public ExecutionResult AddOrder(List<BaseDataBorrowReturn> danhSachDonHang, CheckBox khquen, string sdtKH)
        {
            string contactKH = "";
            if (khquen.Checked)
                contactKH = ((DataSet)selectGW.GetContactKH(sdtKH).Anything).Tables[0].Rows[0]["userid"].ToString();
            return updateGW.InsertOrder(danhSachDonHang, khquen, contactKH);
        }
        public ExecutionResult GetOrderNo(string ordeNo)
        {
            return selectGW.GetOrderNo(ordeNo);
        }
        public ExecutionResult DoneOrder(List<Tuple<long, int, long, decimal>> orderList)
        {
            return updateGW.DoneOrder(orderList);
        }
        public ExecutionResult CancelOrder(List<Tuple<long, int, long>> orderList)
        {
            return updateGW.CancelOrder(orderList);
        }
        public List<RentalSummary> GetSumBorrowReturn(DataGridView dgv, string status, int year, int? month = null)
        {
            var result = new List<RentalSummary>();

            ExecutionResult exe = selectGW.GetRentalSummary(status, year, month);
            DataSet ds = (DataSet)exe.Anything;
            var data = new List<RentalSummary>();
            if (exe.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new RentalSummary
                    {
                        Date = DateTime.Parse(ds.Tables[0].Rows[i]["rental_date"].ToString()),
                        Type = ds.Tables[0].Rows[i]["type_production"].ToString(),
                        Quantity = Convert.ToInt32(ds.Tables[0].Rows[i]["total_quantity"])
                    });
                }
                ExecutionResult exe1 = selectGW.GetRentalSummary1(status, year, month);
                DataSet ds1 = (DataSet)exe1.Anything;
                dgv.DataSource = ds1.Tables[0];
            }
            return result;
        }
        public ExecutionResult CheckQTYinWH(string typeSP, int sl, long productid)
        {
            return selectGW.GetPictureByTypeSP(typeSP, 1, sl, productid);
        }
        public ExecutionResult CheckQTYinWH1(string typeSP, long productid)
        {
            return selectGW.GetPictureByTypeSP(typeSP, 3, 0, productid);
        }
        public ExecutionResult CheckContactKH(string ContactKH)
        {
            return selectGW.GetContactKH(ContactKH);
        }
        public List<RevenueRecord> GetDoanhThuThueDoUocTinh(CheckBox cbDTuoctinh, string typesp, int year, int? month = null, int? day = null)
        {
            var result = new List<RevenueRecord>();
            ExecutionResult exe = new ExecutionResult();

            if (cbDTuoctinh.Checked)
                exe = selectGW.GetDoanhThuThueDoUocTinh(typesp, year, month, day);
            else
                exe = selectGW.GetDoanhThuThueDoChinhXac(typesp, year, month, day);
            DataSet ds = (DataSet)exe.Anything;
            if (exe.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new RevenueRecord
                    {
                        RentalDate = DateTime.Parse(ds.Tables[0].Rows[i]["rental_date"].ToString()),
                        ProductType = ds.Tables[0].Rows[i]["product_type"].ToString(),
                        Revenue = decimal.Parse(ds.Tables[0].Rows[i]["revenue"].ToString())
                    });
                }
            }
            return result;
        }
        public List<RevenueRecord> GetDoanhThuMakeUp(int checkFunction, string typecheckNV, int year, string? idNV = "", int? month = null, int? day = null)
        {
            var result = new List<RevenueRecord>();
            ExecutionResult exe = new ExecutionResult();

            if (checkFunction == 1)
                exe = selectGW.GetDoanhThuMakeUp(typecheckNV, year, idNV, month, day);
            else
                exe = selectGW.GetDoanhThuMakeUp("ALL", year, idNV, month, day);

            DataSet ds = (DataSet)exe.Anything;
            if (exe.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new RevenueRecord
                    {
                        RentalDate = DateTime.Parse(ds.Tables[0].Rows[i]["time_group"].ToString()),
                        ProductType = ds.Tables[0].Rows[i]["staff_name"].ToString(),
                        Revenue = decimal.Parse(ds.Tables[0].Rows[i]["total_revenue"].ToString())
                    });
                }
            }
            return result;
        }

        public List<ProductStock> GetTotalWH(int check, RadioButton all, RadioButton rdNotReturn, string TypeSP)
        {
            var result = new List<ProductStock>();
            ExecutionResult exe = new ExecutionResult();
            if (check == 0)
                exe = selectGW.GetTotalWH(all, rdNotReturn);
            else
                exe = selectGW.GetTotalWHBySP(all, rdNotReturn, TypeSP);

            DataSet ds = (DataSet)exe.Anything;
            if (exe.Status && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(new ProductStock
                    {
                        TypeProduction = ds.Tables[0].Rows[i]["type_production"].ToString(),
                        TotalQuantity = int.Parse(ds.Tables[0].Rows[i]["total_quantity"].ToString())
                    });
                }
            }
            return result;
        }

        public DataTable GetUpcomingMakeupBookings(DateTime from, DateTime to)
        {
            return selectGW.GetUpcomingMakeupBookings(from, to);
        }
    }
}

