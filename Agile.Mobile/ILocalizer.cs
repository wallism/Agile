using System;

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
