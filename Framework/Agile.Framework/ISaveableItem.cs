using System;

namespace Agile.Framework
{
    public interface ISaveableItem
    {
        /// <summary>
        /// Used for client side saving so we can match up with the right item after saving (especially for Lists)
        /// </summary>
        Guid AltId { get; set; }

        /// <summary>
        /// Returns true if the item has never been saved
        /// </summary>
        bool IsNew { get; set; }

    }

    /// <summary>
    /// Saveable item extension methods
    /// </summary>
    public static class SaveableItemExtensions
    {
        /// <summary>
        /// Returns true if this is the same logic object, ie id is the same
        /// </summary>
        public static bool EqualsId(this ISaveableItem item, ISaveableItem compare)
        {
            if (compare == null)
                return false;
            if (item.AltId == Guid.Empty || compare.AltId == Guid.Empty)
                throw new Exception(string.Format("altId is null! [{0}]", item));

            return item.AltId == compare.AltId;
        }
    }
}