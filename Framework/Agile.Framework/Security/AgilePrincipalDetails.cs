using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Agile.Framework.Security
{
    public class AgilePrincipal : IPrincipal
    {
        public static AgilePrincipal Build(IIdentity id, string userId)
        {
            return new AgilePrincipal { identity = id, userId = userId };
        }

        public static AgilePrincipal Build(AgilePrincipalDetails details)
        {
            return Build(new GenericIdentity(details.Name), details.UserId);
        }

        private string userId;
        /// <summary>
        /// The asp membership user id (guid)
        /// </summary>
        public string UserId
        {
            get { return userId; }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        /// <param name="role">The name of the role for which to check membership. 
        ///                 </param>
        public bool IsInRole(string role)
        {
            return false;
        }

        private IIdentity identity;
        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public IIdentity Identity
        {
            get { return identity; }
        }

    }

#if !SILVERLIGHT
    [Serializable] 
#endif
    public class AgilePrincipalDetails
    {
        public static AgilePrincipalDetails Build(string name, string userId)
        {
            return new AgilePrincipalDetails { Name = name, userId = userId };
        }

        private string userId;
        /// <summary>
        /// The asp membership user id (guid)
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }


        /// <summary>
        /// Get and set the user name
        /// </summary>
        public string Name { get; set; }

    }

}
