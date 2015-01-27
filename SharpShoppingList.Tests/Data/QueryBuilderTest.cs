using NUnit.Framework;
using SharpShoppingList.Data;

namespace SharpShoppingList.Tests.Data
{
    [TestFixture]
    public class QueryBuilderTest
    {
        [Test]
        public void SelectColumns()
        {
            var query = QueryBuilder.Select("col1", "col2").Build();

            Assert.That(query, Is.EqualTo("SELECT col1, col2"));
        }

        [Test]
        public void SelectColumnsWithTable()
        {
            var query = QueryBuilder
                .Select("col1", "col2")
                .From("table")
                .Build();

            Assert.That(query, Is.EqualTo("SELECT col1, col2 FROM table"));            
        }

        [Test]
        public void SelectColumnsWithTableOrderBy()
        {
            var query = QueryBuilder
                .Select("col1", "col2")
                .From("table")
                .OrderBy("col1")
                .Build();

            Assert.That(query, Is.EqualTo("SELECT col1, col2 FROM table ORDER BY col1"));            
        }

        [Test]
        public void SelectColumnsWithTableOrderByDesc()
        {
            var query = QueryBuilder
                .Select("col1", "col2")
                .From("table")
                .OrderByDescending("col1")
                .Build();

            Assert.That(query, Is.EqualTo("SELECT col1, col2 FROM table ORDER BY col1 DESC"));
        }

        [Test]
        public void SelectColumnsWithTableTake10()
        {
            var query = QueryBuilder
                .Select("col1", "col2")
                .From("table")
                .Take(10)
                .Build();

            Assert.That(query, Is.EqualTo("SELECT col1, col2 FROM table LIMIT 10"));
        }

        [Test]
        public void DeleteFrom()
        {
            var query = QueryBuilder
                .Delete()
                .From("table")
                .Build();

            Assert.That(query, Is.EqualTo("DELETE FROM table"));
        }

        [Test]
        public void DeleteFromTableWhereSingleValue()
        {
            var query = QueryBuilder
                .Delete()
                .From("table")
                .Where("col1")
                .Build();

            Assert.That(query, Is.EqualTo("DELETE FROM table WHERE col1 = ?"));
        }

        [Test]
        public void DeleteFromTableWhereMultipleValues()
        {
            var query = QueryBuilder
                .Delete()
                .From("table")
                .Where("col1", "col2", "col3")
                .Build();

            Assert.That(query, Is.EqualTo("DELETE FROM table WHERE col1 = ?, col2 = ?, col3 = ?"));
        }

        [Test]
        public void Update()
        {
            var query = QueryBuilder
                .Update("table")
                .Build();

            Assert.That(query, Is.EqualTo("UPDATE table"));
        }

        [Test]
        public void UpdateSetSingleValue()
        {
            var query = QueryBuilder
                .Update("table")
                .Set("col1")
                .Build();

            Assert.That(query, Is.EqualTo("UPDATE table SET col1 = ?"));
        }

        [Test]
        public void UpdateSetMultipleValues()
        {
            var query = QueryBuilder
                .Update("table")
                .Set("col1", "col2", "col3")
                .Build();

            Assert.That(query, Is.EqualTo("UPDATE table SET col1 = ?, col2 = ?, col3 = ?"));
        }

        [Test]
        public void InsertIntoSingleValue()
        {
            var query = QueryBuilder
                .InsertInto("table", "col1")
                .Build();

            Assert.That(query, Is.EqualTo("INSERT INTO table (col1) VALUES (?)"));
        }

        [Test]
        public void InsertIntoMultipleValues()
        {
            var query = QueryBuilder
                .InsertInto("table", "col1", "col2", "col3")
                .Build();

            Assert.That(query, Is.EqualTo("INSERT INTO table (col1, col2, col3) VALUES (?, ?, ?)"));
        }
    }
}

