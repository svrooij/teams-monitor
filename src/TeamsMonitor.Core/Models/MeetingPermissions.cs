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
    }
}