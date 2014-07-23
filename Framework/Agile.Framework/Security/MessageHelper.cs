using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Framework.Security
{
    public static class MessageHelper
    {
        /// <summary>
        /// Header Key (xml element name) where the users password is stored for validation 
        /// </summary>
        public const string HeaderUserAuthenticationKey = "UAK";
        public const string HeaderNamespace = "bizzymee.com.au.security";

        public const string DefaultValue = "KiK01ter6W1AfANDUjowqA=";
    }
}
