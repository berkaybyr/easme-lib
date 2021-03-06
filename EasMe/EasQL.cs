using EasMe.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EasMe
{
    /// <summary>
    /// SQL helper, used to execute SQL queries, and get data from SQL database.
    /// </summary>
    public class EasQL
    {
        private static string? Connection { get; set; }

        public EasQL(string connection)
        {
            if (!connection.IsValidConnectionString()) throw new NotValidException("EasQL given connection string is not valid");
            Connection = connection;
        }
        /// <summary>
        /// Executes SQL query and returns DataTable.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public DataTable GetTable(SqlCommand cmd, int Timeout = 0)
        {
            return GetTable(Connection, cmd, Timeout);
        }
        /// <summary>
        /// Exectues SQL query and returns affected row count.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public int ExecNonQuery(SqlCommand cmd, int Timeout = 0)
        {
            return ExecNonQuery(Connection, cmd, Timeout);
        }

        /// <summary>
        /// Exectues SQL query and returns the first column of first row in the result set returned by query. Additional columns or rows ignored.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static object ExecScalar(SqlCommand cmd, int Timeout = 0)
        {
            return ExecScalar(Connection, cmd, Timeout);
        }

        /// <summary>
        /// Executes a SQL query to backup database to the given folder path.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="BackupFolderPath"></param>
        /// <param name="Timeout"></param>
        /// <exception cref="EasException"></exception>
        public void BackupDatabase(string DatabaseName, string BackupPath, int Timeout = 0)
        {
            BackupDatabase(Connection, DatabaseName, BackupPath, Timeout);
        }
        /// <summary>
        /// Executes a SQL query to shrink your database and SQL log data. This action will not lose you any real data but still you should backup first.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="DatabaseLogName"></param>
        /// <exception cref="EasException"></exception>
        public void ShrinkDatabase(string DatabaseName, string DatabaseLogName = "_log")
        {
            ShrinkDatabase(Connection, DatabaseName, DatabaseLogName);
        }



        /// <summary>
        /// Deletes all records in given table but keeps the table. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="TableName"></param>
        /// <exception cref="EasException"></exception>
        public void TruncateTable(string TableName)
        {
            TruncateTable(Connection, TableName);
        }



        /// <summary>
        /// Deletes all records in the table and the table from database. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="TableName"></param>
        /// <exception cref="EasException"></exception>
        public void DropTable(string TableName)
        {
            DropTable(Connection, TableName);
        }


        /// <summary>
        /// Deletes all records and all tables and the database entirely. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <exception cref="EasException"></exception>
        public void DropDatabase(string DatabaseName)
        {
            DropDatabase(Connection, DatabaseName);
        }

        /// <summary>
        /// Gets all table names in SQL database and returns.
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public List<string> GetAllTableName()
        {
            return GetAllTableName(Connection);
        }


        #region EasQL Static Methods
        /// <summary>
        /// Executes SQL query and returns DataTable.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static DataTable GetTable(string Connection, SqlCommand cmd, int Timeout = 0)
        {
            DataTable dt = new();
            using (SqlConnection conn = new SqlConnection(Connection))
            {
                try
                {
                    conn.Open();
                    cmd.Connection = conn;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = Timeout;
                    da.Fill(dt);
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception e)
                {
                    throw new SqlErrorException("GetTable failed SQL Query: " + cmd.CommandText, e);
                }

            }
            return dt;
        }

        /// <summary>
        /// Exectues SQL query and returns affected row count.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static int ExecNonQuery(string Connection, SqlCommand cmd, int Timeout = 0)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new(Connection))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandTimeout = Timeout;
                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception e)
                {
                    throw new SqlErrorException("ExecNonQuery failed SQL Query: " + cmd.CommandText, e);

                }

            }
            return rowsAffected;
        }
        /// <summary>
        /// Exectues SQL query and returns the first column of first row in the result set returned by query. Additional columns or rows ignored.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="cmd"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static object ExecScalar(string Connection, SqlCommand cmd, int Timeout = 0)
        {
            var obj = new object();
            using (SqlConnection conn = new(Connection))
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandTimeout = Timeout;
                    conn.Open();
                    obj = cmd.ExecuteScalar();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception e)
                {
                    throw new SqlErrorException("ExecScalar failed SQL Query: " + cmd.CommandText, e);

                }
            }
            return obj;
        }

        /// <summary>
        /// Executes a SQL query to backup database to the given folder path.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="BackupFolderPath"></param>
        /// <param name="Timeout"></param>
        /// <exception cref="EasException"></exception>
        public static void BackupDatabase(string Connection, string DatabaseName, string BackupFolderPath, int Timeout = 0)
        {
            try
            {
                if (!Directory.Exists(BackupFolderPath)) Directory.CreateDirectory(BackupFolderPath);
                string query = $@"BACKUP DATABASE {DatabaseName} TO DISK = '{BackupFolderPath}\{DatabaseName}.bak'";
                var cmd = new SqlCommand(query);
                ExecNonQuery(Connection, cmd, Timeout);
            }
            catch (Exception e)
            {
                throw new SqlErrorException("BackupDatabase failed: " + DatabaseName, e);
            }
        }
        /// <summary>
        /// Executes a SQL query to shrink your database and SQL log data. This action will not lose you any real data but still you should backup first.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="DatabaseLogName"></param>
        /// <exception cref="EasException"></exception>
        public static void ShrinkDatabase(string Connection, string DatabaseName, string DatabaseLogName = "_log")
        {
            try
            {
                if (DatabaseLogName == "_log") DatabaseLogName = DatabaseName + DatabaseLogName;
                string query = $@"BEGIN
                                ALTER DATABASE [{DatabaseName}] SET RECOVERY SIMPLE WITH NO_WAIT
                                DBCC SHRINKFILE(N'{DatabaseLogName}', 1)
                                ALTER DATABASE [{DatabaseName}] SET RECOVERY FULL WITH NO_WAIT
                            END
                            BEGIN
                                ALTER DATABASE [{DatabaseName}] SET RECOVERY SIMPLE WITH NO_WAIT
                                DBCC SHRINKFILE(N'{DatabaseName}', 1)
                                ALTER DATABASE [{DatabaseName}] SET RECOVERY FULL WITH NO_WAIT
                            END";
                var cmd = new SqlCommand(query);
                ExecNonQuery(Connection, cmd);
            }
            catch (Exception e)
            {
                throw new SqlErrorException("ShrinkDatabase failed: " + DatabaseName, e);
            }
        }
        /// <summary>
        /// Deletes all records in given table but keeps the table. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="TableName"></param>
        /// <exception cref="EasException"></exception>
        public static void TruncateTable(string Connection, string TableName)
        {

            try
            {
                string query = $@"TRUNCATE TABLE {TableName}";
                var cmd = new SqlCommand(query);
                ExecNonQuery(Connection, cmd);
            }
            catch (Exception e)
            {
                throw new SqlErrorException("TruncateTable failed: " + TableName, e);
            }
        }
        /// <summary>
        /// Deletes all records in the table and the table from database. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="TableName"></param>
        /// <exception cref="EasException"></exception>
        public static void DropTable(string Connection, string TableName)
        {
            try
            {
                string query = $@"DROP TABLE {TableName}";
                var cmd = new SqlCommand(query);
                ExecNonQuery(Connection, cmd);
            }
            catch (Exception e)
            {
                throw new SqlErrorException("DropTable failed: " + TableName, e);
            }
        }
        /// <summary>
        /// Deletes all records and all tables and the database entirely. This action can not be undone, be aware of the risks before running this.
        /// </summary>
        /// <param name="Connection"></param>
        /// <param name="DatabaseName"></param>
        /// <exception cref="EasException"></exception>
        public static void DropDatabase(string Connection, string DatabaseName)
        {
            try
            {
                string query = $@"DROP DATABASE {DatabaseName}";
                var cmd = new SqlCommand(query);
                ExecNonQuery(Connection, cmd);
            }
            catch (Exception e)
            {
                throw new SqlErrorException("DropDatabase failed: " + DatabaseName, e);
            }
        }
        /// <summary>
        /// Gets all table names in SQL database and returns.
        /// </summary>
        /// <param name="Connection"></param>
        /// <returns></returns>
        /// <exception cref="EasException"></exception>
        public static List<string> GetAllTableName(string Connection)
        {
            try
            {
                string query = $@"SELECT '['+SCHEMA_NAME(schema_id)+'].['+name+']' FROM sys.tables";
                var list = new List<string>();
                SqlCommand cmd = new SqlCommand(query);
                var dt = GetTable(Connection, cmd);
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(row[0].ToString());
                }
                return list;
            }
            catch (Exception e)
            {
                throw new SqlErrorException("GetAllTableName failed.", e);
            }
        }

        public static List<string> GetColumns(string connection, string tableName)
        {
            try
            {
                var list = new List<string>();
                string query = $@"SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('{tableName}')";
                SqlCommand cmd = new(query);
                var dt = GetTable(connection, cmd);
                foreach (DataRow row in dt.Rows)
                {
                    if (row != null)
                    {
                        var columnName = row[0].ToString();
                        if (!string.IsNullOrEmpty(columnName))
                        {
                            list.Add(columnName);
                        }
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                throw new SqlErrorException("GetColumns failed.", e);
            }
        }
        #endregion




    }
}
