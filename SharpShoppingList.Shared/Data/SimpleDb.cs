﻿#if __ANDROID__
using Mono.Data.Sqlite;
#else

using System.Data.SqlClient;

#endif

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
#if __ANDROID__
            var command = new SqliteCommand(sql);
#else
            var command = new SqlCommand();
#endif
            if (args.Length > 0)
                command.AddParameters(args);
            return command;
        }

        protected int Execute(string sql, params object[] args)
        {
            return Execute(new[] { CreateCommand(sql, args) });
        }

        protected int Execute(IDbConnection connection, string sql, params object[] args)
        {
            return Execute(connection, new[] { CreateCommand(sql, args) });
        }

        protected int Execute(IEnumerable<IDbCommand> commands)
        {
            var result = 0;
            using (var connection = OpenConnection())
            {
                result += Execute(connection, commands);
            }
            return result;
        }

        protected static int Execute(IDbConnection connection, IEnumerable<IDbCommand> commands)
        {
            var result = 0;
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
                result = Scalar(connection, sql, args);
            }
            return result;
        }

        protected object Scalar(IDbConnection connection, string sql, params object[] args)
        {
            var command = CreateCommand(sql, args);
            command.Connection = connection;
            var result = command.ExecuteScalar();
            return result;
        }

        protected IDbConnection OpenConnection()
        {
#if __ANDROID__
            var connection = new SqliteConnection(ConnectionString);
#else
            var connection = new SqlConnection();
#endif
            connection.Open();
            return connection;
        }
    }
}