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

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is MeetingUpdate other)
            {
                return Equals(MeetingState, other.MeetingState) &&
                       Equals(MeetingPermissions, other.MeetingPermissions);
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
                hash = hash * 23 + (MeetingState?.GetHashCode() ?? 0);
                hash = hash * 23 + (MeetingPermissions?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}