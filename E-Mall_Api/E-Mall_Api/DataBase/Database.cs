using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Database
{
    public static class Database
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["baglanti"].ToString());
        static Database()
        {
            connection.Open();
        }

        public static object GetValue(string sorgu)
        {
            return GetCommand(sorgu).ExecuteScalar();
        }
        public static void InsertValue(string sorgu)
        {
            GetCommand(sorgu).ExecuteNonQuery();
        }
        public static void DeleteValue(string sorgu)
        {
            InsertValue(sorgu);
        }
        public static SqlDataReader GetReader(string sorgu)
        {
            return GetCommand(sorgu).ExecuteReader();
        }

        private static SqlCommand GetCommand(string sorgu)
        {
            return new SqlCommand(sorgu, connection);
        }
    }
}