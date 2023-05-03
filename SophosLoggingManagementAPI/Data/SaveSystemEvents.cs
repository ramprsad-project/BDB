using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;

namespace SophosLoggingManagementAPI.Data
{
    public static class SaveSystemEvents
    {
        public static int SavesystemEventsToDB(string event_id, string severity, string name, string location, string type, string created_at, string source_info_ip, string customer_id, string endpoint_type, string endpoint_id, string user_id, string when_occured, string source, string group_action)
        {
            string connstring = String.Format("Server={0}; Port={1};" +
                "User Id = {2}; Password={3};Database={4};",
                "52.234.227.225", "5432", "postgres", "BigDog2021!!", "syslog");
            NpgsqlConnection dbcon = new NpgsqlConnection(connstring);
            dbcon.Open();
            NpgsqlCommand dbcmd = dbcon.CreateCommand();
            try
            {
                string sql1 = "INSERT INTO sophossystemevents(event_id, severity, name, location, type, created_at, source_info_ip, customer_id, endpoint_type, endpoint_id, user_id, when_occured, source, group_action) " +
                " VALUES (" + event_id + ",'" + severity + "','" + name + "','" + location + "','" + type + "','" + created_at + "','" + source_info_ip + "','" + customer_id + "','" + endpoint_type + "','" + endpoint_id + "','" + user_id + "','" + when_occured + "','" + source + "','" + group_action"')";
                dbcmd.CommandText = sql1;
                dbcmd.ExecuteNonQuery();
                return 1;
            }
            catch (NpgsqlException ex)
            {
                if (ex.Data == null)
                { return 0; }
                else
                { return 0; }
            }
            finally { dbcon.Close(); }
        }
    }
}
