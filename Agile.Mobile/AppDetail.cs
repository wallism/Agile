using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Mobile
{
    /// <summary>
    /// Details about the app that are useful for logging and info gathering
    /// </summary>
    public class AppDetail
    {
        /// <summary>
        /// just a simple way to access for now, todo: interface it and put in container
        /// </summary>
        public static AppDetail Instance { get; set; }

        public string Name { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public string Device { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public AppDetail(string name, string os, string osVersion, string device, string country, string language, string appVersion)
        {
            Instance = this;

            Name = name;
            OS = os;
            OSVersion = osVersion;
            AppVersion = appVersion;
            Device = device;
            Country = country;
            Language = language;
        }
    }
}
