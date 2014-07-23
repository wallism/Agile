namespace Agile.Shared
{
    /// <summary>
    /// Summary description for IObserver.
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Notify the observer of a change, given the details of the change.
        /// </summary>
        /// <param name="state">Details the observer will want to know about.</param>
        void Notify(object state);
    }
}