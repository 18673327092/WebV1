﻿using System.Data;
using System.Data.SqlClient;

namespace Utility.WeChat
{
    public static class DBHelper
    {

        public static void InsertLog(string content)
        {
            string sql = "insert Log(LogContent, CreateDate) values('" + content + "',getdate())";
            DBHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private readonly static string config = "Data Source=120.27.105.96;database=CRMLog;User ID=log;Password=log";
        /// <summary>
        /// 离线查询，返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static DataTable ExecuteTable(string sql, params SqlParameter[] par)
        {
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, config))
            {
                if (par != null && par.Length > 0)
                {
                    sda.SelectCommand.Parameters.AddRange(par);
                }
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }

        }
        /// <summary>
        /// 查询首行首列，返回object
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] par)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    if (par != null && par.Length > 0)
                    {
                        com.Parameters.AddRange(par);
                    }
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return com.ExecuteScalar();
                }
            }
        }
        /// <summary>
        /// 在线查询，返回SqlDataReader，存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader1(string procname, params SqlParameter[] par)
        {
            SqlConnection con = new SqlConnection(config);

            using (SqlCommand com = new SqlCommand(procname, con))
            {
                com.CommandType = CommandType.StoredProcedure;
                if (par != null && par.Length > 0)
                {
                    com.Parameters.AddRange(par);
                }
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return com.ExecuteReader(CommandBehavior.CloseConnection);

            }
        }
        /// <summary>
        /// 在线查询，返回SqlDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string procname, params SqlParameter[] par)
        {
            SqlConnection con = new SqlConnection(config);

            using (SqlCommand com = new SqlCommand(procname, con))
            {

                if (par != null && par.Length > 0)
                {
                    com.Parameters.AddRange(par);
                }
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return com.ExecuteReader(CommandBehavior.CloseConnection);

            }
        }
        /// <summary>
        /// 增删改方法
        /// </summary>
        /// <param name="sql"></param>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] par)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    if (par != null && par.Length > 0)
                    {
                        com.Parameters.AddRange(par);
                    }
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return com.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 增删改方法，存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryProc(string sql, params SqlParameter[] par)
        {
            using (SqlConnection con = new SqlConnection(config))
            {
                using (SqlCommand com = new SqlCommand(sql, con))
                {
                    com.CommandTimeout = 60;
                    com.CommandType = CommandType.StoredProcedure;
                    if (par != null && par.Length > 0)
                    {
                        com.Parameters.AddRange(par);
                    }
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    return com.ExecuteNonQuery();
                }
            }
        }
    }
}