using Agile.Diagnostics.Logging;
using Agile.Shared.IoC;

namespace Agile.Web.Rest
{
    /// <summary>
    /// System functions.
    /// </summary>
    /// <remarks>Create a partial class that 'inherits':  : NancyModuleBase~ErrorLogRepository, ErrorLog~ 
    /// 
    /// Don't include in Agile.Web.Rest.csproj, instead add it as a linked file to application module project.
    /// (added the file here to make it kind of shareable across multiple systems, can't add it for real because in
    /// its current form it needs a reference to app specific repo's and biz library)</remarks>
    public partial class SystemModule
    {
        /// <summary>
        /// ctor
        /// </summary>
        public SystemModule() : base("")
        {
            Get["/"] = parameters => //"Hello world";
            {
                Logger.Debug("SystemModule");
                return View["main"];
            };
            Get["/version"] = parameters => NancyHelper.GetVersion();
            Get["/info"] = parameters => NancyHelper.GetInfo();
            Post["/logerror"] = parameters => PostRoute();

            DefineRoutes();
        }
        
    }
}
