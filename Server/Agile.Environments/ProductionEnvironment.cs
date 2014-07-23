using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for ProductionEnvironment.
    /// </summary>
    public class ProductionEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "PROD"; }
        }
    }
}