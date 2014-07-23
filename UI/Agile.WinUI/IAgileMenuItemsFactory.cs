namespace Agile.WinUI
{
    /// <summary>
    /// Interface for creating AgileMenuItemss
    /// </summary>
    public interface IAgileMenuItemsFactory
    {
        /// <summary>
        /// Create the menu items.
        /// </summary>
        /// <returns>Collection of AgileMenuItems.</returns>
        AgileMenuItemCollection CreateMenuItems();
    }
}