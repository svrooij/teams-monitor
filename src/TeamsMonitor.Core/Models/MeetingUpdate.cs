namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// MeetingUpdate
    /// </summary>
    public class MeetingUpdate
    {
        /// <summary>
        /// MeetingState
        /// </summary>
        public MeetingState? MeetingState { get; set; }
        /// <summary>
        /// MeetingPermissions
        /// </summary>
        public MeetingPermissions? MeetingPermissions { get; set; }
    }
}