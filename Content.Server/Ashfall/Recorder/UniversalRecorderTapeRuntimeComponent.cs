using Content.Shared.Ashfall.Recorder;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Ashfall.Recorder;

[RegisterComponent]
[Access(typeof(UniversalRecorderSystem))]
public sealed partial class UniversalRecorderTapeRuntimeComponent : Component
{
    [DataField]
    public UniversalRecorderTapeSide Side = UniversalRecorderTapeSide.Front;

    [DataField]
    public TimeSpan UsedCapacity = TimeSpan.Zero;

    [DataField]
    public TimeSpan OtherSideUsedCapacity = TimeSpan.Zero;

    [DataField]
    public bool Unspooled;

    [DataField]
    public string? FrontName;

    [DataField]
    public string? BackName;

    [DataField]
    public List<RecorderEntry> Entries = new();

    [DataField]
    public List<RecorderEntry> OtherSideEntries = new();
}
