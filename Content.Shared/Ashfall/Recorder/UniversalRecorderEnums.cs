using Robust.Shared.Serialization;

namespace Content.Shared.Ashfall.Recorder;

[Serializable, NetSerializable]
public enum UniversalRecorderState : byte
{
    Stopped,
    Recording,
    Playing,
}

[Serializable, NetSerializable]
public enum UniversalRecorderVisuals : byte
{
    State,
}

[Serializable, NetSerializable]
public enum UniversalRecorderVisualLayers : byte
{
    Base,
}

[Serializable, NetSerializable]
public enum UniversalRecorderVisualState : byte
{
    Empty,
    Idle,
    Recording,
    Playing,
}

[Serializable, NetSerializable]
public enum UniversalRecorderTapeSide : byte
{
    Front,
    Back,
}

[Serializable, NetSerializable]
public enum UniversalRecorderTapeVisuals : byte
{
    Side,
    Unspooled,
}

[Serializable, NetSerializable]
public enum UniversalRecorderTapeVisualLayers : byte
{
    Base,
    Ribbon,
}
