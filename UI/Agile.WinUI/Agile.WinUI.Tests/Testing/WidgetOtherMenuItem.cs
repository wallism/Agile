using Agile.Common.Testing;

namespace Agile.WinUI.Tests.Testing
{
    /// <summary>
    /// WidgetMenuItem for testing use only, has two instantiate methods
    /// the first being the same as the standard WidgetMenuItem.
    /// </summary>
    public class WidgetOtherMenuItem : AgileMenuItem
    {
        /// <summary>
        /// Constructor. 
        /// </summary>
        private WidgetOtherMenuItem(Widget widget
                                    , string someOtherParameter)
            : base("WidgetOtherMenuItem")
        {
        }

        /// <summary>
        /// Instantiate with a widget. 
        /// </summary>
        public static WidgetOtherMenuItem Build(Widget widget
                                                , string someOtherParameter)
        {
            return new WidgetOtherMenuItem(widget, someOtherParameter);
        }

        /// <summary>
        /// Instantiate with a widget. 
        /// </summary>
        public static WidgetOtherMenuItem Build(Widget widget)
        {
            return Build(widget, string.Empty);
        }
    }
}