using System.Text;
using Agile.Shared;

namespace Agile.DataAccess
{
    /// <summary>
    /// Summary description for VarcharPrimaryKey.
    /// </summary>
    public class PrimaryKeyVarchar : IPrimaryKey
    {
        /// <summary>
        /// Value of the Primary Key
        /// </summary>
        private readonly ColumnCollection _columns;

        /// <summary>
        /// Instantiate a new varchar primary key with the value of the key.
        /// </summary>
        /// <param name="columns">Columns that make up the primary key.</param>
        internal protected PrimaryKeyVarchar(ColumnCollection columns)
        {
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
AND  {0} = '{1}'"
                                                     , column.Name
                                                     , column.Value));
                }
                return whereClause.ToString().RemoveFirstInstanceOf("AND ");
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
        public static PrimaryKeyVarchar Build(ColumnCollection columns)
        {
            return new PrimaryKeyVarchar(columns);
        }

        /// <summary>
        /// Instantiate a new varchar primary key with columns that make up the key.
        /// </summary>
        /// <param name="columnName">Name of the primary key column.
        /// May NOT be null.</param>
        /// <param name="primaryKeyValue">Value of the records primary key.
        /// Value may be null.</param>
        /// <returns>VarcharPrimaryKey</returns>
        public static PrimaryKeyVarchar Build(string columnName, object primaryKeyValue)
        {
            Column key = Column.Build(columnName, primaryKeyValue);
            ColumnCollection keys = ColumnCollection.Build();
            keys.Add(key);
            return Build(keys);
        }
    }

    /// <summary>
    /// PrimaryKeyNvarchar
    /// </summary>
    public class PrimaryKeyNvarchar : PrimaryKeyVarchar
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeyNvarchar(ColumnCollection columns) : base(columns) { }

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeyNvarchar Build(ColumnCollection columns)
        {
            return new PrimaryKeyNvarchar(columns);
        }
    }

    /// <summary>
    /// PrimaryKeyNchar
    /// </summary>
    public class PrimaryKeyNchar : PrimaryKeyVarchar
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeyNchar(ColumnCollection columns) : base(columns) { }

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeyNchar Build(ColumnCollection columns)
        {
            return new PrimaryKeyNchar(columns);
        }
    }

    /// <summary>
    /// PrimaryKeyChar
    /// </summary>
    public class PrimaryKeyChar : PrimaryKeyVarchar
    {
        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        internal protected PrimaryKeyChar(ColumnCollection columns) : base(columns) { }

        /// <summary>
        /// Instantiate a new primary key
        /// </summary>
        public new static PrimaryKeyChar Build(ColumnCollection columns)
        {
            return new PrimaryKeyChar(columns);
        }
    }
}