using Content.Shared.Ashfall.Recorder;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Ashfall.Recorder;

[RegisterComponent]
[Access(typeof(UniversalRecorderSystem))]
public sealed partial class UniversalRecorderRuntimeComponent : Component
{
    [DataField]
    public UniversalRecorderState State = UniversalRecorderState.Stopped;

    [DataField]
    public bool WarningSent;

    [DataField]
    public TimeSpan RecordingBaseOffset = TimeSpan.Zero;

    [DataField]
    public TimeSpan RecordingStartedAt = TimeSpan.Zero;

    [DataField]
    public TimeSpan NextPlaybackAt = TimeSpan.Zero;

    [DataField]
    public TimeSpan NextPrintAt = TimeSpan.Zero;

    [DataField]
    public int PlaybackIndex;

    [DataField]
    public int? PendingSilenceSeconds;

    [DataField]
    public TimeSpan HissLoopStartAt = TimeSpan.Zero;

    [DataField]
    public bool WaitingForHissLoop;

    [DataField]
    public EntityUid? PlaybackStartStream;

    [DataField]
    public EntityUid? PlaybackStream;
}
