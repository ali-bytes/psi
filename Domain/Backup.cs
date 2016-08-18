using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Db;
using NewIspNL.Helpers;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace NewIspNL.Domain
{
    public class Backup
    {
        public string StartBackup()
        {
            var backUp = new Db.Backup
            {
                Time = DateTime.Now.AddHours()

            };
            using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                context.Backups.InsertOnSubmit(backUp);
                context.SubmitChanges();
                var bck = context.Backups.FirstOrDefault(x => x.Id == backUp.Id);
                bck.Url = backUp.Id + ".Bak";
                
                context.SubmitChanges();
            }

            //new code

            var sqlcon = new SqlConnection
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
            };
            var dbName = sqlcon.Database;
            string destination = HttpContext.Current.Server.MapPath("~/DbBackup");
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            //Define a Backup object variable.
            Microsoft.SqlServer.Management.Smo.Backup sqlBackup = new Microsoft.SqlServer.Management.Smo.Backup();
 
            //Specify the type of backup, the description, the name, and the database to be backed up.
            sqlBackup.Action = BackupActionType.Database;
            sqlBackup.BackupSetDescription = "BackUp of:" + dbName + "on" + DateTime.Now.ToShortDateString();
            sqlBackup.BackupSetName = backUp.Id.ToString();
            sqlBackup.Database = dbName;
 
            //Declare a BackupDeviceItem
            BackupDeviceItem deviceItem = new BackupDeviceItem(destination +"\\" + backUp.Id + ".bak", DeviceType.File);
            //Define Server connection
            ServerConnection connection = new ServerConnection(sqlcon);
            //To Avoid TimeOut Exception
            Server sqlServer = new Server(connection);
            sqlServer.ConnectionContext.StatementTimeout = 60 * 60;
            Database db = sqlServer.Databases[dbName];
 
            sqlBackup.Initialize = true;
            sqlBackup.Checksum = true;
            sqlBackup.ContinueAfterError = true;
 
            //Add the device to the Backup object.
            sqlBackup.Devices.Add(deviceItem);
            //Set the Incremental property to False to specify that this is a full database backup.
            sqlBackup.Incremental = false;
 
            sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
            //Specify that the log must be truncated after the backup is complete.
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;
 
            sqlBackup.FormatMedia = false;
            //Run SqlBackup to perform the full database backup on the instance of SQL Server.
            sqlBackup.SqlBackup(sqlServer);
            //Remove the backup device from the Backup object.
            sqlBackup.Devices.Remove(deviceItem);

            return "تم الحفظ";




            //old code
            //var sqlcon = new SqlConnection
            //{
            //    ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
            //};
            //var dbName = sqlcon.Database;
            //string destination = HttpContext.Current.Server.MapPath("~/DbBackup");
            //if (!Directory.Exists(destination))
            //{
            //    Directory.CreateDirectory(destination);
            //}
            //try
            //{

            //    sqlcon.Open();


            //    var sqlcmd = new SqlCommand("backup database " + dbName + " to disk='" + destination + "\\" + backUp.Id + ".Bak'", sqlcon);
            //    sqlcmd.ExecuteNonQuery();
            //    sqlcon.Close();
            //    return "تم الحفظ";
            //}
            //catch (Exception exception)
            //{
            //    return "Error back up" + exception.Message;
            //}
        }

        //public string StartRestore(string newDbName)
        //{
        //    var sqlcon = new SqlConnection
        //    {
        //        ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
        //        //ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=master;Integrated Security=True"
        //    };

        //    string destination = HttpContext.Current.Server.MapPath("~/DbBackup/" + newDbName + ".Bak");
        //    //if (destination != null && !Directory.Exists(destination))
        //    //{
        //    //    Directory.CreateDirectory(destination);
        //    //}
        //    try
        //    {
        //        // ali
        //        Restore sqlRestore = new Restore();
        //        // new database name
        //        sqlRestore.Database = sqlcon.Database + "New";
        //        sqlRestore.Action = RestoreActionType.Database;
        //        BackupDeviceItem deviceItem = new BackupDeviceItem(destination,DeviceType.File);
        //        sqlRestore.Devices.Add(deviceItem);
        //        //create a new database
        //        sqlRestore.ReplaceDatabase = true;
        //        //the database will be NOT in a restoring state.
        //        sqlRestore.NoRecovery = false;

        //        ServerConnection connection = new ServerConnection(sqlcon);
        //        Server sqlserver = new Server(connection);
        //        //Database db = sqlserver.Databases[sqlcon.Database];

        //        // RelocateFiles collection allows you to specify the logical file names and physical file names (new locations) if you want to restore to a different location
        //        //string dataFileLocation = HttpContext.Current.Server.MapPath("~/DbBackup/" + newDbName + "New.mdf");
        //        //string logFileLocation = HttpContext.Current.Server.MapPath("~/DbBackup/" + newDbName + "New_Log.ldf");

        //        //db = sqlserver.Databases[sqlcon.Database];

        //        //RelocateFile rf = new RelocateFile(sqlcon.Database, dataFileLocation);
        //        //sqlRestore.RelocateFiles.Add(new RelocateFile(sqlcon.Database, dataFileLocation));
        //        //sqlRestore.RelocateFiles.Add(new RelocateFile(sqlcon.Database + "_log.ldf", logFileLocation));
                
        //        sqlRestore.Complete += sqlRestore_Complete;
        //        //sqlRestore.PercentCompleteNotification = 10;
        //        sqlRestore.PercentComplete += sqlRestore_PercentComplete;

        //        sqlRestore.SqlRestore(sqlserver);

        //        //db = sqlserver.Databases[sqlcon.Database];

        //        //db.SetOnline();

        //        sqlserver.Refresh();





        //        //var dbName = sqlcon.Database;
        //        //sqlcon.Open();
        //        ////var sqlcmd = new SqlCommand("RESTORE DATABASE " + dbName + " FROM disk='" + destination + "\\" + newDbName + ".Bak'", sqlcon);
        //        //StringBuilder stb = new StringBuilder();
        //        //stb.AppendLine("ALTER DATABASE " + dbName + " SET SINGLE_USER");
        //        ////stb.AppendLine("WITH ROLLBACK IMMEDIATE");
        //        ////var sqlcmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = '" + destination + "\\" + newDbName + ".Bak'", sqlcon);
        //        //////var sqlcmd2 = new SqlCommand("ALTER DATABASE " + dbName + " SINGLE_USER WITH ROLLBACK IMMEDIATE;", sqlcon);
        //        //var sqlcmd2 = new SqlCommand(stb.ToString(), sqlcon);
        //        //sqlcmd2.CommandTimeout = 0;
        //        //var sqlcmd3 = new SqlCommand("RESTORE DATABASE " + dbName + " FROM disk='" + destination + "\\" + newDbName + ".Bak'", sqlcon);
        //        ////sqlcmd.ExecuteNonQuery();
        //        //sqlcmd2.ExecuteNonQuery();
        //        //sqlcmd3.ExecuteNonQuery();
        //        //var sqlcmdl = new SqlCommand("ALTER DATABASE " + dbName + " SET MULTI_USER GO", sqlcon);
        //        //sqlcmdl.ExecuteNonQuery();
        //        //sqlcon.Close();
        //        return "Done";
        //    }
        //    catch (Exception exception)
        //    {
        //        return exception.Message;
        //    }
        //}
        private static Server srvSql;
        public static String Databs;


        public string StartRestore(string newDbName)
        {
            try
            {
                try
                {
                     var sqlcon = new SqlConnection
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"]
                //ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=master;Integrated Security=True"
            };
                     ServerConnection srvConn = new ServerConnection(sqlcon);
                    Databs = sqlcon.Database;
                    // Log in using SQL authentication instead of Windows authentication
                    ////srvConn.LoginSecure = false;
                    // Give the login username
                    ////srvConn.Login = "sa";
                    // Give the login password
                    ////srvConn.Password = "123";
                    srvSql = new Server(srvConn);
                    // get database name
                    Databs = sqlcon.Database;
                }
                catch { }

                // If there was a SQL connection created
                if (srvSql != null)
                {
                    // If the user has chosen the file from which he wants the database to be restored
                    string destination = HttpContext.Current.Server.MapPath("~/DbBackup/" + newDbName + ".Bak");
                        // Create a new database restore operation
                        Restore rstDatabase = new Restore();
                        // Set the restore type to a database restore
                        rstDatabase.Action = RestoreActionType.Database;
                        // Set the database that we want to perform the restore on
                        rstDatabase.Database = Databs;

                        // Set the backup device from which we want to restore, to a file
                        BackupDeviceItem bkpDevice = new BackupDeviceItem(destination, DeviceType.File);
                        // Add the backup device to the restore type
                        rstDatabase.Devices.Add(bkpDevice);
                        srvSql.KillAllProcesses(rstDatabase.Database);
                        // If the database already exists, replace it
                        rstDatabase.ReplaceDatabase = true;
                        // Perform the restore

                        rstDatabase.SqlRestore(srvSql);
                    return "Done";
                }
                else
                {
                    return "server error";

                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        //public event EventHandler<PercentCompleteEventArgs> PercentComplete;

        //void sqlRestore_PercentComplete(object sender, PercentCompleteEventArgs e)
        //{
        //    if (PercentComplete!=null)
        //    {
        //        PercentComplete(sender,e);
        //    }
        //}

        //public event EventHandler<ServerMessageEventArgs> Complete;
        //void sqlRestore_Complete(object sender, ServerMessageEventArgs e)
        //{
        //    if (Complete != null)
        //    {
        //        Complete(sender, e);
        //    }
        //}
    }
  
}