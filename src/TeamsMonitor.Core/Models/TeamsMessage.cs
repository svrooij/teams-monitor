namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// TeamsMessage
    /// </summary>
    public class TeamsMessage
    {
        /// <summary>
        /// ApiVersion
        /// </summary>
        public string? ApiVersion { get; set; }

        /// <summary>
        /// MeetingUpdate
        /// </summary>
        public MeetingUpdate? MeetingUpdate { get; set; }
    }
}