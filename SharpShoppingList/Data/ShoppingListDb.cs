using System;
using System.Collections.Generic;
using System.Data;
using SharpShoppingList.Models;
using System.IO;

namespace SharpShoppingList.Data
{
    public class ShoppingListDb : SimpleDb
    {
        public ShoppingListDb(string dbPath)
            : base("Data Source=" + dbPath)
        {
            InitializeDatabase(dbPath);
        }

        public int DeleteItem(int id)
        {
            return Execute("DELETE FROM [Lists] WHERE [Id] = ?;", id);
        }

        public IEnumerable<List> GetLists(int count = 0)
        {
            var lists = new List<List>();
            var sql = "SELECT [Id], [Name] FROM [Lists]";
            if (count > 0)
                sql += " ORDER BY Id DESC LIMIT " + count;

            var results = Query(sql, new object[] { }, MapToList);
            lists.AddRange(results);

            return lists;
        }

        public int SaveItem(List item)
        {
            int result;
            if (item.Id != 0)
            {
                result = Execute("UPDATE [Lists] SET [Name] = ? WHERE [Id] = ?;", item.Name, item.Id);
                return result;
            }

            result = Execute("INSERT INTO [Lists] ([Name]) VALUES (?)", item.Name);
            item.Id = Convert.ToInt32(Scalar("SELECT last_insert_rowid()"));
            return result;
        }

        private static List MapToList(IDataRecord reader)
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

            var commands = new[]
            {
                CreateCommand(@"CREATE TABLE [Lists] (
                                [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
                                [Name]  TEXT NOT NULL
                            );"),
                CreateCommand(@"CREATE TABLE [Items] (
                                [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
                                [Name]  TEXT NOT NULL
                            );"),
                CreateCommand(@"CREATE TABLE [ListItems] (
                                [ListId]    INTEGER REFERENCES Lists(Id),
                                [ItemId]    INTEGER REFERENCES Items(Id),
                                PRIMARY KEY(ListId,ItemId)
                            );")
            };
            Execute(commands);
        }
    }
}