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
        /// <param name="action">Action to call</param>
        /// <param name="requestId">Request ID</param>
        /// <param name="additionalData">Data to be send as Parameters</param>
        public ServiceRequest(string action, int requestId = 0, object? additionalData = null)
        {
            Action = action;
            RequestId = requestId;
            Parameters = additionalData ?? new { };
        }

        /// <summary>
        /// Action to call
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Request ID
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Additional parameters for service call
        /// </summary>
        public object Parameters { get; set; }
    }
}