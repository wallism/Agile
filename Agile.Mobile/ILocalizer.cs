using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Mobile
{
    /// <summary>
    /// Implement to return localized strings from AppResource manager
    /// </summary>
    public interface ILocalizer
    {
        string Get(string name, string resourceName = null);
    }
}
