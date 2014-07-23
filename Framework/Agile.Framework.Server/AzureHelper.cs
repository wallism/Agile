using System;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Agile.Framework.Server
{

    public enum RuntimeEnvironment
    {
        AzureFabric = 0,
        DevFabric = 1,
        NoFabric = 2
    }

    public static class AzureHelper
    {
        public static RuntimeEnvironment RuntimeEnvironment
        {
            get
            {
                try
                {
                    if (RoleEnvironment.IsAvailable)
                    {
                        return IsRunningInDevFabric
                            ? RuntimeEnvironment.DevFabric
                            : RuntimeEnvironment.AzureFabric;
                    }
                    return RuntimeEnvironment.NoFabric;
                }
                catch (Exception)
                {
                    // do nothing with ex is fine.
                    // don't know of another way to test if not running in fabric at all.
                    // most of the time will be running in either the dev fabric or the azureFabric (prod)
                    return RuntimeEnvironment.NoFabric;
                }
            }
        }

        public static bool IsRunningInDevFabric
        {
            get
            {
                // easiest check: try translate deployment ID into guid
                // note: this check works fine with the current sdk but may fail in future sdks if the Id generation changes
                // taken from http://www.davidmakogon.com/2010/08/azure-tip-of-day-determine-if-running.html
                Guid guidId;
                return !Guid.TryParse(RoleEnvironment.DeploymentId, out guidId);
            }
        }

        /// <summary>
        /// Is either running in the Dev or Azure fabric
        /// </summary>
        public static bool IsInFabric
        {
            get { return RuntimeEnvironment != RuntimeEnvironment.NoFabric; }
        }
    }
}

