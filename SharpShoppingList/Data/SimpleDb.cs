using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace SharpShoppingList.Data
{
    public abstract class SimpleDb
    {
        protected abstract string ConnectionString { get; }

        protected IDbCommand CreateCommand(string sql, params object[] args)
        {
            var command = new SqliteCommand(sql);
            if (args.Length > 0)
                command.AddParameters(args);
            return command;
        }

        protected int Execute(string sql, params object[] args)
        {
            return Execute(new[] { CreateCommand(sql, args) });
        }

        protected int Execute(IEnumerable<IDbCommand> commands)
        {
            var result = 0;
            using (var connection = OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var command in commands)
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        result += command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return result;
        }

        protected IEnumerable<T> Query<T>(string sql, object[] args, Func<IDataReader, T> mapFunc)
        {
            using (var connection = OpenConnection())
            {
                var command = CreateCommand(sql, args);
                command.Connection = connection;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return mapFunc(reader);
                }
            }
        }

        protected object Scalar(string sql, params object[] args)
        {
            object result;
            using (var connection = OpenConnection())
            {
                var command = CreateCommand(sql, args);
                command.Connection = connection;
                result = command.ExecuteScalar();
            }
            return result;
        }

        private IDbConnection OpenConnection()
        {
            var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}