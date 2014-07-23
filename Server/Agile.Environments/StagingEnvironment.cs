
namespace Agile.Environments
{
    /// <summary>
    /// Summary description for StagingEnvironment.
    /// </summary>
    public class StagingEnvironment : AgileEnvironment
    {
        /// <summary>
        /// Gets the Name of the enviroment, e.g. Development or UAT
        /// </summary>
        public override string Name
        {
            get { return "Staging"; }
        }
    }
}