namespace Agile.WinUI.Tests.Testing
{
    /// <summary>
    /// WidgetBogusMenuItem for testing use only.
    /// </summary>
    public class WidgetBogusMenuItem : AgileMenuItem
    {
        /// <summary>
        /// Constructor. 
        /// </summary>
        private WidgetBogusMenuItem(string doesntTakeAWidget)
            : base("WidgetBogusMenuItem")
        {
        }

        /// <summary>
        /// Instantiate with a widget. 
        /// </summary>
        public static WidgetBogusMenuItem Build(string doesntTakeAWidget)
        {
            return new WidgetBogusMenuItem(doesntTakeAWidget);
        }
    }
}