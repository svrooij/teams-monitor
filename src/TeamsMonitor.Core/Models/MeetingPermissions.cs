namespace TeamsMonitor.Core.Models
{
    /// <summary>
    /// Permissions you have in the current call
    /// </summary>
    public class MeetingPermissions
    {
        /// <summary>
        /// Allowed to mute/unmute
        /// </summary>
        public bool CanToggleMute { get; set; }

        /// <summary>
        /// Allowed to toggle video
        /// </summary>
        public bool CanToggleVideo { get; set; }

        /// <summary>
        /// Allowed to Raise hand
        /// </summary>
        public bool CanToggleHand { get; set; }

        /// <summary>
        /// Allowed to toggle blur
        /// </summary>
        public bool CanToggleBlur { get; set; }

        /// <summary>
        /// Allowed to leave
        /// </summary>
        /// <remarks>When would this not be allowed?</remarks>
        public bool CanLeave { get; set; }

        /// <summary>
        /// Allowed to react in current call
        /// </summary>
        public bool CanReact { get; set; }

        /// <summary>
        /// Allowed to toggle share tray
        /// </summary>
        public bool CanToggleShareTray { get; set; }

        /// <summary>
        /// Allowed to toggle chat
        /// </summary>
        public bool CanToggleChat { get; set; }

        /// <summary>
        /// Allowed to stop sharing
        /// </summary>
        public bool CanStopSharing { get; set; }

        /// <summary>
        /// Allowed to pair
        /// </summary>
        public bool CanPair { get; set; }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is MeetingPermissions other)
            {
                return CanToggleMute == other.CanToggleMute &&
                       CanToggleVideo == other.CanToggleVideo &&
                       CanToggleHand == other.CanToggleHand &&
                       CanToggleBlur == other.CanToggleBlur &&
                       CanLeave == other.CanLeave &&
                       CanReact == other.CanReact &&
                       CanToggleShareTray == other.CanToggleShareTray &&
                       CanToggleChat == other.CanToggleChat &&
                       CanStopSharing == other.CanStopSharing &&
                       CanPair == other.CanPair;
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
                hash = hash * 23 + CanToggleMute.GetHashCode();
                hash = hash * 23 + CanToggleVideo.GetHashCode();
                hash = hash * 23 + CanToggleHand.GetHashCode();
                hash = hash * 23 + CanToggleBlur.GetHashCode();
                hash = hash * 23 + CanLeave.GetHashCode();
                hash = hash * 23 + CanReact.GetHashCode();
                hash = hash * 23 + CanToggleShareTray.GetHashCode();
                hash = hash * 23 + CanToggleChat.GetHashCode();
                hash = hash * 23 + CanStopSharing.GetHashCode();
                hash = hash * 23 + CanPair.GetHashCode();
                return hash;
            }
        }
    }
}