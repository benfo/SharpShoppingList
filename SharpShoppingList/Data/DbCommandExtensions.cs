using System;
using System.Data;

namespace SharpShoppingList.Data
{
    public static class DbCommandExtensions
    {

        public static void AddParameters(this IDbCommand command, params object[] args)
        {
            foreach (var item in args)
            {
                AddParameter(command, item);
            }
        }

        public static void AddParameter(this IDbCommand command, object item)
        {
            const int maxSize = 4000;

            var parameter = command.CreateParameter();
            parameter.ParameterName = string.Format("@{0}", command.Parameters.Count);

            if (item == null)
            {
                parameter.Value = DBNull.Value;
            }
            else if (item is Guid)
            {
                parameter.Value = item.ToString();
                parameter.DbType = DbType.String;
                parameter.Size = maxSize;
            }
            else
            {
                parameter.Value = item;

                var stringItem = item as string;
                if (stringItem != null)
                    parameter.Size = stringItem.Length > maxSize ? -1 : maxSize;
            }

            command.Parameters.Add(parameter);
        }
    }
}