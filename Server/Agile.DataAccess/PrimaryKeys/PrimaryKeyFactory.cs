using System;

namespace Agile.DataAccess.PrimaryKeys
{
    /// <summary>
    /// Summary description for PrimaryKeyFactory.
    /// </summary>
    public sealed class PrimaryKeyFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private PrimaryKeyFactory()
        {
        }


        /// <summary>
        /// Instantiate a primary key object for a table that has only one field as it's key.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="primaryKeyValue"></param>
        /// <returns>An IPrimary key, typed based on the data type of the primary keys value.</returns>
        public static IPrimaryKey Build(string columnName, object primaryKeyValue)
        {
//TODO: potential bug here if the datatype in the db is varchar but the value is actually a number.

            if (primaryKeyValue is int)
                return PrimaryKeyInt.Build(columnName, primaryKeyValue);
            if (primaryKeyValue is string)
                return PrimaryKeyVarchar.Build(columnName, primaryKeyValue);
            else
                throw new NotImplementedException("A PrimaryKey type does not exist for type: " +
                                                  primaryKeyValue.GetType());
        }
    }
}