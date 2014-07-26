namespace Agile.Mobile.Environments
{
    /// <summary>
    /// Environment specific details for web services 
    ///  (looking for a better name)
    /// </summary>
    public class EndpointDetail
    {
        /// <summary>
        /// Gets the name of the service to connect to ( human readable name). 
        /// This is used by HttpServiceBase to get the right endpointDetail.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string BaseUrl { get; set; }
    }
}