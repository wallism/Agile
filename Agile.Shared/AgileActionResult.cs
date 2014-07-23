namespace Agile.Shared
{
    /// <summary>
    /// Essentially an extended 'bool' for when you want to know
    /// details of why something failed.
    /// </summary>
    public class AgileActionResult
    {
        /// <summary>
        /// The reason why the action failed.
        /// </summary>
        private readonly string failureReason;

        /// <summary>
        /// Action result constructor.
        /// </summary>
        /// <param name="reason">Reason for failure. Set to null when the action was successful.</param>
        public AgileActionResult(string reason)
        {
            failureReason = reason;
        }

        /// <summary>
        /// Instantiate a successful action result.
        /// </summary>
        public static AgileActionResult Success
        {
            get { return new AgileActionResult(null); }
        }

        /// <summary>
        /// Gets the reason why the action failed.
        /// </summary>
        public string FailureReason
        {
            get { return failureReason; }
        }

        /// <summary>
        /// Returns true if the action was successful.
        /// </summary>
        public bool WasSuccessful
        {
            get { return failureReason == null; }
        }

        /// <summary>
        /// Instantiate a failed action result with the reason for the failure.
        /// </summary>
        /// <param name="reason">Why the action failed.</param>
        /// <returns></returns>
        public static AgileActionResult Failed(string reason)
        {
            return new AgileActionResult(reason);
        }
    }
}