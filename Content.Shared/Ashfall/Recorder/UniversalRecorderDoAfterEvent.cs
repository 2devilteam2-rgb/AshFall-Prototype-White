using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Ashfall.Recorder;

[Serializable, NetSerializable]
public sealed partial class UniversalRecorderTapeRespoolDoAfterEvent : SimpleDoAfterEvent;
