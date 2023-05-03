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
        public static int SavesystemEventsToDB(string customerid, string receivedat, string devicereportedtime, string facility, string priority, string fromhost, string message, string ntseverity, string importance, string eventsource, string eventuser, string eventcategory, string eventid, string eventbinarydata, string maxavailable, string currusage, string minusage, string maxusage, string syslogtag, string eventlogtype, string genericfilename, string systemid)
        {
            string connstring = String.Format("Server={0}; Port={1};" +
                "User Id = {2}; Password={3};Database={4};",
                "52.234.227.225", "5432", "postgres", "BigDog2021!!", "syslog");
            NpgsqlConnection dbcon = new NpgsqlConnection(connstring);
            dbcon.Open();
            NpgsqlCommand dbcmd = dbcon.CreateCommand();
            try
            {
                string sql1 = "INSERT INTO systemevents(customerid,receivedat, devicereportedtime, facility, priority, fromhost, message, ntseverity, importance, eventsource, eventuser, eventcategory, eventid, eventbinarydata, maxavailable, currusage, minusage, maxusage, infounitid, syslogtag, eventlogtype, genericfilename, systemid) " +
                " VALUES (" + Regex.Replace(customerid, "[^0-9.]", "") + ",'" + receivedat + "','" + devicereportedtime + "','" + 2 +"','" + 1 + "','"+ fromhost + "','"  + message + "','" + 0 + "','" + 0 + "','"+ eventsource + "','" + eventuser +  "','" + 4 + "','"+ 7 +  "','" + eventbinarydata + "','" + 1 + "','"+ 1 + "','" + 3 +"','" + 2 + "','"+ 1 + "','" + syslogtag +  "','" + 1 +"','" + genericfilename + "','" + 2 + "')";
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
