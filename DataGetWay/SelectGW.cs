using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace LuongEmStudio.DataGetWay
{
    public class SelectGW
    {
        DatabaseConnection DatabaseConnection;

        public SelectGW()
        {
            DatabaseConnection = new DatabaseConnection();
        }
        public ExecutionResult GetProductByID(BaseDataSPInfo product)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string query = @"SELECT productid as Mã_SP,productname AS Tên_SP,type_production as Loại_SP, size AS Size_SP,priceperday AS Giá_Thuê1_Ngày, stockquantity as Số_lượng_trong_kho,description as Mô_tả, createdate
                               FROM clothings.products WHERE 1=1";

            if (!string.IsNullOrEmpty(product.productID.ToString()) && product.productID.ToString() != "0")
            {
                query += " AND productid = :productid ";
                parameters.Add(new NpgsqlParameter("@productid", product.productID));
            }

            if (!string.IsNullOrEmpty(product.nameSP))
            {
                query += " AND productname = :productname ";
                parameters.Add(new NpgsqlParameter("@productname", product.nameSP));
            }

            if (!string.IsNullOrEmpty(product.typeSP))
            {
                query += " AND type_production = :type_production ";
                parameters.Add(new NpgsqlParameter("@type_production", product.typeSP));
            }

            if (!string.IsNullOrEmpty(product.PriceSP.ToString()))
            {
                query += " AND priceperday = :priceperday ";
                parameters.Add(new NpgsqlParameter("@priceperday", product.PriceSP));
            }

            if (!string.IsNullOrEmpty(product.sizeSP))
            {
                query += " AND size = :size ";
                parameters.Add(new NpgsqlParameter("@size", product.sizeSP));
            }
            query += " LIMIT 15 ";
            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult LoadCalendar(int month)
        {
            Dictionary<DateTime, int> appointments = new Dictionary<DateTime, int>();
            int year = DateTime.Now.Year;
            string query = @"SELECT DATE(ScheduledDate) AS day, COUNT(*) AS total FROM clothings.makeupbookings 
                      WHERE EXTRACT(YEAR FROM ScheduledDate) = @year 
                      AND EXTRACT(MONTH FROM ScheduledDate) = @month and status = 'OPEN'
                      GROUP BY DATE(ScheduledDate)";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter("@year", year));
            parameters.Add(new NpgsqlParameter("@month", month));

            ExecutionResult exe1 = DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
            DataSet ds = (DataSet)exe1.Anything;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTime date = Convert.ToDateTime(ds.Tables[0].Rows[i]["day"].ToString());
                    int count = Convert.ToInt32(ds.Tables[0].Rows[i]["total"]);
                    appointments[date] = count;
                }
            }

            ExecutionResult ex = new ExecutionResult();
            ex.Anything = appointments;
            return ex;
        }
        public ExecutionResult GetContactKH(string ContactKH)
        {
            string query = @"SELECT * FROM clothings.users WHERE UPPER(facebookphone) = @facebookphone ";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@facebookphone", ContactKH.ToUpper()));
            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetMakeupBookings()
        {
            string query = @"SELECT 
                        BookingID AS ID, 
                        UserID AS 'Mã khách hàng', 
                        ServiceID AS 'Mã dịch vụ', 
                        BookingDate AS 'Ngày đặt', 
                        ScheduledDate AS 'Ngày hẹn', 
                        TotalPrice AS 'Tổng tiền', 
                        Status AS 'Trạng thái' 
                    FROM makeupbookings";

            return DatabaseConnection.ExecuteQueryDS(query);
        }
        public ExecutionResult GetStaffInfo(string name, string sdt)
        {
            string query = @"SELECT * FROM clothings.makeupstaff WHERE 1=1";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            if (!string.IsNullOrEmpty(name))
            {
                query += " AND LOWER(namestaff) LIKE LOWER(@name)";
                parameters.Add(new NpgsqlParameter("@name", "%" + name + "%"));
            }

            if (!string.IsNullOrEmpty(sdt))
            {
                query += " AND sdt LIKE @sdt";
                parameters.Add(new NpgsqlParameter("@sdt", "%" + sdt + "%"));
            }

            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetScheduleMake(DateTime dateMake, string IDMake)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string query = @" SELECT *
                                FROM clothings.makeupbookings bok
                                JOIN clothings.makeupstaff sta ON bok.idstaffmake = sta.staffid
                                WHERE 
                                    bok.status = 'OPEN'
                                    AND bok.idstaffmake = :idstaff
                                    AND scheduleddate + (:MinGapMinutes + 60) * INTERVAL '1 minute' > :NewBookingStart
                                    AND scheduleddate < :NewBookingStart + :NewBookingDuration * INTERVAL '1 minute'";

            parameters.Add(new NpgsqlParameter("@NewBookingStart", dateMake));
            parameters.Add(new NpgsqlParameter("@NewBookingDuration", 60));
            parameters.Add(new NpgsqlParameter("@MinGapMinutes", 60));
            parameters.Add(new NpgsqlParameter("@idstaff", IDMake));

            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetScheduleMakeByDatetime(DateTime dateMake)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string query = @" SELECT scheduleddate as Thời_Gian_Make, namestaff as NV_Make, locationmake as Địa_Điểm_Make,us.fullname as Tên_Khách_Hàng,
                                goi.servicename as Gói_Make, goi.price as Giá_Gói_Make,goi.moneycoc as Tiền_Cọc,  us.facebookphone as liên_hệ_KH, bok.bookingid, goi.qty
                                FROM clothings.makeupbookings bok,clothings.makeupservices goi, clothings.makeupstaff sta, clothings.users us  Where  bok.idstaffmake = sta.staffid
                                and us.userid = bok.userid and  bok.status = 'OPEN' and goi.serviceid =  bok.bookingid
                                AND bok.scheduleddate::date =:BookingStart::date";

            parameters.Add(new NpgsqlParameter("@BookingStart", dateMake.Date));

            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetScheduleMakeByDatetimeByID(string id)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string query = @" SELECT scheduleddate, namestaff, locationmake,us.fullname,goi.servicename, goi.price,goi.moneycoc,  us.facebookphone, bok.bookingid, goi.qty
                                FROM clothings.makeupbookings bok,clothings.makeupservices goi, clothings.makeupstaff sta, clothings.users us  Where  bok.idstaffmake = sta.staffid
                                and us.userid = bok.userid and  bok.status = 'OPEN' and goi.serviceid =  bok.bookingid and bok.bookingid=:bookingid ";

            parameters.Add(new NpgsqlParameter("@bookingid", long.Parse(id)));

            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetforUpdate(string id)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string query = @" SELECT us.* FROM clothings.makeupbookings bok, clothings.users us  Where  us.userid = bok.userid and bok.bookingid=:id";

            parameters.Add(new NpgsqlParameter("@id", long.Parse(id)));

            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetBookingData(string status, int year, int? month = null, string staffId = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT bok.scheduleddate, us.namestaff, COUNT(*) AS total
                       FROM clothings.makeupbookings bok, clothings.makeupstaff us 
                      WHERE  bok.idstaffmake = us.staffid and EXTRACT(YEAR FROM scheduleddate) = @year ");

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM bok.scheduleddate) = @month");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (!string.IsNullOrEmpty(staffId))
            {
                sql.Append(" AND staffId = @staffId");
                parameters.Add(new NpgsqlParameter("staffId", staffId));
            }

            if (status != "ALL")
            {
                sql.Append(" AND bok.status = @statuss ");
                parameters.Add(new NpgsqlParameter("statuss", status));
            }

            sql.Append(" GROUP BY bok.scheduleddate, us.namestaff ORDER BY bok.scheduleddate, us.namestaff");
            parameters.Add(new NpgsqlParameter("year", year));

            string sql1 = sql.ToString();
            return DatabaseConnection.ExecuteQueryDS(sql1, parameters.ToArray());
        }
        public ExecutionResult GetBookingData1(string status, int year, int? month = null, string staffId = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT bok.status , scheduleddate as Thời_Gian_Make, namestaff as NV_Make, locationmake as Địa_Điểm_Make,user1.fullname as Tên_NV,
                                goi.servicename as Gói_Make, goi.price as Giá_Gói_Make,goi.moneycoc as Tiền_Cọc,  user1.facebookphone as liên_hệ_KH
                       FROM clothings.makeupbookings bok, clothings.makeupstaff us ,clothings.makeupservices goi, clothings.users user1 
                      WHERE  bok.idstaffmake = us.staffid  and user1.userid = bok.userid  and goi.serviceid =  bok.bookingid and EXTRACT(YEAR FROM scheduleddate) = @year ");

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM bok.scheduleddate) = @month");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (!string.IsNullOrEmpty(staffId))
            {
                sql.Append(" AND staffId = @staffId");
                parameters.Add(new NpgsqlParameter("staffId", staffId));
            }

            if (status != "ALL")
            {
                sql.Append(" AND bok.status = @statuss ");
                parameters.Add(new NpgsqlParameter("statuss", status));
            }

            sql.Append(" ORDER BY bok.scheduleddate, us.namestaff");
            parameters.Add(new NpgsqlParameter("year", year));

            string sql1 = sql.ToString();
            return DatabaseConnection.ExecuteQueryDS(sql1, parameters.ToArray());
        }
        public ExecutionResult GetTypeSP()
        {
            string query = @"SELECT distinct(type_production) FROM clothings.products";
            return DatabaseConnection.ExecuteQueryDS(query);
        }
        public ExecutionResult GetPictureByTypeSP(string typeSP, int check, int sl, long? productid = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            string query = @"SELECT * FROM clothings.products WHERE type_production =:typeSP ";

            parameters.Add(new NpgsqlParameter("@typeSP", typeSP));
            if (check == 0)
            {
                query += " and saveqty > :sl ";
                parameters.Add(new NpgsqlParameter("@sl", sl));
            }
            else if (check == 1)
            {
                query += " and saveqty >= :sl and productid=:productid";
                parameters.Add(new NpgsqlParameter("@sl", sl));
                parameters.Add(new NpgsqlParameter("@productid", productid));
            }
            else
            {
                query += " and productid=:productid";
                parameters.Add(new NpgsqlParameter("@sl", sl));
                parameters.Add(new NpgsqlParameter("@productid", productid));
            }


            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }
        public ExecutionResult GetOrderNo(string orderNo)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            string query = @"SELECT prod.productid, u.fullname, od.borrowdate, prod.type_production, od.moneycoc, od.qty, od.notes, prod.priceperday, od.totalamount, od.tienphatsinh,od.orderid
                               FROM clothings.orders od
                               JOIN clothings.users u ON u.userid = od.userid 
                               JOIN clothings.products prod ON od.productid = prod.productid
                               WHERE od.status = 'BORROW' AND u.facebookphone =:phoneKH ";
            parameters.Add(new NpgsqlParameter("@phoneKH", orderNo));
            return DatabaseConnection.ExecuteQueryDS(query, parameters.ToArray());
        }

        public ExecutionResult GetRentalSummary(string status, int year, int? month = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT 
                               DATE(b.borrowdate) AS rental_date,
                               p.type_production,
                               SUM(b.qty) AS total_quantity
                           FROM clothings.orders b
                           JOIN clothings.products p ON b.productid = p.productid
                           WHERE EXTRACT(YEAR FROM b.borrowdate) = :year ");

            parameters.Add(new NpgsqlParameter("year", year));

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.borrowdate) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (status != "ALL")
            {
                sql.Append(" AND b.status = :status ");
                parameters.Add(new NpgsqlParameter("status", status));
            }

            sql.Append(" GROUP BY rental_date, p.type_production ORDER BY rental_date");

            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }
        public ExecutionResult GetRentalSummary1(string status, int year, int? month = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@" SELECT fullname as Tên_Người_Thuê,facebookphone as Liên_Hệ,borrowdate as Ngày_Thuê, returndate as Ngày_Trả,type_production as Loại_Đồ,size,qty as Số_Lượng,
                           totalamount as Tổng_Tiền,priceperday as Giá_Thuê_1Ngày,moneycoc as Tiền_Cọc, tienphatsinh as Tiền_Phát_Sinh,status as Trạng_Thái
                           FROM clothings.orders b
                           JOIN clothings.products p ON b.productid = p.productid
                           JOIN clothings.users u ON u.userid = b.userid
                           WHERE  EXTRACT(YEAR FROM b.borrowdate) = :year ");

            parameters.Add(new NpgsqlParameter("year", year));

            if (month != null)
            {
                sql.Append(" AND EXTRACT(MONTH FROM b.borrowdate) = :month ");
                parameters.Add(new NpgsqlParameter("month", month));
            }

            if (status != "ALL")
            {
                sql.Append(" AND b.status = :status ");
                parameters.Add(new NpgsqlParameter("status", status));
            }
            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }
        public ExecutionResult GetDoanhThuThueDoUocTinh(string typesp, int year, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT o.borrowdate::date AS rental_date,
                                p.type_production AS product_type,
                                SUM(o.totalamount - o.moneycoc) AS revenue
                          FROM clothings.orders o 
                          join clothings.products p on p.productid = o.productid
                          WHERE status = 'BORROW'                    
                            AND (:nam IS NULL OR EXTRACT(YEAR FROM borrowdate) = :nam) ");

            parameters.Add(new NpgsqlParameter("nam", year));

            if (day != null)
            {
                sql.Append(" AND (:ngay IS NULL OR EXTRACT(DAY FROM o.borrowdate) = :ngay) ");
                parameters.Add(new NpgsqlParameter("ngay", day));
            }

            if (month != null)
            {
                sql.Append("  AND (:thang IS NULL OR EXTRACT(MONTH FROM o.borrowdate) = :thang) ");
                parameters.Add(new NpgsqlParameter("thang", month));
            }

            if (!string.IsNullOrEmpty(typesp))
            {
                sql.Append(" AND p.type_production = :typesp ");
                parameters.Add(new NpgsqlParameter("typesp", typesp));
            }
            sql.Append(" group by rental_date, product_type ORDER BY rental_date ");

            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }
        public ExecutionResult GetDoanhThuThueDoChinhXac(string typesp, int year, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT o.borrowdate::date AS rental_date,
                                p.type_production AS product_type,
                                SUM(o.lastmoney) AS revenue
                          FROM clothings.orders o 
                          join clothings.products p on p.productid = o.productid
                          WHERE status = 'RETURN'                    
                            AND (:nam IS NULL OR EXTRACT(YEAR FROM borrowdate) = :nam) ");

            parameters.Add(new NpgsqlParameter("nam", year));

            if (day != null)
            {
                sql.Append(" AND (:ngay IS NULL OR EXTRACT(DAY FROM o.borrowdate) = :ngay) ");
                parameters.Add(new NpgsqlParameter("ngay", day));
            }

            if (month != null)
            {
                sql.Append("  AND (:thang IS NULL OR EXTRACT(MONTH FROM o.borrowdate) = :thang) ");
                parameters.Add(new NpgsqlParameter("thang", month));
            }

            if (!string.IsNullOrEmpty(typesp))
            {
                sql.Append(" AND p.type_production = :typesp ");
                parameters.Add(new NpgsqlParameter("typesp", typesp));
            }
            sql.Append(" group by rental_date, product_type ORDER BY rental_date ");

            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }

        public ExecutionResult GetDoanhThuMakeUp(string typecheckNV, int year, string? idNV = null, int? month = null, int? day = null)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            sql.Append(@"SELECT 
                               m.scheduleddate::date AS time_group,                               
                               o.namestaff AS staff_name,                               
                               SUM(s.price * s.qty) AS total_revenue
                               
                           FROM clothings.makeupbookings m 
                           join clothings.makeupstaff o on o.staffid = m.idstaffmake
                           join clothings.makeupservices s on s.serviceid = m.bookingid
                           WHERE m.status = 'CLOSE'
                             AND (:year IS NULL OR EXTRACT(YEAR FROM m.scheduleddate) = :year) ");

            parameters.Add(new NpgsqlParameter("year", year));

            if (day != null)
            {
                sql.Append("  AND (:ngay IS NULL OR EXTRACT(DAY FROM m.scheduleddate) = :ngay) ");
                parameters.Add(new NpgsqlParameter("ngay", day));
            }

            if (month != null)
            {
                sql.Append(" AND (:thang IS NULL OR EXTRACT(MONTH FROM m.scheduleddate) = :thang) ");
                parameters.Add(new NpgsqlParameter("thang", month));
            }

            if (typecheckNV != "ALL")
            {
                sql.Append(" AND o.staffid = :idnv ");
                parameters.Add(new NpgsqlParameter("idnv", idNV));
            }
            sql.Append(" GROUP BY time_group,  o.namestaff ORDER BY MIN(m.scheduleddate),  o.namestaff ");

            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }
        public ExecutionResult GetTotalWH(RadioButton all, RadioButton rdNotReturn)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            if (all.Checked)
                sql.Append(@"SELECT type_production, SUM(saveqty) AS total_quantity
                                FROM clothings.products
                                GROUP BY type_production
                                ORDER BY type_production");
            else if (rdNotReturn.Checked)
                sql.Append(@"SELECT type_production, SUM(stockquantity - saveqty) AS total_quantity
                                FROM clothings.products
                                GROUP BY type_production
                                ORDER BY type_production");
            else
                sql.Append(@"SELECT type_production, SUM(stockquantity) AS total_quantity
                                FROM clothings.products
                                GROUP BY type_production
                                ORDER BY type_production");
            return DatabaseConnection.ExecuteQueryDS(sql.ToString());
        }
        public ExecutionResult GetTotalWHBySP(RadioButton all, RadioButton rdNotReturn, string typeSP)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            StringBuilder sql = new StringBuilder();

            if (all.Checked)
            {
                sql.Append(@"SELECT productname as type_production, SUM(saveqty) AS total_quantity
                                FROM clothings.products where type_production=:type_production
                                GROUP BY productname");
                parameters.Add(new NpgsqlParameter("type_production", typeSP));
            }
            else if (rdNotReturn.Checked)
            {
                sql.Append(@"SELECT productname as type_production, SUM(stockquantity - saveqty) AS total_quantity
                                FROM clothings.products where type_production =:type_production
                                GROUP BY productname");
                parameters.Add(new NpgsqlParameter("type_production", typeSP));
            }
            else
            {
                sql.Append(@"SELECT productname as type_production, SUM(stockquantity) AS total_quantity
                                FROM clothings.products where type_production =:type_production
                                GROUP BY productname");
                parameters.Add(new NpgsqlParameter("type_production", typeSP));
            }
            return DatabaseConnection.ExecuteQueryDS(sql.ToString(), parameters.ToArray());
        }
        public DataTable GetUpcomingMakeupBookings(DateTime from, DateTime to)
        {
            string query = @" SELECT m.bookingid, u.fullname as customername, m.scheduleddate, m.locationmake
                                FROM clothings.makeupbookings m join clothings.users u on u.userid = m.userid
                               WHERE m.status = 'OPEN'
                                 AND m.scheduleddate >= :from1
                                 AND m.scheduleddate <= :to1";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("from1", from));
            parameters.Add(new NpgsqlParameter("to1", to));

            ExecutionResult ex = DatabaseConnection.ExecuteQueryDS(query.ToString(), parameters.ToArray());
            return ((DataSet)ex.Anything).Tables[0];
        }

    }
}
