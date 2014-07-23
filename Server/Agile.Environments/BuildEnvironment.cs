using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for BuildEnvironment.
    /// </summary>
    public class BuildEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "Build"; }
        }
    }
}