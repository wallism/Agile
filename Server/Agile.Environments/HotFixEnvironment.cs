using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for HotFixEnvironment.
    /// </summary>
    public class HotFixEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "HotFix"; }
        }
    }
}