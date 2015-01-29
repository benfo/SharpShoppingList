using SharpShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SharpShoppingList.Data
{
    public class ShoppingListDb : SimpleDb
    {
        private const string ShoppingListTable = "ShoppingLists";
        private const string ProductTable = "Products";

        private readonly string _connectionString;

        public ShoppingListDb()
        {
            var dbPath = new DatabaseFilenameProvider("SharpShoppingList.db3").DatabaseFilePath;
            _connectionString = "Data Source=" + dbPath;

            InitializeDatabase(dbPath);
        }

        protected override string ConnectionString
        {
            get { return _connectionString; }
        }

        public int DeleteShoppingList(int id)
        {
            var query = QueryBuilder
                .Delete()
                .From(ShoppingListTable)
                .Where("Id")
                .Build();

            return Execute(query, id);
        }

        public IEnumerable<ShoppingList> GetAllShoppingLists(int count = 0)
        {
            var lists = new List<ShoppingList>();
            var queryBuilder = QueryBuilder
                .Select("Id", "Name")
                .From(ShoppingListTable);

            if (count > 0)
            {
                queryBuilder
                    .OrderByDescending("Id")
                    .Take(count);
            }

            var results = Query(queryBuilder.Build(), new object[] { }, MapToList);
            lists.AddRange(results);

            return lists;
        }

        public int SaveShoppingList(ShoppingList item)
        {
            if (item.Id != 0)
            {
                return UpdateShoppingList(item);
            }

            return InsertShoppingList(item);
        }

        private int InsertShoppingList(ShoppingList item)
        {
            var insertQuery = QueryBuilder
                .InsertInto(ShoppingListTable, "Name")
                .Build();
            var getIdQuery = QueryBuilder
                .Select("last_insert_rowid()")
                .Build();

            using (var connection = OpenConnection())
            {
                var result = Execute(connection, insertQuery, item.Name);
                item.Id = Convert.ToInt32(Scalar(connection, getIdQuery));
                return result;
            }
        }

        private int UpdateShoppingList(ShoppingList item)
        {
            var updateQuery = QueryBuilder
                .Update(ShoppingListTable)
                .Set("Name")
                .Where("Id")
                .Build();

            var result = Execute(updateQuery, item.Name, item.Id);
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

        public int SaveProduct(Product item)
        {
            if (item.Id != 0)
            {
                return UpdateProduct(item);
            }

            return InsertProduct(item);
        }

        private int UpdateProduct(Product item)
        {
            var updateQuery = QueryBuilder
               .Update(ProductTable)
               .Set("Name")
               .Where("Id")
               .Build();

            var result = Execute(updateQuery, item.Name, item.Id);
            return result;
        }      

        private int InsertProduct(Product item)
        {
            var insertQuery = QueryBuilder
               .InsertInto(ProductTable, "Name")
               .Build();
            var getIdQuery = QueryBuilder
                .Select("last_insert_rowid()")
                .Build();

            using (var connection = OpenConnection())
            {
                var result = Execute(connection, insertQuery, item.Name);
                item.Id = Convert.ToInt32(Scalar(connection, getIdQuery));
                return result;
            }
        }

        public int DeleteProduct(int id)
        {
            var query = QueryBuilder
                .Delete()
                .From(ProductTable)
                .Where("Id")
                .Build();

            return Execute(query, id);
        }

        private void InitializeDatabase(string path)
        {
            // create the tables
            var exists = File.Exists(path);
            if (exists)
                return;

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
                                [ShoppingListId]    INTEGER REFERENCES ShoppingLists(Id),
                                [ProductId]    INTEGER REFERENCES Products(Id),
                                PRIMARY KEY(ShoppingListId,ProductId)
                            );")
            };
            Execute(commands);
        }
    }
}