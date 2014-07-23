namespace Agile.DataAccess
{
    /// <summary>
    /// Description of a database table column
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the field in the database. (exact match)</param>
        /// <param name="columnValue">the columns value as stored in the database (or to be stored).</param>
        private Column(string name, object columnValue)
        {
            _name = name;
            Value = columnValue;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the value of the column.
        /// </summary>
        public object Value { // TODO: review the type, it might need to be 'object' or we may need to create a class.
            get; set; }

        /// <summary>
        /// Instantiate a new column with all required details.
        /// </summary>
        /// <param name="columnName">Name of the field in the database. (exact match)</param>
        /// <param name="columnValue">the columns value as stored in the database (or to be stored).</param>
        public static Column Build(string columnName, object columnValue)
        {
            return new Column(columnName, columnValue);
        }
    }
}