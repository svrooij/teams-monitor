namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// TeamsMessage
    /// </summary>
    public class TeamsMessage
    {
        /// <summary>
        /// MeetingUpdate
        /// </summary>
        public MeetingUpdate? MeetingUpdate { get; set; }

        /// <summary>
        /// If present this is a response to a request
        /// </summary>
        public int? RequestId { get; set; }

        /// <summary>
        /// If the requestId is present this is the response to the service call
        /// </summary>
        public string? Response { get; set; }

        /// <summary>
        /// If TokenRefresh is present, the next time the application connects to Teams is has to use this new token
        /// </summary>
        public string? TokenRefresh { get; set; }
    }
}