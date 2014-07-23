using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Agile.Shared;

namespace Agile.Framework
{
    /// <summary>
    /// Generic (default) FaultContract for Agile Framework WCF Service OperationContracts.
    /// </summary>
    [Obsolete("do not use with REST ")]
    public class AgileServiceFault
    {
        /// <summary>
        /// ctor
        /// </summary>
        private AgileServiceFault(string failReason)
        {
            FailReason = failReason;
        }

        /// <summary>
        /// Create a new AgileServiceFault
        /// </summary>
        public static AgileServiceFault Build(string failReason)
        {
            return new AgileServiceFault(failReason);
        }

        /// <summary>
        /// Create a new AgileServiceFault
        /// </summary>
        public static AgileServiceFault Build(AgileActionResult result)
        {
            return new AgileServiceFault(result.FailureReason);
        }

        /// <summary>
        /// Gets the name of the failReason.
        /// </summary>
        public string FailReason { get; set; }
    }
}
