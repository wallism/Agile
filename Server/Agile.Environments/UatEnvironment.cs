using Agile.Environments;

namespace Agile.Common.Environments
{
    /// <summary>
    /// Summary description for UatEnvironment.
    /// </summary>
    public class UatEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "UAT"; }
        }
    }
}