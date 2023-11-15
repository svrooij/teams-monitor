namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// Status of various options in Teams
    /// </summary>
    public class MeetingState
    {
        /// <summary>
        /// Currently muted
        /// </summary>
        public bool IsMuted { get; set; }

        /// <summary>
        /// Video is on
        /// </summary>
        public bool IsVideoOn { get; set; }

        /// <summary>
        /// Hand raised
        /// </summary>
        public bool IsHandRaised { get; set; }

        /// <summary>
        /// Currently in a meeting
        /// </summary>
        public bool IsInMeeting { get; set; }

        /// <summary>
        /// Current call is recorded
        /// </summary>
        public bool IsRecordingOn { get; set; }

        /// <summary>
        /// Background blur is on
        /// </summary>
        public bool IsBackgroundBlurred { get; set; }

        /// <summary>
        /// User is sharing content
        /// </summary>
        public bool IsSharing { get; set; }

        /// <summary>
        /// User has unread messages
        /// </summary>
        public bool HasUnreadMessages { get; set; }
    }
}