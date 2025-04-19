using LuongEmStudio.BaseData;
using LuongEmStudio.Core;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LuongEmStudio.DataGetWay
{
    public class UpdateGW
    {
        DatabaseConnection DatabaseConnection;
        public string connectionString = "Host=ep-hidden-shadow-a5zngo73-pooler.us-east-2.aws.neon.tech;Database=KidoStudio;Username=neondb_owner;Password=npg_zvbXYL5BZy7x;SSL Mode=Require";
        public UpdateGW()
        {
            DatabaseConnection = new DatabaseConnection();
        }
        public ExecutionResult InserProduct(BaseDataSPInfo product)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string query = @"INSERT INTO clothings.products (ProductID, ProductName, Description, PricePerDay, StockQuantity, Size, Type_production, saveqty,createdate) 
                         VALUES (@productID, @productName, @description, @priceperday, @stockquantity, @size, @typeproduct, @qtysave,@createdate)";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@productID", product.productID),
            new NpgsqlParameter("@productName", product.nameSP),
            new NpgsqlParameter("@description", product.DescSP),
            new NpgsqlParameter("@priceperday", product.PriceSP),
            new NpgsqlParameter("@stockquantity", product.QtySP),
            new NpgsqlParameter("@size", product.sizeSP),
            new NpgsqlParameter("@typeproduct", product.typeSP),
            new NpgsqlParameter("@qtysave", product.QtySP),
            new NpgsqlParameter("@createdate", gioVN)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public ExecutionResult DeleteProduct(BaseDataSPInfo product)
        {
            string query = @" DELETE FROM clothings.products WHERE productid=:productID ";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@productID", product.productID)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public ExecutionResult UpdateProductSP(BaseDataSPInfo product)
        {
            string query = @" Update clothings.products set productid=:productID, productname=:productName, 
                                description=:description, priceperday=:priceperday, stockquantity=:stockquantity, size=:size, type_production=:typeproduct, saveqty=:stockquantity  
                               WHERE productid=:productID ";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@productID", product.productID),
            new NpgsqlParameter("@productName", product.nameSP),
            new NpgsqlParameter("@description", product.DescSP),
            new NpgsqlParameter("@priceperday", product.PriceSP),
            new NpgsqlParameter("@stockquantity", product.QtySP),
            new NpgsqlParameter("@size", product.sizeSP),
            new NpgsqlParameter("@typeproduct", product.typeSP)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }

        public ExecutionResult InserNewStaff(string id, string name, string sdt, string address, string notes)
        {
            string query = @"INSERT INTO clothings.makeupstaff (staffid, namestaff, sdt, status, notes, addressstaff, type) 
                         VALUES (@id, @Name, @sdt,'active',@notes, @address, 'nv' )";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@Name", name),
            new NpgsqlParameter("@sdt", sdt),
            new NpgsqlParameter("@address", address),
            new NpgsqlParameter("@notes", notes)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public ExecutionResult UpdateStaff(string id, string name, string sdt, string address, string notes)
        {
            string query = @"UPDATE clothings.makeupstaff set namestaff =:Name, sdt=:sdt, notes=:notes, addressstaff=:address 
                             WHERE staffid=:id";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@Name", name),
            new NpgsqlParameter("@sdt", sdt),
            new NpgsqlParameter("@address", address),
            new NpgsqlParameter("@notes", notes)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public ExecutionResult DeleteStaff(string id)
        {
            string query = @" DELETE FROM clothings.makeupstaff WHERE staffid=:id ";

            NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@id", id)};

            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }

        public ExecutionResult InsertMakeSchedule(BaseDataMakeInfo MakeInfo, DateTime SCheduleMake, CheckBox cbkhachquen, string contactKH, DateTime timeClose)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            Random rand = new Random();
            int id = int.Parse(DateTime.Now.ToString("HHmmss") + rand.Next(10, 99));
            if (cbkhachquen.Checked)
                id = int.Parse(contactKH);

            List<string> queries = new List<string>
            {
                "INSERT INTO clothings.makeupbookings (bookingid, userid, scheduleddate, status,idstaffmake,locationmake,bookingdate,timeforclose) VALUES (@bookingid, @userid, @scheduleddate, 'OPEN',@idstaffmake, @locationmake,@bookingdate,@timeforclose)",
                "INSERT INTO clothings.makeupservices (serviceid, servicename, description, price, moneycoc, totalprice ,createdate, qty) VALUES (@serviceid, @servicename, @description, @price, @moneycoc, @totalprice,@createdate, @sl)"
            };
    
            List<NpgsqlParameter[]> parametersList = new List<NpgsqlParameter[]>
            {
                new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@bookingid", MakeInfo.BookingID),
                        new NpgsqlParameter("@userid", id),
                        new NpgsqlParameter("@scheduleddate", SCheduleMake),
                        new NpgsqlParameter("@idstaffmake", MakeInfo.IDNVMake),
                        new NpgsqlParameter("@locationmake", MakeInfo.LocationMake),
                        new NpgsqlParameter("@bookingdate", gioVN), new NpgsqlParameter("@timeforclose", timeClose)
                    },

                new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@serviceid", MakeInfo.BookingID),
                    new NpgsqlParameter("@servicename", MakeInfo.NameBooking),
                    new NpgsqlParameter("@description", MakeInfo.DescBooking),
                    new NpgsqlParameter("@price", decimal.Parse(MakeInfo.GoiMake)),
                    new NpgsqlParameter("@moneycoc", decimal.Parse(MakeInfo.Tiencoc)),
                    new NpgsqlParameter("@totalprice", MakeInfo.TotalPrice),
                    new NpgsqlParameter("@createdate", gioVN),
                    new NpgsqlParameter("@sl", MakeInfo.QTYkhach)
                }
            };

            if (!cbkhachquen.Checked)
            {
                queries.Add("INSERT INTO clothings.users (userid, fullname, facebookphone) VALUES (@userid, @fullname, @facebookphone)");
                parametersList.Add(
                new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@userid", id),
                    new NpgsqlParameter("@fullname",MakeInfo.NameKach ),
                    new NpgsqlParameter("@facebookphone",MakeInfo.SdtKhach )
                });
            }

            ExecutionResult result = DatabaseConnection.ExecuteMultiUpdate(queries, parametersList);

            return result;
        }

        public ExecutionResult UpdateMakeSchedule(BaseDataMakeInfo MakeInfo, DateTime SCheduleMake, string userID, DateTime timeClose)
        {
            List<string> queries = new List<string>
            {
                "UPDATE clothings.makeupbookings set  scheduleddate=:scheduleddate, locationmake=:locationmake  where bookingid=:bookingid ",
                "UPDATE clothings.makeupservices set servicename=:servicename, description=:description, price=:price, moneycoc=:moneycoc, totalprice=:totalprice , qty=:sl where serviceid=:serviceid ",
                "UPDATE clothings.users set fullname=:fullname, facebookphone=:facebookphone where userid=:userid"
            };

            List<NpgsqlParameter[]> parametersList = new List<NpgsqlParameter[]>
            {
                new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@bookingid",  long.Parse(BaseDataMakeInfo.IDlichMake)),
                        new NpgsqlParameter("@scheduleddate", SCheduleMake),
                        new NpgsqlParameter("@locationmake", MakeInfo.LocationMake)
                        //new NpgsqlParameter("@timeclosebooking", timeClose)
                    },

                new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@serviceid", long.Parse(BaseDataMakeInfo.IDlichMake)),
                    new NpgsqlParameter("@servicename", MakeInfo.NameBooking),
                    new NpgsqlParameter("@description", MakeInfo.DescBooking),
                    new NpgsqlParameter("@price", decimal.Parse(MakeInfo.GoiMake)),
                    new NpgsqlParameter("@moneycoc", decimal.Parse(MakeInfo.Tiencoc)),
                    new NpgsqlParameter("@totalprice", MakeInfo.TotalPrice),
                    new NpgsqlParameter("@sl", MakeInfo.QTYkhach)
                },

                new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@userid", long.Parse(userID)),
                    new NpgsqlParameter("@fullname",MakeInfo.NameKach ),
                    new NpgsqlParameter("@facebookphone",MakeInfo.SdtKhach )
                }
            };

            ExecutionResult result = DatabaseConnection.ExecuteMultiUpdate(queries, parametersList);

            return result;
        }
        public ExecutionResult CloseOrderDuoDate()
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            string query = @" UPDATE clothings.makeupbookings
                                 SET status = 'CLOSE',
                                     timeclosebooking = @gioclose
                               WHERE status = 'OPEN'
                                 AND @gio >= timeforclose";

            NpgsqlParameter[] parameters = { new NpgsqlParameter("@gio", gioVN), new NpgsqlParameter("@gioclose", gioVN) };
            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public ExecutionResult CancelMakeUp(string id)
        {
            string query = @" UPDATE clothings.makeupbookings
                                 SET status = 'CANCEL'
                                 WHERE bookingid=:id";

            NpgsqlParameter[] parameters = { new NpgsqlParameter("@id", long.Parse(id)) };
            return DatabaseConnection.ExecuteUpdate(query, parameters);
        }
        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            Random rand = new Random();
            int range = (to - from).Days;
            return from.AddDays(rand.Next(range)).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60)).AddSeconds(rand.Next(0, 60));
        }
        public ExecutionResult InsertOrder(List<BaseDataBorrowReturn> danhSachDonHang, CheckBox khquen, string userID)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);

            ExecutionResult result = new ExecutionResult();
            Random rand = new Random();
            int id = int.Parse(DateTime.Now.ToString("HHmmss") + rand.Next(10, 99));
            string NameKach = "", SdtKhach = "";
            if (khquen.Checked)
                id = int.Parse(userID);

            foreach (var don in danhSachDonHang)
            {

                NameKach = don.NameKach;
                SdtKhach = don.SdtKhach;
                List<string> queries = new List<string>
                {
                    "INSERT INTO clothings.orders (orderid, userid, totalamount, status, moneycoc, productid, qty, notes, tienphatsinh,borrowdate) VALUES (@orderid, @userid, @totalamount, 'BORROW', @moneycoc, @productid, @qty, @notess, @tienphatsinh,@borrowdate)",
                    "UPDATE clothings.products set  saveqty=saveqty - :qty where productid=:bookingid "
                };

                List<NpgsqlParameter[]> parametersList = new List<NpgsqlParameter[]>
                {
                    new NpgsqlParameter[]
                        {
                            new NpgsqlParameter("@orderid", long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"))),
                            new NpgsqlParameter("@userid", id),
                            new NpgsqlParameter("@totalamount", don.TotalPrice),
                            new NpgsqlParameter("@moneycoc", don.Tiencoc),
                            new NpgsqlParameter("@productid", don.productID),
                            new NpgsqlParameter("@qty", don.QTYThue),
                            new NpgsqlParameter("@notess", don.notes),
                            new NpgsqlParameter("@tienphatsinh", don.Tienphatsinh),
                             new NpgsqlParameter("@borrowdate", gioVN)
                        },

                    new NpgsqlParameter[]
                    {
                      new NpgsqlParameter("@qty", don.QTYThue),
                      new NpgsqlParameter("@bookingid", don.productID)
                    }
                };
                result = DatabaseConnection.ExecuteMultiUpdate(queries, parametersList);
            }
            if (!khquen.Checked)
                InsertUser(id, NameKach, SdtKhach);
            return result;
        }
        public void InsertUser(int id, string NameKach, string SdtKhach)
        {
            List<string> queries = new List<string>
                {
                    "INSERT INTO clothings.users (userid, fullname, facebookphone) VALUES (@userid, @fullname, @facebookphone)"
                };

            List<NpgsqlParameter[]> parametersList = new List<NpgsqlParameter[]>
                {
                    new NpgsqlParameter[]
                    {
                        new NpgsqlParameter("@userid", id),
                        new NpgsqlParameter("@fullname",NameKach ),
                        new NpgsqlParameter("@facebookphone",SdtKhach )
                    }
                };
            DatabaseConnection.ExecuteMultiUpdate(queries, parametersList);
        }

        public ExecutionResult DoneOrder(List<Tuple<long, int, long, decimal>> orderList)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            ExecutionResult exe = new ExecutionResult();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var id in orderList)
                        {
                            using (var cmd = new NpgsqlCommand("UPDATE clothings.orders SET status='RETURN', returndate=@timereturn, lastmoney=@lastmoney WHERE orderid=@idorder", conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@idorder", id.Item1);
                                cmd.Parameters.AddWithValue("@timereturn", gioVN);
                                cmd.Parameters.AddWithValue("@lastmoney", id.Item4);
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = new NpgsqlCommand("UPDATE clothings.products SET  saveqty=saveqty + :sl where productid=:bookingid ", conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@bookingid", id.Item3);
                                cmd.Parameters.AddWithValue("@sl", id.Item2);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        exe.Status = true;
                        exe.Message = "Trả đồ thành công";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        exe.Status = false;
                        exe.Message = "Trả đồ lỗi";
                    }
                }
            }
            return exe;
        }
        public ExecutionResult CancelOrder(List<Tuple<long, int, long>> orderList)
        {
            TimeZoneInfo vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime gioVN = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnZone);
            ExecutionResult exe = new ExecutionResult();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var id in orderList)
                        {
                            using (var cmd = new NpgsqlCommand("UPDATE clothings.orders SET status='CANCEL', returndate=@timereturn WHERE orderid=@idorder", conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@idorder", id.Item1);
                                cmd.Parameters.AddWithValue("@timereturn", gioVN);
                                cmd.ExecuteNonQuery();
                            }
                            using (var cmd = new NpgsqlCommand("UPDATE clothings.products SET  saveqty=saveqty + :sl where productid=:bookingid ", conn))
                            {
                                cmd.Transaction = transaction;
                                cmd.Parameters.AddWithValue("@bookingid", id.Item3);
                                cmd.Parameters.AddWithValue("@sl", id.Item2);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                        exe.Status = true;
                        exe.Message = "Hủy đơn thành công";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        exe.Status = false;
                        exe.Message = "Hủy đơn lỗi";
                    }
                }
            }
            return exe;
        }
    }
}


