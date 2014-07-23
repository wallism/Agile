using System;
using System.Collections;

namespace Agile.DataAccess
{
    /// <summary>
    /// Collection of database table columns.
    /// </summary>
    public class ColumnCollection : CollectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private ColumnCollection()
        {
        }


        /// <summary>
        /// Gets the column from the collection that is at the given ordinal position.
        /// </summary>
        public Column this[int index]
        {
            get { return (Column) InnerList[index]; }
        }

        /// <summary>
        /// Gets the column from the collection with the given name.
        /// </summary>
        /// <remarks>Throws an exception if the column does not exist in the collection.</remarks>
        public Column this[string columnName]
        {
            get
            {
                Column column = GetColumn(columnName);
                if (column == null)
                {
                    throw new Exception(string.Format("Column '{0}' does not exist in the collection."
                                                                 , columnName));
                }
                return column;
            }
        }

        /// <summary>
        /// Instantiate a new empty Column Collection.
        /// </summary>
        /// <returns></returns>
        public static ColumnCollection Build()
        {
            return new ColumnCollection();
        }

        /// <summary>
        /// Gets the column from the collection with the given name.
        /// </summary>
        /// <remarks>Returns null if the column does not exist in the collection.</remarks>
        public Column GetColumn(string columnName)
        {
            foreach (Column column in this)
            {
                if (column.Name.Equals(columnName))
                    return column;
            }
            return null;
        }

//		#region Overrides
//
//		/// <summary>
//		/// Initialises the DataTable from the collection.
//		/// </summary>
//		protected override DataTable InitializeDataTable()
//		{
//			DataTable table = new DataTable("Column");
//			foreach (Column item in this)
//			{
//				DataRow row = table.NewRow();
//				row["Name"] = item.Name;
//				row["Value"] = item.Value;
//				table.Rows.Add(row);
//			}
//			return table;
//		}
//
//
//		#endregion

        /// <summary>
        /// Add a column the collection.
        /// </summary>
        /// <param name="column"></param>
        public void Add(Column column)
        {
            InnerList.Add(column);
        }
    }
}