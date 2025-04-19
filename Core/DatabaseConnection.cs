using LuongEmStudio.BaseData;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuongEmStudio.Core
{
    class DatabaseConnection
    {
        public string connectionString = "Host=ep-hidden-shadow-a5zngo73-pooler.us-east-2.aws.neon.tech;Database=KidoStudio;Username=neondb_owner;Password=npg_zvbXYL5BZy7x;SSL Mode=Require";

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
        public ExecutionResult ExecuteQueryDS(string query, NpgsqlParameter[] parameters = null)
        {
            ExecutionResult exeResult = new ExecutionResult();
            DataSet dataSet = new DataSet();

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet);
                        }
                        exeResult.Status = true;
                        exeResult.Message = "OK";
                        exeResult.Anything = dataSet;
                    }
                }
            }
            catch (Exception ex)
            {
                exeResult.Status = false;
                exeResult.Message = "NG" + ex.Message;
            }
            return exeResult;
        }
        public int ExecuteQueryInt(string query, NpgsqlConnection conn)
        {
            int count = 0;
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        count = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            return count;
        }
        public ExecutionResult ExecuteUpdate(string query, NpgsqlParameter[] parameters = null)
        {
            ExecutionResult exeResult = new ExecutionResult();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (var cmd = new NpgsqlCommand(query, conn, transaction))
                    {
                        try
                        {
                            if (parameters != null)
                                cmd.Parameters.AddRange(parameters);

                            int affectedRows = cmd.ExecuteNonQuery();
                            transaction.Commit();
                            exeResult.Status = true;
                            exeResult.Message = "OK";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            exeResult.Status = false;
                            exeResult.Message = "NG" + ex.Message;
                        }
                    }

                }
            }
            return exeResult;
        }

        public ExecutionResult ExecuteMultiUpdate(List<string> queries, List<NpgsqlParameter[]> parametersList)
        {
            ExecutionResult exeResult = new ExecutionResult();

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) // Mở transaction duy nhất
                {
                    try
                    {
                        for (int i = 0; i < queries.Count; i++)
                        {
                            using (var cmd = new NpgsqlCommand(queries[i], conn, transaction))
                            {
                                if (parametersList[i] != null)
                                    cmd.Parameters.AddRange(parametersList[i]);

                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit(); // Chỉ commit sau khi tất cả câu lệnh thành công
                        exeResult.Status = true;
                        exeResult.Message = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Nếu có lỗi, rollback toàn bộ
                        exeResult.Status = false;
                        exeResult.Message = "NG: " + ex.Message;
                    }
                }
            }
            return exeResult;
        }

    }
}
