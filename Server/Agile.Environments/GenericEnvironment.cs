using System;
using System.Configuration;
using System.IO;
using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for GenericEnvironment. e.g. YourName
    /// </summary>
    public class GenericEnvironment : AgileEnvironment
    {


        /// <summary>
        /// Gets the Name of the enviroment, e.g. Generic or UAT
        /// </summary>
        public override string Name
        {
            get { return "Generic"; }
        }

    }
}