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

        public int DeleteShoppingList(int id)
        {
            return Execute("DELETE FROM [ShoppingLists] WHERE [Id] = ?;", id);
        }

        public IEnumerable<ShoppingList> GetAllShoppingLists(int count = 0)
        {
            var lists = new List<ShoppingList>();
            var sql = "SELECT [Id], [Name] FROM [ShoppingLists]";
            if (count > 0)
                sql += " ORDER BY Id DESC LIMIT " + count;

            var results = Query(sql, new object[] { }, MapToList);
            lists.AddRange(results);

            return lists;
        }

        public int SaveShoppingList(ShoppingList item)
        {
            int result;
            if (item.Id != 0)
            {
                result = Execute("UPDATE [ShoppingLists] SET [Name] = ? WHERE [Id] = ?;", item.Name, item.Id);
                return result;
            }

            result = Execute("INSERT INTO [ShoppingLists] ([Name]) VALUES (?)", item.Name);
            item.Id = Convert.ToInt32(Scalar("SELECT last_insert_rowid()"));
            return result;
        }

        private static ShoppingList MapToList(IDataRecord reader)
        {
            return new ShoppingList
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
                CreateCommand(@"CREATE TABLE [ShoppingLists] (
                                [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
                                [Name]  TEXT NOT NULL
                            );"),
                CreateCommand(@"CREATE TABLE [Products] (
                                [Id]    INTEGER PRIMARY KEY AUTOINCREMENT,
                                [Name]  TEXT NOT NULL
                            );"),
                CreateCommand(@"CREATE TABLE [ShoppingListProducts] (
                                [ShoppingListId]    INTEGER REFERENCES Lists(Id),
                                [ProductId]    INTEGER REFERENCES Items(Id),
                                PRIMARY KEY(ShoppingListId,ProductId)
                            );")
            };
            Execute(commands);
        }
    }
}