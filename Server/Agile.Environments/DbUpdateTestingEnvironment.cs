using Agile.Common.Environments;

namespace Agile.Environments
{
    /// <summary>
    /// Summary description for DbUpdateTestingEnvironment.
    /// </summary>
    public class DbUpdateTestingEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "TestEnvironmentFileDoNotRemove"; }
        }

        /// <summary>
        /// Override of ToString because this environment needs to return this...
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "TestEnvironmentFileDoNotRemove";
        }
    }
}