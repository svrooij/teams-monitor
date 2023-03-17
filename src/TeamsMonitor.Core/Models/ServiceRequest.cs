namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// ServiceRequest model
    /// </summary>
    public class ServiceRequest
    {
        /// <summary>
        /// Create new ServiceRequest
        /// </summary>
        /// <param name="service">Service to call</param>
        /// <param name="action">Action to call</param>
        public ServiceRequest(string service, string action)
        {
            Service = service;
            Action = action;
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds() * 1000;
        }

        /// <summary>
        /// ApiVersion
        /// </summary>
        public string ApiVersion { get; set; } = "1.0.0";

        /// <summary>
        /// Service to call
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Action to call
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Manufacturer
        /// </summary>
        public string Manufacturer { get; set; } = "Elgato";

        /// <summary>
        /// Device
        /// </summary>
        public string Device { get; set; } = "StreamDeck";

        /// <summary>
        /// Timestamp in ms since epoch
        /// </summary>
        public long Timestamp { get; set; }
    }
}