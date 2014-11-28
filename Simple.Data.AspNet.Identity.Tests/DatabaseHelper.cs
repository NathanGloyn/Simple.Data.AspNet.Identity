using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Simple.Data.AspNet.Identity.Tests
{
    internal static class DatabaseHelper
    {

        public static readonly string ConnectionString = GetConnectionString();

        private static string GetConnectionString() {
            return
                ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"]
                    .ConnectionString;
        }

        public static dynamic Open()
        {
            return Database.Opener.OpenConnection(ConnectionString);
        }

        public static void Reset()
        {
            try {
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = "TestReset";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
