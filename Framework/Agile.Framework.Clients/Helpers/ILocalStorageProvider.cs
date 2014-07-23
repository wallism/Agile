namespace Agile.Framework.Helpers
{
    /// <summary>
    /// Implement this provider for the device you are targeting
    /// ie WP7, iPhone or Android
    /// </summary>
    public interface ILocalStorageProvider
    {
        /// <summary>
        /// Load data from the phone State
        /// </summary>
        object LoadFromState(string key);
        /// <summary>
        /// Load data from the phone Local Storage
        /// </summary>
        object LoadFromLocalStorage<T>(string key) where T : class;

        /// <summary>
        /// Save data to the phone State
        /// </summary>
        void SaveToState(string key, object value);
        /// <summary>
        /// Save data to the phone Local Storage
        /// </summary>
        void SaveToLocalStorage<T>(string key, T value) where T : class;
    }
}