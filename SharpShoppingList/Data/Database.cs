using Mono.Data.Sqlite;
using SharpShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SharpShoppingList.Data
{
    public class Database
    {
        private static readonly object Locker = new object();
        private readonly string _connectionString;

        public Database(string dbPath)
        {
            _connectionString = "Data Source=" + dbPath;
            InitializeDatabase(dbPath);
        }

        public int DeleteItem(int id)
        {
            lock (Locker)
            {
                int result;
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM [Lists] WHERE [Id] = ?;";
                        command.Parameters.Add(new SqliteParameter(DbType.Int32) { Value = id });
                        result = command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return result;
            }
        }

        public IEnumerable<List> GetLists(int count = 0)
        {
            var lists = new List<List>();

            lock (Locker)
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    using (var contents = connection.CreateCommand())
                    {
                        var command = "SELECT [Id], [Name] from [Lists]";
                        if (count > 0)
                            command += " ORDER BY Id DESC LIMIT " + count;

                        contents.CommandText = command;

                        var reader = contents.ExecuteReader();
                        while (reader.Read())
                        {
                            lists.Add(FromReader(reader));
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }
            return lists;
        }

        public int SaveItem(List item)
        {
            lock (Locker)
            {
                int result;
                if (item.Id != 0)
                {
                    using (var connection = new SqliteConnection(_connectionString))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE [Lists] SET [Name] = ? WHERE [Id] = ?;";
                            command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                            result = command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    return result;
                }
                else
                {
                    using (var connection = new SqliteConnection(_connectionString))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "INSERT INTO [Lists] ([Name]) VALUES (?)";
                            command.Parameters.Add(new SqliteParameter(DbType.String) { Value = item.Name });
                            result = command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    return result;
                }
            }
        }

        private static List FromReader(SqliteDataReader reader)
        {
            return new List
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString()
            };
        }

        private void InitializeDatabase(string path)
        {
            // create the tables
            var exists = File.Exists(path);
            if (exists) return;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var commands = new[]
                {
                    @"CREATE TABLE [Lists] (
    [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
    [Name]  TEXT NOT NULL
);",
                    @"CREATE TABLE [Items] (
    [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
    [Name]  TEXT NOT NULL
);",
                    @"CREATE TABLE [ListItems] (
    `ListId`    INTEGER REFERENCES Lists(Id),
    `ItemId`    INTEGER REFERENCES Items(Id),
    PRIMARY KEY(ListId,ItemId)
);"
                };
                foreach (var command in commands)
                {
                    using (var c = connection.CreateCommand())
                    {
                        c.CommandText = command;
                        var result = c.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }
    }
}