namespace Agile.DataAccess
{
    /// <summary>
    /// Summary description for IPrimaryKey.
    /// </summary>
    public interface IPrimaryKey
    {
        /// <summary>
        /// Gets the primary key as a where clause
        /// </summary>
        string WhereClause { get; }

        /// <summary>
        /// Gets the collection of columns that make up the Primary Key
        /// </summary>
        ColumnCollection Columns { get; }


        // Not sure about these yet, but may be useful to add
        // If we had these details, we could pass a primary key 
        // object to the DAL and use reflection to return an
        // instantiated DatabaseTable object.(not sure how useful that
        // would be for us at the moment though...)

        // string TableName{get;}
        // string DatabaseName{get;}
    }
}