using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using TesonetTask1.Interfaces;
using TesonetTask1.Model;

namespace TesonetTask1.Modules
{
    public class Database
    {
        private readonly string DataSource = "sqlLiteDb.db";
        Logger logger = Logger.GetInstance();

        public Database()
        {
            var path = "Data Source=" + DataSource;
            using (var connection = new SqliteConnection(path))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = 
                    "CREATE TABLE IF NOT EXISTS Servers( name TEXT NOT NULL, distance INTEGER NOT NULL, PRIMARY KEY(name))";
                command.ExecuteNonQuery();
            }
        }

        public DataTable Read(string tableName, List<WhereArgs> args = null)
        {
            var path = "Data Source=" + DataSource;
            string whereArgs = null;
            if (args != null && args.Count > 0)
            {
                whereArgs = " where";
                foreach (var arg in args)
                {
                    whereArgs += " " + arg.argA + arg.symbol + arg.argB;
                }
            }
            try
            {
                using (var connection = new SqliteConnection(path))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM " + tableName + whereArgs;

                    var reader = command.ExecuteReader();

                    var results = new DataTable();
                    results.Load(reader);
                    return results;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw exception;
            }

        }
        /// <summary>
        /// Write row into database
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="Data">Dictionary of column names and values</param>
        public void Write(string tableName, IDictionary<string, object> Data)
        {
            var path = "Data Source=" + DataSource;
            string commandText = "INSERT OR REPLACE INTO " + tableName + "("; //insert or replace to ignore duplicates
            foreach (var key in Data.Keys)
            {
                commandText += key + ",";
            }
            commandText = commandText.TrimEnd(',', ' ');
            commandText += ") VALUES (";
            foreach (var key in Data.Keys)
            {
                commandText += "@" + key + ",";
            }
            commandText = commandText.TrimEnd(',', ' ');
            commandText += ")";
            try
            {
                using (var connection = new SqliteConnection(path))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = commandText;
                    foreach (var key in Data.Keys)
                    {
                        command.Parameters.AddWithValue("@" + key, Data[key]);
                    }
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw exception;
            }

        }

    }
}
