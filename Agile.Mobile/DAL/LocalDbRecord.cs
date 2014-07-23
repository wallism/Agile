using System;
using Agile.Shared;
using Agile.Shared.IoC;
using SQLite.Net.Attributes;

namespace Agile.Mobile.DAL
{
    /// <summary>
    /// Created this class because some things ARE
    /// common to all tables
    /// </summary>
    public abstract class LocalDbRecord
    {
        /// <summary>
        /// ctor
        /// </summary>
        public LocalDbRecord()
        {
            Created = AgileDateTime.UtcNow;
            Updated = Created; // important to be same as Created.
            AltId = Guid.NewGuid();
        }

        public override string ToString()
        {
            return string.Format("[{0} Id:{1}]", GetType().Name, GetId());
        }

        public abstract long GetId();
        public abstract void SetId(long id);

        /// <summary>
        ///  because this is a remote local db when we create records we
        /// don't get the actual id until it gets saved on the server 
        /// (all our tables use bigint autoincrements)
        /// </summary>
        public Guid AltId { get; set; }

        /// <summary>
        /// In code, Get and Set Created NOT this. This is here for SQLite to use
        /// </summary>
        public string CreatedUtc { get; set; }

        [Ignore]
        public DateTimeOffset Created
        {
            get { return Safe.DateTimeOffset(CreatedUtc, AgileDateTime.UtcNow.AddSeconds(1)); } // adding a second for now, mostly for testing, to ensure the value is geting set properly (shouldn't hit the default)
            set { CreatedUtc = value.ToString("o"); }
        }


        /// <summary>
        /// In code, Get and Set Updated NOT this. This is here for SQLite to use
        /// </summary>
        public string UpdatedUtc { get; set; }

        [Ignore]
        public DateTimeOffset? Updated
        {
            get { return Safe.NullableDateTimeOffset(UpdatedUtc); } // adding a second for now, mostly for testing, to ensure the value is geting set properly (shouldn't hit the default)
            set 
            {
                UpdatedUtc = (value.HasValue)
                    ? value.Value.ToString("o") : null; 
            }
        }







        #region Equals override - matches on Id
        
        /// <summary>
        /// Override of Equals operator. Evaluate if the id is different (important for Lookup types, especially when consider serialization)
        /// </summary>
        public static bool operator ==(LocalDbRecord a, LocalDbRecord b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;
            return a.Equals(b);
        }

        /// <summary>
        /// Override of NotEquals operator
        /// </summary>
        public static bool operator !=(LocalDbRecord a, LocalDbRecord b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Override of GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return GetId().GetHashCode();
        }

        /// <summary>
        /// Override Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is LocalDbRecord))
                return false;
            return GetId() == ((LocalDbRecord)obj).GetId();
            
        }

        #endregion

    }
}
