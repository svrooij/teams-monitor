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

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is MeetingState other)
            {
                return IsMuted == other.IsMuted &&
                       IsVideoOn == other.IsVideoOn &&
                       IsHandRaised == other.IsHandRaised &&
                       IsInMeeting == other.IsInMeeting &&
                       IsRecordingOn == other.IsRecordingOn &&
                       IsBackgroundBlurred == other.IsBackgroundBlurred &&
                       IsSharing == other.IsSharing &&
                       HasUnreadMessages == other.HasUnreadMessages;
            }

            return false;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + IsMuted.GetHashCode();
                hash = hash * 23 + IsVideoOn.GetHashCode();
                hash = hash * 23 + IsHandRaised.GetHashCode();
                hash = hash * 23 + IsInMeeting.GetHashCode();
                hash = hash * 23 + IsRecordingOn.GetHashCode();
                hash = hash * 23 + IsBackgroundBlurred.GetHashCode();
                hash = hash * 23 + IsSharing.GetHashCode();
                hash = hash * 23 + HasUnreadMessages.GetHashCode();
                return hash;
            }
        }


    }
}