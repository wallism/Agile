namespace Agile.Framework.UI
{
    /// <summary>
    /// T is the Type of the primary key field
    /// </summary>
    public interface IAgileLookup
    {
        /// <summary>
        /// Gets the Display field
        /// </summary>
        string Display { get; }


        /// <summary>
        /// Gets the id as an int of the lookup item
        /// </summary>
        int Identifier { get; }
    }
}