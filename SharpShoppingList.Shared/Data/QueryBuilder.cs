using System.Text;

namespace SharpShoppingList.Data
{
    public class QueryBuilder
    {
        private readonly StringBuilder _query;

        private QueryBuilder()
        {
            _query = new StringBuilder();
        }

        public static QueryBuilder Delete()
        {
            return new QueryBuilder().InternalDelete();
        }

        public static QueryBuilder InsertInto(string table, params string[] columns)
        {
            return new QueryBuilder().InternalInsertInto(table, columns);
        }

        public static QueryBuilder Select(params string[] columns)
        {
            return new QueryBuilder().InternalSelect(columns);
        }

        public static QueryBuilder Update(string table)
        {
            return new QueryBuilder().InternalUpdate(table);
        }

        public string Build()
        {
            return _query.ToString();
        }

        public QueryBuilder From(string table)
        {
            _query.AppendFormat(" FROM {0}", table);
            return this;
        }

        public QueryBuilder OrderBy(string column)
        {
            _query.AppendFormat(" ORDER BY {0}", column);
            return this;
        }

        public QueryBuilder OrderByDescending(string column)
        {
            _query.AppendFormat(" ORDER BY {0} DESC", column);
            return this;
        }

        public QueryBuilder Set(params string[] columns)
        {
            _query.Append(" SET ");
            _query.Append(WildcardList(columns));
            return this;
        }

        public QueryBuilder Take(int count)
        {
            _query.AppendFormat(" LIMIT {0}", count);
            return this;
        }

        public QueryBuilder Where(params string[] columns)
        {
            _query.Append(" WHERE ");
            _query.Append(WildcardList(columns));

            return this;
        }

        private static string ValuePlaceholders(int count)
        {
            var placeholderList = new StringBuilder();
            for (var i = 0; i < count - 1; i++)
            {
                placeholderList.Append("?, ");
            }
            placeholderList.Append("?");

            return placeholderList.ToString();
        }

        private static string WildcardList(string[] columns)
        {
            var columnList = new StringBuilder();
            var count = columns.Length - 1;
            for (var i = 0; i < count; i++)
            {
                columnList.AppendFormat("{0} = ?, ", columns[i]);
            }
            columnList.AppendFormat("{0} = ?", columns[count]);
            return columnList.ToString();
        }

        private QueryBuilder InternalDelete()
        {
            _query.Append("DELETE");
            return this;
        }

        private QueryBuilder InternalInsertInto(string table, params string[] columns)
        {
            _query.AppendFormat(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                table,
                string.Join(", ", columns),
                ValuePlaceholders(columns.Length));
            return this;
        }

        private QueryBuilder InternalSelect(params string[] columns)
        {
            _query.AppendFormat("SELECT {0}", string.Join(", ", columns));
            return this;
        }

        private QueryBuilder InternalUpdate(string table)
        {
            _query.AppendFormat("UPDATE {0}", table);
            return this;
        }
    }
}