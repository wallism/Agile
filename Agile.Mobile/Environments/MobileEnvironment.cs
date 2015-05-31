using System.Collections.Generic;
using Agile.Shared.IoC;

namespace Agile.Mobile.Environments
{
    public interface IMobileEnvironment
    {
        /// <summary>
        /// Gets the list of services (endpoints that the system uses, often just one but also often multiple)
        /// </summary>
        List<EndpointDetail> Services { get; }

        /// <summary>
        /// Gets the name of the environment being used, e.g. Dev, Prod
        /// </summary>
        string Name { get; set; }
    }

    public class MobileEnvironment : IMobileEnvironment
    {
        /// <summary>
        /// ctor
        /// </summary>
        public MobileEnvironment(string name)
        {
            Name = name;
            Services = new List<EndpointDetail>();
        }

        public const string DEV = "DEV";
        public const string DEVFromMobile = "DEVFromMobile";
        public const string PROD = "PROD";
        public const string UAT = "UAT";

        public List<EndpointDetail> Services { get; set; }

        /// <summary>
        /// Gets the name of the environment being used, e.g. Dev, Prod
        /// (really just used for display purposes)
        /// </summary>
        public string Name { get; set; }


        private static IMobileEnvironment registeredEnvironment;

        public static IMobileEnvironment RegisteredEnvironment
        {
            get { return registeredEnvironment ?? (registeredEnvironment = Container.Resolve<IMobileEnvironment>()); }
        }

        public static bool IsProd {
            get { return RegisteredEnvironment.Name == PROD; }
        }
    }
}
