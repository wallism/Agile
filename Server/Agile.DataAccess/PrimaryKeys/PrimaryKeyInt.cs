using System.Text;
using Agile.Shared;

namespace Agile.DataAccess
{
    /// <summary>
    /// Summary description for PrimaryKeyInt.
    /// </summary>
    public class PrimaryKeyInt : IPrimaryKey
    {
        /// <summary>
        /// Value of the Primary Key
        /// </summary>
        private readonly ColumnCollection _columns;

        /// <summary>
        /// Instantiate a new varchar primary key with the value of the key.
        /// </summary>
        /// <param name="columns">Columns that make up the primary key.</param>
        internal protected PrimaryKeyInt(ColumnCollection columns)
        {
            ArgumentValidation.CheckForNullReference(columns, "columns");

            _columns = columns;
        }

        #region IPrimaryKey Members

        /// <summary>
        /// Gets the primary key as a where clause
        /// </summary>
        string IPrimaryKey.WhereClause
        {
            get
            {
                var whereClause = new StringBuilder("WHERE ");
                foreach (Column column in _columns)
                {
                    whereClause.Append(string.Format(@"
AND  {0} = {1}"
                                                     , column.Name
                                                     , column.Value));
                }
                return whereClause.ToString().RemoveFirstInstanceOf("AND");
            }
        }

        /// <summary>
        /// Gets the collection of columns that make up the Primary Key
        /// </summary>
        ColumnCollection IPrimaryKey.Columns
        {
            get { return _columns; }
        }

        #endregion

        /// <summary>
        /// Instantiate a new varchar primary key with columns that make up the key.
        /// </summary>
        /// <param name="columns">Columns that make up the primary key.</param>
        /// <returns>VarcharPrimaryKey</returns>
        public static PrimaryKeyInt Build(ColumnCollection columns)
        {
            return new PrimaryKeyInt(columns);
        }

        /// <summary>
        /// Instantiate a new varchar primary key with columns that make up the key.
        /// </summary>
        /// <param name="columnName">Name of the primary key column.
        /// May NOT be null.</param>
        /// <param name="primaryKeyValue">Value of the records primary key.
        /// Value may be null.</param>
        /// <returns>VarcharPrimaryKey</returns>
        public static PrimaryKeyInt Build(string columnName, object primaryKeyValue)
        {
            ArgumentValidation.CheckForNullReference(columnName, "columnName");
            ArgumentValidation.CheckForEmptyString(columnName, "columnName");

            Column key = Column.Build(columnName, primaryKeyValue);
            ColumnCollection keys = ColumnCollection.Build();
            keys.Add(key);
            return Build(keys);
        }
    }
    
    /// <summary>
    /// PrimaryKeySmallint.
    /// </summary>
    public class PrimaryKeySmallint : PrimaryKeyInt
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeySmallint(ColumnCollection columns) : base(columns){}

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeySmallint Build(ColumnCollection columns)
        {
            return new PrimaryKeySmallint(columns);
        }
    }


    /// <summary>
    /// PrimaryKeyTinyint.
    /// </summary>
    public class PrimaryKeyTinyint : PrimaryKeyInt
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeyTinyint(ColumnCollection columns) : base(columns) { }

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeyTinyint Build(ColumnCollection columns)
        {
            return new PrimaryKeyTinyint(columns);
        }
    }

    /// <summary>
    /// PrimaryKeyBigint.
    /// </summary>
    public class PrimaryKeyBigint : PrimaryKeyInt
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeyBigint(ColumnCollection columns) : base(columns) { }

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeyBigint Build(ColumnCollection columns)
        {
            return new PrimaryKeyBigint(columns);
        }
    }

}