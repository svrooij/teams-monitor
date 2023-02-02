public class TeamsApiResponse {
    public string? ApiVersion { get; set; }
    public TeamsMeetingUpdate? MeetingUpdate { get; set; }
}

public class TeamsMeetingUpdate {
    public TeamsMeetingState? MeetingState { get; set; }
    public TeamsMeetingPermissions? MeetingPermissions { get; set; }
}

public class TeamsMeetingState {
    public bool IsMuted { get; set; }
    public bool IsCameraOn { get; set; }
    public bool IsHandRaised { get; set; }
    public bool IsInMeeting { get; set; }
    public bool IsRecordingOn { get; set; }
    public bool IsBackgroundBlurred { get; set; }
}

public class TeamsMeetingPermissions {
    public bool CanToggleMute { get; set; }
    public bool CanToggleVideo { get; set; }
    public bool CanToggleHand { get; set; }
    public bool CanToggleBlur { get; set; }
    public bool CanToggleRecord { get; set; }
    public bool CanLeave { get; set; }
    public bool CanReact { get; set; }
}