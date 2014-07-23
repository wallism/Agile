using System;
using System.Runtime.Serialization;

namespace Agile.Framework
{
    /// <summary>
    /// Base class for DTO objects
    /// </summary>
    public abstract class DtoBase
    {

        private SaveResult saveResult;
        /// <summary>
        /// Gets and sets the save result
        /// </summary>
        public virtual SaveResult SaveResult
        {
            get
            {
                // if this hasn't been set we don't want an exception to be thrown so (for now) just set to success
                if (saveResult == null)
                    saveResult = SaveResult.Success();
                return saveResult;
            }
            set { saveResult = value; }
        }
        
    }
}
