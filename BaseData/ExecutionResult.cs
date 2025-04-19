using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuongEmStudio.BaseData
{
    public class BaseDataSPInfo
    {
        public static string ImageFolder = AppDomain.CurrentDomain.BaseDirectory + @"Images\";
        public static string FileConfig = AppDomain.CurrentDomain.BaseDirectory + @"Appconfig.json";
        public static string MakeUpOption = AppDomain.CurrentDomain.BaseDirectory + @"Option\";

        private string vnameSP = "";
        private string vtypeSP = "";
        private string vsizeSP;
        private decimal? vPriceSP;
        private int? vQtySP;
        private string vDescSP = "";
        private long? vproductID;
        public string nameSP
        {
            get { return vnameSP; }
            set { vnameSP = value; }
        }
        public string typeSP
        {
            get { return vtypeSP; }
            set { vtypeSP = value; }
        }
        public string sizeSP
        {
            get { return vsizeSP; }
            set { vsizeSP = value; }
        }
        public decimal? PriceSP
        {
            get { return vPriceSP; }
            set { vPriceSP = value; }
        }
        public int? QtySP
        {
            get { return vQtySP; }
            set { vQtySP = value; }
        }
        public string DescSP
        {
            get { return vDescSP; }
            set { vDescSP = value; }
        }
        public long? productID
        {
            get { return vproductID; }
            set { vproductID = value; }
        }
    }
    public class BaseDataMakeInfo
    {
        public static string IDlichMake = "";
        //public static string FileConfig = AppDomain.CurrentDomain.BaseDirectory + @"Appconfig.json";
        //public static string MakeUpOption = AppDomain.CurrentDomain.BaseDirectory + @"Option\";

        private string vGoiMake = "";
        private string vNVmake = "";
        private string vNameKach = "";
        private string vSdtKhach = "";
        private string vTiencoc = "";
        private long? vBookingID;
        private decimal? vTotalPrice;
        private string vNameBooking = "";
        private string vLocationMake = "";
        private string vDescBooking = "";
        private int vQTYkhach;

        public int QTYkhach
        {
            get { return vQTYkhach; }
            set { vQTYkhach = value; }
        }
        public string DescBooking
        {
            get { return vDescBooking; }
            set { vDescBooking = value; }
        }
        public decimal? TotalPrice
        {
            get { return vTotalPrice; }
            set { vTotalPrice = value; }
        }
        public string GoiMake
        {
            get { return vGoiMake; }
            set { vGoiMake = value; }
        }
        public string IDNVMake
        {
            get { return vNVmake; }
            set { vNVmake = value; }
        }
        public string LocationMake
        {
            get { return vLocationMake; }
            set { vLocationMake = value; }
        }
        public string NameKach
        {
            get { return vNameKach; }
            set { vNameKach = value; }
        }
        public string SdtKhach
        {
            get { return vSdtKhach; }
            set { vSdtKhach = value; }
        }
        public string Tiencoc
        {
            get { return vTiencoc; }
            set { vTiencoc = value; }
        }

        public string NameBooking
        {
            get { return vNameBooking; }
            set { vNameBooking = value; }
        }
        public long? BookingID
        {
            get { return vBookingID; }
            set { vBookingID = value; }
        }
    }
    public class ExecutionResult
    {
        private bool statusField = false;

        private string msgCodeField = "";

        private string messageField = "";

        private object[] argesField;

        private object anythingField;

        public bool Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        public string MSGCode
        {
            get
            {
                return msgCodeField;
            }
            set
            {
                msgCodeField = value;
            }
        }

        public string Message
        {
            get
            {
                return messageField;
            }
            set
            {
                messageField = value;
            }
        }

        public object[] Arges
        {
            get
            {
                return argesField;
            }
            set
            {
                argesField = value;
            }
        }

        public object Anything
        {
            get
            {
                return anythingField;
            }
            set
            {
                anythingField = value;
            }
        }
    }
    public class BookingData
    {
        public DateTime Date { get; set; }
        public string StaffName { get; set; }
        public int Count { get; set; }
    }
    public static class SelectedProductInfo
    {
        public static string? ProductId { get; set; }
        public static string? ProductName { get; set; }
        public static string? Description { get; set; }
        public static string? PricePerDay { get; set; }
        public static string? Size { get; set; }
        public static string? stockquantity { get; set; }
        public static string? imagePath { get; set; }
        public static string? QTYinWH { get; set; }

        public static void ClearProductInfo()
        {
            ProductId = null;
            ProductName = null;
            Description = null;
            PricePerDay = null;
            Size = null;
            stockquantity = null;
            imagePath = null;
            QTYinWH = null;
        }
    }
    public class BaseDataBorrowReturn
    {
        private string vNameKach = "";
        private string vSdtKhach = "";
        private decimal vTiencoc;
        private long? vBookingID;
        private decimal? vTotalPrice;
        private decimal? vTienphatsinh;
        private string vDescBooking = "";
        private int vQTYThue;
        private string vnotes = "";

        public decimal? Tienphatsinh
        {
            get { return vTienphatsinh; }
            set { vTienphatsinh = value; }
        }
        public string notes
        {
            get { return vnotes; }
            set { vnotes = value; }
        }
        public int QTYThue
        {
            get { return vQTYThue; }
            set { vQTYThue = value; }
        }
        public string DescBooking
        {
            get { return vDescBooking; }
            set { vDescBooking = value; }
        }
        public decimal? TotalPrice
        {
            get { return vTotalPrice; }
            set { vTotalPrice = value; }
        }
        public string NameKach
        {
            get { return vNameKach; }
            set { vNameKach = value; }
        }
        public string SdtKhach
        {
            get { return vSdtKhach; }
            set { vSdtKhach = value; }
        }
        public decimal Tiencoc
        {
            get { return vTiencoc; }
            set { vTiencoc = value; }
        }
        public long? productID
        {
            get { return vBookingID; }
            set { vBookingID = value; }
        }
    }
    public class RentalSummary
    {
        public DateTime Date { get; set; }    // Ngày thuê
        public string Type { get; set; }      // Loại đồ
        public int Quantity { get; set; }     // Số lượng thuê
    }
    public class ProductTag
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string PricePerDay { get; set; }
        public string Size { get; set; }
        public string StockQuantity { get; set; }
        public string ImagePath { get; set; }
        public string QTYinWH { get; set; }
    }
    public class RevenueRecord
    {
        public DateTime RentalDate { get; set; }     // Ngày thuê
        public string ProductType { get; set; }      // Loại đồ: "Áo dài", "Vest", "Váy cưới", ...
        public decimal Revenue { get; set; }         // Doanh thu
    }
    public class ProductStock
    {
        public string TypeProduction { get; set; }
        public int TotalQuantity { get; set; }
    }
    public class ProductData
    {
        public List<string> TypeSP { get; set; }
        public List<string> PriceSP { get; set; }
    }
}
