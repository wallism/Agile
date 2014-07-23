using Agile.Common.Testing;

namespace Agile.WinUI.Tests.Testing
{
    /// <summary>
    /// WidgetMenuItem for testing use only.
    /// </summary>
    public class WidgetMenuItem : AgileMenuItem
    {
        /// <summary>
        /// Constructor. 
        /// </summary>
        private WidgetMenuItem(Widget widget)
            : base("WidgetMenuItem")
        {
        }

        /// <summary>
        /// Instantiate a new menu item with a widget. 
        /// </summary>
        public static WidgetMenuItem Build(Widget widget)
        {
            return new WidgetMenuItem(widget);
        }
    }
}